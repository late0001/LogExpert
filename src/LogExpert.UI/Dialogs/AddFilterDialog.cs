using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
    }

    public AddFilterDialog (FilterRule rule) : this()
    {
        Rule = rule;
        txtText.Text = rule.Text;
        txtDesc.Text = rule.Description;
        chkExclude.Checked = rule.IsExclude;
        chkCase.Checked = rule.MatchCase;
        chkRegex.Checked = rule.IsRegex;
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

    private void btnColor_Click (object sender, EventArgs e)
    {
        using var cd = new ColorDialog();
        if (cd.ShowDialog() == DialogResult.OK)
        {
            Rule.HighlightColor = cd.Color;
            btnColor.BackColor = cd.Color;
        }
            
    }
}
