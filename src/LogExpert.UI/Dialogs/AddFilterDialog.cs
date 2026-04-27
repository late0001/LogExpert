using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LogExpert.Core.Classes.Filter;

namespace LogExpert.UI.Dialogs;
public partial class AddFilterDialog : Form
{
    public FilterRule Rule { get; private set; }
    public AddFilterDialog ()
    {
        InitializeComponent();
        Rule = new FilterRule();
        cboTextColor.Items.Add("[Custom]");
        cboBackground.Items.Add("[Custom]");
        cboTextColor.SelectedIndex = 0;
        cboBackground.SelectedIndex = 0;
    }

    public AddFilterDialog (string line) : this()
    {
        txtText.Text = line;
    }

    public AddFilterDialog (FilterRule rule) : this()
    {
        Rule = rule;
        txtText.Text = rule.Text;
        txtDesc.Text = rule.Description;
        chkExclude.Checked = rule.IsExclude;
        chkCase.Checked = rule.MatchCase;
        chkRegex.Checked = rule.IsRegex;
        int iFindIndex = -1;
        if (rule.TextColor == Color.Black)
        {
            iFindIndex = 0;
        }
        else
        {
            for (int fgIndex = 1; fgIndex < cboTextColor.Items.Count - 1; fgIndex++)
            {

                if (rule.TextColor == ((Color)cboTextColor.Items[fgIndex]))
                {
                    iFindIndex = fgIndex;
                    break;
                }
            }
        }
           
        if (iFindIndex > -1)
            cboTextColor.SelectedIndex = iFindIndex;
        else
            cboTextColor.SelectedIndex = cboTextColor.Items.Count - 1;

        iFindIndex = -1;
        if (rule.Background == Color.White)
        {
            iFindIndex = 0;
        }
        else
        {
            for (int bgIndex = 1; bgIndex < cboBackground.Items.Count - 1; bgIndex++)
            {
                if (rule.Background == ((Color)cboBackground.Items[bgIndex]))
                {
                    iFindIndex = bgIndex;
                    break;
                }
            }
        }
            
        if(iFindIndex > -1) 
            cboBackground.SelectedIndex = iFindIndex;
        else 
            cboBackground.SelectedIndex = cboBackground.Items.Count-1;

        txtText.ForeColor = rule.TextColor;
        txtText.BackColor = rule.Background;
    }

    private void btnOk_Click (object sender, EventArgs e)
    {
        Rule.Text = txtText.Text;
        Rule.Description = txtDesc.Text;
        Rule.IsExclude = chkExclude.Checked;
        Rule.MatchCase = chkCase.Checked;
        Rule.IsRegex = chkRegex.Checked;
        DialogResult = DialogResult.OK;
    }

    // 自动选黑色/白色前景（保证文字看得清）
    private Color GetContrastColor (Color bg)
    {
        int brightness = (bg.R * 299 + bg.G * 587 + bg.B * 114) / 1000;
        return brightness > 125 ? Color.Black : Color.White;
    }

    private void cboTextColor_DrawItem (object sender, DrawItemEventArgs e)
    {
        e.DrawBackground();
        if (e.Index == -1) return;
        if (e.Index == 0)
        {
            e.Graphics.DrawString("[Default]", e.Font, Brushes.Black, new PointF(4, e.Bounds.Top + 2));
        }
        else if (e.Index < cboTextColor.Items.Count - 1)
        {
            Rectangle rectangle = new(4, e.Bounds.Top + 2, 30, e.Bounds.Height - 4);
            var rectColor = (Color)cboTextColor.Items[e.Index];


            using (Brush br = new SolidBrush(rectColor))
            {
                if (rectColor == Color.White)
                {
                    e.Graphics.FillRectangle(Brushes.Black, 0, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height);
                }
                e.Graphics.FillRectangle(br, rectangle);
                e.Graphics.DrawRectangle(Pens.Black, rectangle);

                e.Graphics.DrawString(((Color)cboTextColor.Items[e.Index]).Name, e.Font, br, new PointF(42, e.Bounds.Top + 2));
            } 

            if (!Enabled)
            {
                HatchBrush brush = new(HatchStyle.Percent50, Color.LightGray, Color.FromArgb(10, Color.LightGray));
                rectangle.Inflate(1, 1);
                e.Graphics.FillRectangle(brush, rectangle);
                brush.Dispose();
            }


        }
        else if (e.Index == cboTextColor.Items.Count - 1)
        {
            e.Graphics.DrawString("[Custom]", e.Font, Brushes.Black, new PointF(4, e.Bounds.Top + 2));
        }

        e.DrawFocusRectangle();
    }

    private void cboTextColor_SelectedIndexChanged (object sender, EventArgs e)
    {
        int index = cboTextColor.SelectedIndex;
        if (index == -1) return;
        if (index == 0)
        {
            Rule.TextColor = Color.Black;
            txtText.ForeColor = Color.Black;
        }
        else if (index < cboTextColor.Items.Count - 1)
        {
            var tColor = (Color)cboTextColor.Items[index];
            Rule.TextColor = tColor;
            txtText.ForeColor = tColor;
        }
        else
        {
            using var cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                Rule.TextColor = cd.Color;
                txtText.ForeColor = cd.Color;
            }
        }
    }
    private void cboBackground_DrawItem (object sender, DrawItemEventArgs e)
    {
        e.DrawBackground();
        if (e.Index == -1) return;
        if (e.Index == 0)
        {
            e.Graphics.DrawString("[Default]", e.Font, Brushes.Black, new PointF(4, e.Bounds.Top + 2));
        }
        else if (e.Index < cboBackground.Items.Count - 1)
        {
            Rectangle rectangle = new(0, e.Bounds.Top , e.Bounds.Width, e.Bounds.Height);
            var rectColor = (Color)cboBackground.Items[e.Index];
            e.Graphics.FillRectangle(new SolidBrush(rectColor), rectangle);
            //e.Graphics.DrawRectangle(Pens.Black, rectangle);
            var fgColor = GetContrastColor(rectColor);
            var fBr = fgColor == Color.Black ? Brushes.Black : Brushes.White;
            e.Graphics.DrawString(((Color)cboBackground.Items[e.Index]).Name, e.Font, fBr, new PointF(4, e.Bounds.Top + 2));
            

            if (!Enabled)
            {
                HatchBrush brush = new(HatchStyle.Percent50, Color.LightGray, Color.FromArgb(10, Color.LightGray));
                rectangle.Inflate(1, 1);
                e.Graphics.FillRectangle(brush, rectangle);
                brush.Dispose();
            }


        }
        else if (e.Index == cboBackground.Items.Count - 1)
        {
            e.Graphics.DrawString("[Custom]", e.Font, Brushes.Black, new PointF(4, e.Bounds.Top + 2));
        }

        e.DrawFocusRectangle();
    }

    private void cboBackground_SelectedIndexChanged (object sender, EventArgs e)
    {
        int index = cboBackground.SelectedIndex;
        if (index == -1) return;
        if (index == 0)
        {
            Rule.Background = Color.White;
            txtText.BackColor = Color.White;
        }
        else if (index < cboBackground.Items.Count - 1)
        {
            var tColor = (Color)cboBackground.Items[index];
            Rule.Background = tColor;
            txtText.BackColor = tColor;
        }
        else
        {
            using var cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                Rule.Background = cd.Color;
                txtText.BackColor = cd.Color;
            }
        }
    }


}
