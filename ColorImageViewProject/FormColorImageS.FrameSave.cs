using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ColorImageViewProject
{
    public partial class FormColorImageS
    {
        private void InitializeFrameSaveMenu()
        {
            表示画像を保存ToolStripMenuItem.Text = "表示範囲を保存（枠線あり）";
            元画像をファイルに保存ToolStripMenuItem.Text = "元画像を保存（枠線なし）";
            var fullImage = new ToolStripMenuItem("画像全体を保存（枠線あり）");
            fullImage.Click += (_, _) => SaveFullImageWithFrame();
            contextMenuStripL.Items.Insert(contextMenuStripL.Items.IndexOf(表示画像を保存ToolStripMenuItem) + 1, fullImage);
        }

        /// <summary>元画像を変更せず、現在の表示倍率に合わせた線幅で保存用画像を作成します。</summary>
        private Bitmap CreateFullImageWithFrame()
        {
            var bitmap = _sourceImage.Clone(new Rectangle(Point.Empty, _sourceImage.Size), PixelFormat.Format24bppRgb);
            try
            {
                using var graphics = Graphics.FromImage(bitmap);
                using var pen = new Pen(Color.Red, 3.0f / _scalePbox.DispRate);
                var rect = _displayFrameOverlay.Value;
                graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                return bitmap;
            }
            catch
            {
                bitmap.Dispose();
                throw;
            }
        }

        private void SaveFullImageWithFrame()
        {
            if (_sourceImage == null || !_displayFrameOverlay.HasValue || _scalePbox == null) return;
            try
            {
                using var bitmap = CreateFullImageWithFrame();
                SaveImageFile(bitmap, Text + "_枠線あり");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "枠線付き画像を保存できませんでした。\n" + ex.Message,
                    "画像の保存", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowFrameSaveOptions()
        {
            using var dialog = new Form
            {
                Text = "保存方法を選択", ClientSize = new Size(520, 185),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent, MinimizeBox = false, MaximizeBox = false
            };
            var full = new RadioButton
            {
                Text = "画像全体を保存（枠線あり・元の解像度）", Checked = true,
                AutoSize = true, Location = new Point(20, 20)
            };
            var visible = new RadioButton
            {
                Text = "表示範囲を保存（枠線あり・現在の表示サイズ）",
                AutoSize = true, Location = new Point(20, 55)
            };
            var original = new RadioButton
            {
                Text = "元画像を保存（枠線なし・元の解像度）",
                AutoSize = true, Location = new Point(20, 90)
            };
            var save = new Button { Text = "次へ", Location = new Point(300, 140), DialogResult = DialogResult.OK };
            var cancel = new Button { Text = "キャンセル", Location = new Point(395, 140), Width = 100, DialogResult = DialogResult.Cancel };
            dialog.Controls.AddRange(new Control[] { full, visible, original, save, cancel });
            dialog.AcceptButton = save;
            dialog.CancelButton = cancel;
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            if (full.Checked) SaveFullImageWithFrame();
            else if (visible.Checked) 表示画像をファイルに保存ToolStripMenuItem_Click(this, EventArgs.Empty);
            else 元画像をファイルに保存ToolStripMenuItem_Click(this, EventArgs.Empty);
        }
    }
}
