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
    // 双击消息
    private const int WM_LBUTTONDBLCLK = 0x0203;

    protected override void WndProc (ref Message m)
    {
        if (m.Msg == WM_LBUTTONDBLCLK && CheckBoxes)
        {
            // 获取鼠标客户端坐标
            Point mousePt = PointToClient(Cursor.Position);
            ListViewHitTestInfo ht = HitTest(mousePt);

            // 关键点：StateImage = 复选框区域；Label/SubItem=文本区域
            // 1. 双击在复选框 → 走原生逻辑（正常切换勾选）
            if (ht.Location == ListViewHitTestLocations.StateImage)
            {
                base.WndProc(ref m);
                return;
            }
            // 2. 双击在文本/行区域 → 吞掉原生双击消息，不再往下传（原生勾选逻辑直接废掉），手动抛双击事件
            if (ht.Item != null)
            {
                OnMouseDoubleClick(new MouseEventArgs(MouseButtons.Left, 2, mousePt.X, mousePt.Y, 0));
                // 直接return，不再调用base.WndProc，原生WM_LBUTTONDBLCLK被拦截，不会触发ItemCheck改勾选
                return;
            }
        }
        // 非双击消息正常走原生
        base.WndProc(ref m);
    }
}
