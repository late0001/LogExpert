using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static Vanara.PInvoke.ComCtl32;

namespace LogExpert.UI.Controls.LogWindow;
public class UDListView : ListView
{
    private const int WM_LBUTTONDBLCLK = 0x0203;
    private const int LVHT_ONITEMSTATEICON = 0x08; // 官方：点在复选框/状态图标上
    [StructLayout(LayoutKind.Sequential)]
    private struct LVHITTESTINFO
    {
        public int pt_x;
        public int pt_y;
        public uint flags;
        public IntPtr lParam;
    }

    [DllImport("user32.dll")]
    private static extern int SendMessage (IntPtr hWnd, int msg, int wParam, ref LVHITTESTINFO info);

    protected override void WndProc (ref Message m)
    {
        if (m.Msg == WM_LBUTTONDBLCLK && CheckBoxes)
        {
            Point pt = PointToClient(Cursor.Position);

            LVHITTESTINFO info = new LVHITTESTINFO
            {
                pt_x = pt.X,
                pt_y = pt.Y
            };

            // 👇 【官方API】精准判断是否点击在复选框上
            SendMessage(Handle, 0x1000 + 42, 0, ref info); // LVM_HITTEST

            // 只有真正点在 CheckBox 上，才允许触发勾选
            bool isClickOnCheckBox = (info.flags & LVHT_ONITEMSTATEICON) != 0;

            if (!isClickOnCheckBox)
            {
                // 不是点复选框 → 只触发双击，不触发勾选
                OnMouseDoubleClick(new MouseEventArgs(MouseButtons.Left, 2, pt.X, pt.Y, 0));
                return;
            }
        }

        base.WndProc(ref m);
    }
}
