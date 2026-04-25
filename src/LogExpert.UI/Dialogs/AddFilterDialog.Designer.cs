using System;
using System.Windows.Forms;
using System.Windows.Shapes;

using Vanara.PInvoke;

namespace LogExpert.UI.Dialogs;

partial class AddFilterDialog
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose (bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent ()
    {
        btnOk = new Button();
        btnCancel = new Button();
        label1 = new Label();
        label2 = new Label();
        txtText = new TextBox();
        txtDesc = new TextBox();
        chkExclude = new CheckBox();
        chkCase = new CheckBox();
        chkRegex = new CheckBox();
        cboTextColor = new ComboBox();
        label3 = new Label();
        label4 = new Label();
        cboBackground = new ComboBox();
        SuspendLayout();
        // 
        // btnOk
        // 
        btnOk.Location = new Point(178, 153);
        btnOk.Name = "btnOk";
        btnOk.Size = new Size(94, 27);
        btnOk.TabIndex = 0;
        btnOk.Text = "Ok";
        btnOk.UseVisualStyleBackColor = true;
        btnOk.Click += btnOk_Click;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(329, 153);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(94, 27);
        btnCancel.TabIndex = 0;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(36, 47);
        label1.Name = "label1";
        label1.Size = new Size(35, 17);
        label1.TabIndex = 1;
        label1.Text = "Text:";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(36, 79);
        label2.Name = "label2";
        label2.Size = new Size(76, 17);
        label2.TabIndex = 1;
        label2.Text = "description:";
        // 
        // txtText
        // 
        txtText.Location = new Point(113, 44);
        txtText.Name = "txtText";
        txtText.Size = new Size(514, 23);
        txtText.TabIndex = 2;
        // 
        // txtDesc
        // 
        txtDesc.Location = new Point(113, 73);
        txtDesc.Name = "txtDesc";
        txtDesc.Size = new Size(514, 23);
        txtDesc.TabIndex = 2;
        // 
        // chkExclude
        // 
        chkExclude.AutoSize = true;
        chkExclude.Location = new Point(88, 110);
        chkExclude.Name = "chkExclude";
        chkExclude.Size = new Size(98, 21);
        chkExclude.TabIndex = 3;
        chkExclude.Text = "Excluding [!]";
        chkExclude.UseVisualStyleBackColor = true;
        // 
        // chkCase
        // 
        chkCase.AutoSize = true;
        chkCase.Location = new Point(223, 110);
        chkCase.Name = "chkCase";
        chkCase.Size = new Size(136, 21);
        chkCase.TabIndex = 3;
        chkCase.Text = "Case-sensitive [Aa]";
        chkCase.UseVisualStyleBackColor = true;
        // 
        // chkRegex
        // 
        chkRegex.AutoSize = true;
        chkRegex.Location = new Point(395, 110);
        chkRegex.Name = "chkRegex";
        chkRegex.Size = new Size(159, 21);
        chkRegex.TabIndex = 3;
        chkRegex.Text = "Regular expression [R]";
        chkRegex.UseVisualStyleBackColor = true;
        // 
        // cboTextColor
        // 
        cboTextColor.DrawMode = DrawMode.OwnerDrawFixed;
        cboTextColor.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTextColor.FormattingEnabled = true;
        cboTextColor.Items.AddRange(new object[] { Color.Black, Color.Maroon, Color.DarkGreen, Color.Olive, Color.DarkBlue, Color.Purple, Color.Aquamarine, Color.Red, Color.Green, Color.Yellow, Color.Blue, Color.Magenta, Color.Cyan, Color.CadetBlue, Color.Chartreuse, Color.Chocolate, Color.CornflowerBlue, Color.Crimson, Color.DarkCyan, Color.DarkMagenta, Color.DeepPink, Color.Firebrick, Color.Goldenrod, Color.HotPink, Color.Indigo, Color.Lime, Color.Orange, Color.Salmon, Color.SeaGreen, Color.SlateBlue, Color.Teal, Color.Violet, Color.Black, Color.White });
        cboTextColor.Location = new Point(341, 14);
        cboTextColor.Name = "cboTextColor";
        cboTextColor.Size = new Size(91, 24);
        cboTextColor.TabIndex = 4;
        cboTextColor.DrawItem += cboTextColor_DrawItem;
        cboTextColor.SelectedIndexChanged += cboTextColor_SelectedIndexChanged;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new Point(264, 17);
        label3.Name = "label3";
        label3.Size = new Size(71, 17);
        label3.TabIndex = 1;
        label3.Text = "Text Color:";
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Location = new Point(452, 17);
        label4.Name = "label4";
        label4.Size = new Size(82, 17);
        label4.TabIndex = 1;
        label4.Text = "Background:";
        // 
        // cboBackground
        // 
        cboBackground.DrawMode = DrawMode.OwnerDrawFixed;
        cboBackground.DropDownStyle = ComboBoxStyle.DropDownList;
        cboBackground.FormattingEnabled = true;
        cboBackground.Items.AddRange(new object[] { Color.Transparent, Color.LightPink, Color.LightSalmon, Color.LightCoral, Color.Plum, Color.Gainsboro, Color.LightGray, Color.LightSlateGray, Color.PaleTurquoise, Color.LightBlue, Color.LightSkyBlue, Color.RoyalBlue, Color.Aquamarine, Color.LightGreen, Color.Green, Color.Khaki, Color.Yellow, Color.Orange, Color.Red, Color.DarkViolet, Color.Black, Color.White });
        cboBackground.Location = new Point(536, 14);
        cboBackground.Name = "cboBackground";
        cboBackground.Size = new Size(91, 24);
        cboBackground.TabIndex = 4;
        cboBackground.DrawItem += cboBackground_DrawItem;
        cboBackground.SelectedIndexChanged += cboBackground_SelectedIndexChanged;
        // 
        // AddFilterDialog
        // 
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(670, 209);
        Controls.Add(cboBackground);
        Controls.Add(cboTextColor);
        Controls.Add(chkRegex);
        Controls.Add(chkCase);
        Controls.Add(chkExclude);
        Controls.Add(txtDesc);
        Controls.Add(txtText);
        Controls.Add(label2);
        Controls.Add(label4);
        Controls.Add(label3);
        Controls.Add(label1);
        Controls.Add(btnCancel);
        Controls.Add(btnOk);
        Name = "AddFilterDialog";
        Text = "AddFilterDialog";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button btnOk;
    private Button btnCancel;
    private Label label1;
    private Label label2;
    private TextBox txtText;
    private TextBox txtDesc;
    private CheckBox chkExclude;
    private CheckBox chkCase;
    private CheckBox chkRegex;
    private ComboBox cboTextColor;
    private Label label3;
    private Label label4;
    private ComboBox cboBackground;
}