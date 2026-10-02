using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace ColorImageViewProject
{
    public partial class FormColorImageS
    {
        private const string ViewSourceClipboardFormat = "ColorImageViewer.ViewSource.v1";
        private const string SyncTitleSuffix = " [同期中]";
        private readonly string _viewSourceId = Guid.NewGuid().ToString();
        private ViewSyncGroup _viewSyncGroup;
        private ToolStripMenuItem _leaveViewSyncMenu;

        private sealed class ViewSyncGroup
        {
            public readonly HashSet<FormColorImageS> Members = new HashSet<FormColorImageS>();
            public bool Updating;
        }

        private void InitializeViewSynchronization()
        {
            var join = new ToolStripMenuItem("コピー元の画像と表示を同期");
            join.Click += (_, _) => StartViewSynchronization();
            _leaveViewSyncMenu = new ToolStripMenuItem("表示の同期を解除") { Enabled = false };
            _leaveViewSyncMenu.Click += (_, _) => LeaveViewSynchronization();
            contextMenuStripL.Items.Add(new ToolStripSeparator());
            contextMenuStripL.Items.Add(join);
            contextMenuStripL.Items.Add(_leaveViewSyncMenu);
            // Disposeで直接閉じられた場合にもグループ参照を解放する。
            Disposed += (_, _) => LeaveViewSynchronization();
        }

        private void StartViewSynchronization()
        {
            string sourceId;
            try { sourceId = Clipboard.GetData(ViewSourceClipboardFormat) as string; }
            catch (System.Runtime.InteropServices.ExternalException)
            {
                ShowViewSyncMessage("クリップボードを取得できません。もう一度お試しください。");
                return;
            }
            var source = Application.OpenForms.OfType<FormColorImageS>()
                .FirstOrDefault(form => form._viewSourceId == sourceId && !form.IsDisposed);
            if (source == null || source._scalePbox == null)
            {
                ShowViewSyncMessage("コピー元の画像が見つかりません。開いている画像で Ctrl+Shift+C を実行してください。");
                return;
            }
            if (source == this)
            {
                ShowViewSyncMessage("コピー元とは別の画像で同期を開始してください。");
                return;
            }
            if (_sourceImage == null || source._sourceImage == null || _sourceImage.Size != source._sourceImage.Size)
            {
                ShowViewSyncMessage("同期できるのは、幅と高さが同じ画像だけです。");
                return;
            }
            if (_viewSyncGroup != source._viewSyncGroup || _viewSyncGroup == null)
            {
                LeaveViewSynchronization();
                var group = source._viewSyncGroup ?? new ViewSyncGroup();
                source._viewSyncGroup = group;
                group.Members.Add(source);
                _viewSyncGroup = group;
                group.Members.Add(this);
                foreach (var member in group.Members) member.UpdateViewSyncIndicator();
            }
            source.SynchronizeViews();
        }

        private void ShowViewSyncMessage(string message) =>
            MessageBox.Show(this, message, "表示の同期", MessageBoxButtons.OK, MessageBoxIcon.Information);

        private void SynchronizeViews()
        {
            var group = _viewSyncGroup;
            if (group == null || group.Updating || _scalePbox == null) return;
            group.Updating = true;
            try
            {
                using var timing = ProcessingTimings.Measure("表示同期（同期先の描画を含む）");
                foreach (var target in group.Members.ToArray())
                {
                    if (target == this || target.IsDisposed || target._scalePbox == null) continue;
                    // 同じ画像座標を表示枠の同じ位置に配置。ウィンドウ・測定範囲は変更しない。
                    target._scalePbox.SetMat((Matrix)_scalePbox.GetMat().Clone());
                    target.labelDisplayRate.Text = String.Format("{0:###0.0 %}", target._scalePbox.DispRate);
                    target._scalePbox.DrawMainImage();
                }
            }
            finally { group.Updating = false; }
        }

        private void LeaveViewSynchronization()
        {
            var group = _viewSyncGroup;
            if (group == null) return;
            _viewSyncGroup = null;
            group.Members.Remove(this);
            if (!IsDisposed) UpdateViewSyncIndicator();
            if (group.Members.Count == 1)
            {
                var remaining = group.Members.First();
                remaining._viewSyncGroup = null;
                group.Members.Clear();
                if (!remaining.IsDisposed) remaining.UpdateViewSyncIndicator();
            }
        }

        private void UpdateViewSyncIndicator()
        {
            if (_leaveViewSyncMenu != null) _leaveViewSyncMenu.Enabled = _viewSyncGroup != null;
            var title = Text.EndsWith(SyncTitleSuffix, StringComparison.Ordinal)
                ? Text.Substring(0, Text.Length - SyncTitleSuffix.Length) : Text;
            Text = title + (_viewSyncGroup != null ? SyncTitleSuffix : "");
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (_viewSyncGroup != null && !Text.EndsWith(SyncTitleSuffix, StringComparison.Ordinal))
            {
                Text += SyncTitleSuffix;
                return;
            }
            base.OnTextChanged(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            LeaveViewSynchronization();
            base.OnFormClosed(e);
        }
    }
}
