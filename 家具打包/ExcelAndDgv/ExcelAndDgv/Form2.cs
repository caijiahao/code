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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegistryKey Key = Registry.LocalMachine;
            RegistryKey myreg = Key.OpenSubKey("software\\keydog", true);
            if (radioButton1.Checked==true)
            {

                string registerOrder = textBox1.Text + textBox2.Text + textBox3.Text + textBox4.Text + textBox5.Text + textBox6.Text;
                myreg.SetValue("registerOrder", registerOrder);
                myreg.SetValue("registerFlag", "1");
                MessageBox.Show("注册成功！");
            }
            if (radioButton2.Checked == true)
            {

                myreg.SetValue("registerFlag", "0");
                myreg.SetValue("firstTime", DateTime.Now.ToString("yyyy-MM-dd"));
                string dd = "";
                dd = myreg.GetValue("firstTime").ToString();
                DateTime t1 = Convert.ToDateTime(dd);
                DateTime t2 = DateTime.Now;
                TimeSpan span = t2.Subtract(t1);
                int days = Math.Abs(span.Days);
                if (days <= 30)
                {
                    MessageBox.Show("你的试用时间还剩" + (29-days)+ "天。");
                }
                else
                {
                    MessageBox.Show("你的软件已经过期!");
                }
                

            }
            
            this.Close();
        }
    }
}
