using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 응소실_HW3
{
    public partial class ServerForm1 : Form
    {
        public ServerForm1()
        {
            InitializeComponent();
        }

        private void 서버열기_Click(object sender, EventArgs e)
        {
            ServerForm2 server2 = new ServerForm2(this,this.textBox1.Text, Convert.ToInt32(this.textBox2.Text));
            DialogResult dResult = server2.ShowDialog();
            
        }
    }
}
