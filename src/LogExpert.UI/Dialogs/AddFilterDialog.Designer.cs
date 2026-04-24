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
        btnBackground = new Button();
        btnTextColor = new Button();
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
        // btnBackground
        // 
        btnBackground.Location = new Point(533, 11);
        btnBackground.Name = "btnBackground";
        btnBackground.Size = new Size(94, 27);
        btnBackground.TabIndex = 0;
        btnBackground.Text = "Color";
        btnBackground.UseVisualStyleBackColor = true;
        btnBackground.Click += btnBackground_Click;
        // 
        // btnTextColor
        // 
        btnTextColor.Location = new Point(424, 12);
        btnTextColor.Name = "btnTextColor";
        btnTextColor.Size = new Size(94, 27);
        btnTextColor.TabIndex = 0;
        btnTextColor.Text = "TextColor";
        btnTextColor.UseVisualStyleBackColor = true;
        btnTextColor.Click += btnTextColor_Click;
        // 
        // AddFilterDialog
        // 
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(670, 209);
        Controls.Add(chkRegex);
        Controls.Add(chkCase);
        Controls.Add(chkExclude);
        Controls.Add(txtDesc);
        Controls.Add(txtText);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(btnCancel);
        Controls.Add(btnTextColor);
        Controls.Add(btnBackground);
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
    private Button btnBackground;
    private Button btnTextColor;
}