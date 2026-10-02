using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Pipes;
using System.Text.Json;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Runtime.Versioning;			// ワーニング対策


namespace ColorImageViewer
{
	[SupportedOSPlatform("windows6.1")]         // ワーニング対策
	static class Program
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetNamedPipeServerProcessId(SafePipeHandle pipe, out uint serverProcessId);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool AllowSetForegroundWindow(uint processId);

		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetHighDpiMode(HighDpiMode.SystemAware);
//			Application.SetCompatibleTextRenderingDefault(false);

			using (var mutex = new Mutex(true, "MtxImageViewerApp", out var createdNew))
			{
				if (createdNew)
				{
					// 初回起動 - サーバーとして動作
					var mainForm = new FormMain(args);
					StartPipeServer(mainForm);
					Application.Run(mainForm);
					mutex.ReleaseMutex();
				}
				else
				{
					// 既に起動している場合 - 引数を既存インスタンスに送信。引数なしの場合はカウンタクリアしてメインフォーム表示する。
					SendCommandToExistingInstanceAsync(args).GetAwaiter().GetResult();
				}
			}
		}


		/// <summary>
		/// パイプサーバースレッドを起動します</summary>
		/// <param name="mainForm">メインフォーム</param>
        private static void StartPipeServer(FormMain mainForm)
        {
            Task.Run(async () =>
            {
                while (!mainForm.IsDisposed)
                {
                    try
                    {
                        using (var server = new NamedPipeServerStream("MyImageViewerPipe", PipeDirection.In))
                        {
                            await server.WaitForConnectionAsync();

                            using (var reader = new StreamReader(server))
                            {
                                var jsonMessage = await reader.ReadToEndAsync();
                                var message = JsonSerializer.Deserialize<PipeMessage>(jsonMessage);
								if (message == null) continue;

								if (!mainForm.IsDisposed && mainForm.IsHandleCreated)
								{
									// UIスレッドで実行
									mainForm.Invoke(() =>
										{
											switch (message.Command)
											{
												case PipeCommands.OpenFiles:
													mainForm.OpenFiles(message.Files, false);
													break;
												case PipeCommands.ClearCounter:
													mainForm.ClearCounter();
													break;
												case PipeCommands.OpenClipboardImage:
													mainForm.OpenClipboardImage(false);
													break;
											}
										});
								}
							}
						}
					}
					catch (Exception ex)
                    {
                        Console.WriteLine($"Pipe server error: {ex.Message}");
                    }
                }
            });
        }


		/// <summary>
		/// 引数を指定してパイプへのコマンド送信を行います</summary>
		/// <param name="args">起動引数</param>
		private static async Task SendCommandToExistingInstanceAsync(string[] args)
        {
            try
            {
                using (var client = new NamedPipeClientStream(".", "MyImageViewerPipe", PipeDirection.Out))
                {
                    await client.ConnectAsync(5000);

					// ユーザー操作で起動されたプロセスから、既存ビューアーに前面表示を許可する。
					if (GetNamedPipeServerProcessId(client.SafePipeHandle, out var serverProcessId))
					{
						AllowSetForegroundWindow(serverProcessId);
					}

                    var message = CreatePipeMessage(args);

                    var jsonMessage = JsonSerializer.Serialize(message);
                    using (var writer = new StreamWriter(client))
                    {
                        await writer.WriteAsync(jsonMessage);
                        await writer.FlushAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"既存のインスタンスとの通信に失敗しました: {ex.Message}");
            }
        }


		/// <summary>
		/// 起動引数からNamed Pipe用メッセージを生成します</summary>
		/// <param name="args">起動引数</param>
		/// <returns>Named Pipe用メッセージ</returns>
		internal static PipeMessage CreatePipeMessage(string[] args)
		{
			var values = args?.Where(w => String.IsNullOrWhiteSpace(w) == false).ToArray() ?? Array.Empty<string>();
			if (values.Any(IsOpenClipboardImageArgument))
			{
				return new PipeMessage { Command = PipeCommands.OpenClipboardImage };
			}

			if (values.Length > 0)
			{
				return new PipeMessage { Command = PipeCommands.OpenFiles, Files = values };
			}

			return new PipeMessage { Command = PipeCommands.ClearCounter };
		}


		/// <summary>
		/// クリップボード画像オープン用引数かどうかを取得します</summary>
		/// <param name="arg">起動引数</param>
		/// <returns>クリップボード画像オープン用引数の場合はtrue</returns>
		private static bool IsOpenClipboardImageArgument(string arg)
		{
			return String.Equals(arg, "--clipboard", StringComparison.OrdinalIgnoreCase)
				|| String.Equals(arg, "/clipboard", StringComparison.OrdinalIgnoreCase)
				|| String.Equals(arg, "-clipboard", StringComparison.OrdinalIgnoreCase)
				|| String.Equals(arg, "--paste", StringComparison.OrdinalIgnoreCase)
				|| String.Equals(arg, "/paste", StringComparison.OrdinalIgnoreCase)
				|| String.Equals(arg, "-paste", StringComparison.OrdinalIgnoreCase);
		}
    }

	/// <summary>Named Pipeコマンド名を保持します</summary>
	internal static class PipeCommands
	{
		/// <summary>画像ファイルを開くコマンド名</summary>
		public const string OpenFiles = "open_files";

		/// <summary>カウンタをクリアするコマンド名</summary>
		public const string ClearCounter = "clear_counter";

		/// <summary>クリップボード画像を開くコマンド名</summary>
		public const string OpenClipboardImage = "open_clipboard_image";
	}


	/// <summary>Named Pipeメッセージを保持します</summary>
    public class PipeMessage
    {
		/// <summary>Named Pipeコマンド名を保持します</summary>
        public string Command { get; set; } = PipeCommands.ClearCounter;

		/// <summary>画像ファイルパス群を保持します</summary>
        public string[] Files { get; set; } = Array.Empty<string>();
    }
}
