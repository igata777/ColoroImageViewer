using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Pipes;
using System.Text.Json;
using System.IO;
using System.Runtime.Versioning;			// ワーニング対策


namespace ColorImageViewer
{
	[SupportedOSPlatform("windows6.1")]         // ワーニング対策
	static class Program
	{
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
					SendFilesToExistingInstanceAsync(args).GetAwaiter().GetResult();
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
                while (true)
                {
                    try
                    {
                        using (var server = new NamedPipeServerStream("MyImageViewerPipe"))
                        {
                            await server.WaitForConnectionAsync();

                            using (var reader = new StreamReader(server))
                            {
                                var jsonMessage = await reader.ReadToEndAsync();
                                var message = JsonSerializer.Deserialize<PipeMessage>(jsonMessage);

								if (!mainForm.IsDisposed && mainForm.IsHandleCreated)
								{
									// UIスレッドで実行
									mainForm.Invoke(() =>
										{
											switch (message.Command)
											{
												case "open_files":
													mainForm.OpenFiles(message.Files, false);
													break;
												case "clear_counter":
													mainForm.ClearCounter();
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
		/// <param name="files">画像ファイル群</param>
		private static async Task SendFilesToExistingInstanceAsync(string[] files)
        {
            try
            {
                using (var client = new NamedPipeClientStream(".", "MyImageViewerPipe", PipeDirection.Out))
                {
                    await client.ConnectAsync(5000);

                    var message = (files != null && files.Any()) ? new PipeMessage{ Command = "open_files", Files = files  } : new PipeMessage { Command = "clear_counter" };

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
    }

    // メッセージ用のクラス
    public class PipeMessage
    {
        public string Command { get; set; }
        public string[] Files { get; set; } = Array.Empty<string>();
    }
}
