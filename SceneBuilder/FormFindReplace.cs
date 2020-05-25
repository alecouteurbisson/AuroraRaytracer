using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScintillaNet;

namespace SceneBuilder
{
  public partial class formFindReplace : Form
  {
    Scintilla scintilla;

    public formFindReplace(Scintilla scintilla)
    {
      InitializeComponent();
      this.scintilla = scintilla;
      if(scintilla.Selection.Length != 0)
        txtbFind.Text = scintilla.Selection.Text;
    }

    private void Find(object sender, EventArgs e)
    {
      SearchFlags sf = chkbMatchCase.Checked ? SearchFlags.MatchCase : SearchFlags.Empty;
      Range range = scintilla.FindReplace.FindNext(txtbFind.Text, sf);
      if(range != null)
        range.Select();
    }

    private void Replace(object sender, EventArgs e)
    {
      Range range;
      SearchFlags sf = chkbMatchCase.Checked ? SearchFlags.MatchCase : SearchFlags.Empty;
      range = scintilla.FindReplace.ReplaceNext(txtbFind.Text, txtbReplace.Text, sf);

      if(range != null)
        range.Select();
    }

    private void ReplaceAll(object sender, EventArgs e)
    {
      SearchFlags sf = chkbMatchCase.Checked ? SearchFlags.MatchCase : SearchFlags.Empty;
      scintilla.FindReplace.ReplaceAll(txtbFind.Text, txtbReplace.Text, sf);
    }

    private void CloseForm(object sender, EventArgs e)
    {
      Hide();
    }
  }
}
