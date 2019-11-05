using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 응소실_HW3_Client
{
    public partial class ClientForm1 : Form
    {
        public ClientForm1()
        {
            InitializeComponent();
        }

        private void 접속_Click(object sender, EventArgs e)
        {
            ClientForm2 client2 = new ClientForm2(this, this.textBox1.Text, Convert.ToInt32(this.textBox2.Text), this.textBox3.Text);
            DialogResult dResult = client2.ShowDialog();
        }
    }
}
