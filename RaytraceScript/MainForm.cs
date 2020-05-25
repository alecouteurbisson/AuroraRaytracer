using Aurora;
using Microsoft.ClearScript.V8;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaytraceScript
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private Scene CreateScene(string script)
        {
            using (var engine = new V8ScriptEngine())
            {
                
            }
        }
    }
}
