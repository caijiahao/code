using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ExcelAndDgv
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegistryKey Key = Registry.LocalMachine;
            RegistryKey myreg = Key.OpenSubKey("software\\keydog", true);
            string registerOrder = textBox12.Text + textBox11.Text + textBox10.Text + textBox9.Text + textBox8.Text + textBox7.Text;
            myreg.SetValue("registerOrder", registerOrder);
            myreg.SetValue("registerFlag", "1");
            MessageBox.Show("注册成功！"+registerOrder);
            this.Close();
        }
    }
}
