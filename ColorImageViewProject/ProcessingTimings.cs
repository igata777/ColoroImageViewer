using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ColorImageViewProject
{
    /// <summary>画像を保持せず、処理時間の集計だけをメモリ上に保存します。</summary>
    internal static class ProcessingTimings
    {
        private static readonly object Gate = new object();
        private static readonly Dictionary<string, Statistics> Entries = new Dictionary<string, Statistics>();
        private static int _generation;

        private sealed class Statistics
        {
            public long Count;
            public double Total, Last, Maximum;
        }

        public static Measurement Measure(string operation)
        {
            lock (Gate) return new Measurement(operation, _generation);
        }

        internal readonly struct Measurement : IDisposable
        {
            private readonly string _operation;
            private readonly int _generation;
            private readonly long _start;

            public Measurement(string operation, int generation)
            {
                _operation = operation;
                _generation = generation;
                _start = Stopwatch.GetTimestamp();
            }

            public void Dispose()
            {
                var milliseconds = Stopwatch.GetElapsedTime(_start).TotalMilliseconds;
                lock (Gate)
                {
                    // リセット前に始まった処理を新しい集計に混ぜない。
                    if (_generation != ProcessingTimings._generation) return;
                    if (!Entries.TryGetValue(_operation, out var value))
                        Entries.Add(_operation, value = new Statistics());
                    value.Count++;
                    value.Total += milliseconds;
                    value.Last = milliseconds;
                    value.Maximum = Math.Max(value.Maximum, milliseconds);
                }
            }
        }

        internal static void Reset()
        {
            lock (Gate)
            {
                Entries.Clear();
                _generation++;
            }
        }

        internal static string CreateReport()
        {
            var report = new StringBuilder();
            report.AppendLine("全画像ウィンドウ共通・今回の起動中の集計（単位: ms）");
            report.AppendLine("各項目は重複するため、合計同士を足しても全体時間にはなりません。");
            report.AppendLine("完了した計測区間を集計します。失敗・キャンセルした処理も含みます。");
            report.AppendLine("描画はCPU側の処理時間で、画面への反映時間やFPSではありません。");
            report.AppendLine();
            lock (Gate)
            {
                if (Entries.Count == 0) report.AppendLine("計測データはありません。画像を開く、拡大・移動するなどの操作をしてください。");
                foreach (var entry in Entries)
                {
                    var value = entry.Value;
                    report.AppendLine(entry.Key);
                    report.AppendLine($"  回数: {value.Count:N0}  最新: {value.Last:F2}  平均: {value.Total / value.Count:F2}  最大: {value.Maximum:F2}  合計: {value.Total:F2}");
                    report.AppendLine();
                }
            }
            return report.ToString();
        }

        public static void ShowReport(Form owner)
        {
            using var dialog = new Form
            {
                Text = "処理時間の計測", Size = new Size(800, 540),
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false, MaximizeBox = false
            };
            var report = new TextBox
            {
                Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Both,
                WordWrap = false, Dock = DockStyle.Fill, Text = CreateReport()
            };
            var buttons = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 42 };
            var refresh = new Button { Text = "更新", AutoSize = true };
            var reset = new Button { Text = "計測をリセット", AutoSize = true };
            var close = new Button { Text = "閉じる", AutoSize = true, DialogResult = DialogResult.Cancel };
            refresh.Click += (_, _) => report.Text = CreateReport();
            reset.Click += (_, _) => { Reset(); report.Text = CreateReport(); };
            buttons.Controls.AddRange(new Control[] { refresh, reset, close });
            dialog.Controls.Add(report);
            dialog.Controls.Add(buttons);
            dialog.CancelButton = close;
            dialog.ShowDialog(owner);
        }
    }
}
