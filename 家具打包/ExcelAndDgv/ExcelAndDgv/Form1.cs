using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using Microsoft.Win32;
using System.Management;
using System.IO;
using System.Security;
using System.Security.Cryptography;

namespace ExcelAndDgv
{
    public partial class Form1 : Form
    {
        private string fileNameType;
        private string path;
        private string conString;
        private string fileName;
        private ArrayList dataList = new ArrayList(); //存储提取数据的对象
        private List<Wood> sourceList = new List<Wood>();//分类后的数据对象
        private List<Wood> dealList = new List<Wood>();//中间处理数组
        private List<Wood> result = new List<Wood>();//找到相同的板
        List<Wood> one = new List<Wood>();
        List<Wood> two = new List<Wood>();
        List<Wood> three = new List<Wood>();
        List<Wood> four = new List<Wood>();
        List<Wood> five = new List<Wood>();
        List<Wood> dist = new List<Wood>();
        List<Wood> wrongData = new List<Wood>();
        
        public Form1()
        {
           // MessageBox.Show("注册表很头疼");
           // deleteRegister();
            //createRegister();
           /*RegistryKey Key = Registry.LocalMachine;
            RegistryKey myreg = Key.OpenSubKey("software\\keydog", true);
            myreg = Key.OpenSubKey("software\\keydog", true); //该项必须已存在
            myreg.SetValue("registerFlag", "0");
            myreg.SetValue("registerOrder", "8ZD35XeLvPfABhPFFOY8yULc");
            MessageBox.Show(myreg.GetValue("registerFlag").ToString());*/
           // if (isRegister())
           // {
                InitializeComponent();
            //}
           // else
            //{
               // System.Environment.Exit(0); 
           // }
        }

        //判断注册表项是否存在
        private bool IsRegeditItemExist()
        {
            string[] subkeyNames;
            RegistryKey hkml = Registry.LocalMachine;
            RegistryKey software = hkml.OpenSubKey("SOFTWARE");
            //RegistryKey software = hkml.OpenSubKey("SOFTWARE", true); 
            subkeyNames = software.GetSubKeyNames();
            //取得该项下所有子项的名称的序列，并传递给预定的数组中 
            foreach (string keyName in subkeyNames)
            //遍历整个数组 
            {
                if (keyName == "keydog")
                //判断子项的名称 
                {
                    hkml.Close();
                    return true;
                }
            }
            hkml.Close();
            return false;
        }

        private void createRegister()
        {
            //创建注册表项
            if (!IsRegeditItemExist())
            {
                RegistryKey key = Registry.LocalMachine;
                RegistryKey software = key.CreateSubKey("software\\keydog");
                //在HKEY_LOCAL_MACHINE\SOFTWARE下新建名为keydog的注册表项。如果已经存在则不影响！
                //打开注册表
                software = key.OpenSubKey("software\\keydog", true); //该项必须已存在
               // software.SetValue("keydog", PNP.Text);
                Form2 f2 = new Form2();
                f2.ShowDialog();
               // MessageBox.Show(software.GetValue("registerOrder").ToString());
               // software.SetValue("firstTime", DateTime.Now.ToString("yyyy-MM-dd"));
               // software.SetValue("registerFlag", "0");
                //在HKEY_LOCAL_MACHINE\SOFTWARE\keydog下创建一个名为“PNP”，值为“PNP.Text”的键值。如果该键值原本已经存在，则会修改替换原来的键值，如果不存在则是创建该键值。
            }
           
        }

        //private void deleteRegister()
        //{
           // RegistryKey key = Registry.LocalMachine;
           // key.DeleteSubKey("software\\keydog", true); //该方法无返回值，直接调用即可
           // key.Close();
       // }

        //判断是否已注册
        private Boolean isRegister()
        {
            RegistryKey Key = Registry.LocalMachine;
            RegistryKey myreg = Key.OpenSubKey("software\\keydog", true);
            if (myreg.GetValue("registerFlag").ToString().Equals("0") && isSoftTime())
            {
                return true;
            }
            else if (myreg.GetValue("registerFlag").ToString().Equals("1") && jodgeUsbInfo())
            {
                return true;
            }
            else
            {
                return false;

            }
 
        }

        //软件试用
        private Boolean isSoftTime()
        {
            string dd = "";
            RegistryKey Key = Registry.LocalMachine;
            RegistryKey time = Key.OpenSubKey("software\\keydog", true);
            dd = time.GetValue("firstTime").ToString();
            DateTime t1 = Convert.ToDateTime(dd);
            DateTime t2 = DateTime.Now;
            TimeSpan span = t2.Subtract(t1);
            int days = Math.Abs(span.Days);
            if (days <= 30)
            {
                return true;
            }
               
            else
            {
                return false;
            }
        }

        //判断U盘信息
        private Boolean jodgeUsbInfo()
        {
            string driveName = "";
            string caption = "";
            string size = "";
            string pnpDeviceId = "";
            string rev = "";
            string vid = "";
            RegistryKey Key = Registry.LocalMachine;
            RegistryKey myreg = Key.OpenSubKey("software\\keydog", true);
            DriveInfo[] s = DriveInfo.GetDrives();//提供驱动器信息
            foreach (DriveInfo drive in s)//访问驱动器信息
            {
                if (drive.DriveType == DriveType.Removable)//判断驱动器是不是可移动存储器
                {
                  driveName= drive.Name.ToString();//驱动器名称
                    break;
                }
            }
            //获取硬盘ID
            ManagementClass cimobject = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moc = cimobject.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo.Properties["InterfaceType"].Value.ToString() == "USB")
                {
                    try
                    {
                        //产品名称
                      caption = mo.Properties["Caption"].Value.ToString();

                        //总容量
                       size= mo.Properties["Size"].Value.ToString();
                        string[] info = mo.Properties["PNPDeviceID"].Value.ToString().Split('&');
                        string[] xx = info[3].Split('\\');

                        //序列号
                       pnpDeviceId = xx[1];
                       // MessageBox.Show("U盘序列号:" + xx[1]);
                       // PNPDeviceID.Text = xx[1];
                        xx = xx[0].Split('_');

                        //版本号
                       rev= xx[1];

                        //制造商ID
                        xx = info[1].Split('_');
                        vid = xx[1];

                    }
                    catch (Exception ex)
                    {
                        System.Environment.Exit(0); 
                       // MessageBox.Show(ex.Message);
                    }
                }
            }
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] palindata1 = Encoding.Default.GetBytes(driveName);//将要加密的字符串转换为字节数组
            byte[] palindata2 = Encoding.Default.GetBytes(caption);
            byte[] palindata3 = Encoding.Default.GetBytes(size);
            byte[] palindata4 = Encoding.Default.GetBytes(pnpDeviceId);
            byte[] palindata5 = Encoding.Default.GetBytes(rev);
            byte[] palindata6 = Encoding.Default.GetBytes(vid);

            byte[] encryptdata1 = md5.ComputeHash(palindata1);//将字符串加密后也转换为字符数组
            byte[] encryptdata2 = md5.ComputeHash(palindata2);
            byte[] encryptdata3 = md5.ComputeHash(palindata3);
            byte[] encryptdata4 = md5.ComputeHash(palindata4);
            byte[] encryptdata5 = md5.ComputeHash(palindata5);
            byte[] encryptdata6 = md5.ComputeHash(palindata6);

            string registerOrder = "";
            registerOrder+= Convert.ToBase64String(encryptdata1).Substring(0,4);//将加密后的字节数组转换为加密字符串
            registerOrder+= Convert.ToBase64String(encryptdata2).Substring(0,4);
            registerOrder+= Convert.ToBase64String(encryptdata3).Substring(0,4);
            registerOrder+= Convert.ToBase64String(encryptdata4).Substring(0,4);
            registerOrder+= Convert.ToBase64String(encryptdata5).Substring(0,4);
            registerOrder+= Convert.ToBase64String(encryptdata6).Substring(0,4);

            if (myreg.GetValue("registerOrder").ToString().Trim().Equals(registerOrder))
            {
                return true;
            }
            else
            {
                return false;
            }
 
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            RegistryKey Key = Registry.LocalMachine;
            RegistryKey myreg = Key.OpenSubKey("software\\keydog", true);
            Form3 f3 = new Form3();
            f3.ShowDialog();
           // MessageBox.Show(myreg.GetValue("registerOrder").ToString());
        }

        private void button1_Click(object sender, EventArgs e) //导入数据表按钮
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                conString = openDlg.FileName;
                int pathIndex=conString.LastIndexOf("\\");
                path = conString.Substring(0, pathIndex);
                fileName = conString.Substring(pathIndex + 1);
                int n = conString.LastIndexOf(".");
                if (n >= 0)
                {
                    fileNameType = conString.Substring(n);
                }
                if (fileNameType.Contains("xlsx")||fileNameType.Contains("xls")) //读取的表是xlsx格式
                {
                    OpenExcel();
                }
                else if(fileNameType.Contains("csv"))//读取的表是csv格式
                {
                      OpenCsv();
                }
            }
        }

        private void OpenExcel() //打开xlsx
        {
            string strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + conString + ";Extended Properties='Excel 12.0;HDR=YES;IMEX=1;'";
            OleDbConnection conn = new OleDbConnection(strCon);
            string sqlstring = @"SELECT * FROM [Sheet1$]";
            //string sqlstring = @"SELECT * FROM [Page1]";
            OleDbDataAdapter adapter = new OleDbDataAdapter(sqlstring, conn);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            this.packageData.DataSource = ds.Tables[0];
        }
       
        private void OpenCsv() //打开csv文件
        {
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+path+";Extended Properties=\"Text;HDR=no;FMT=Delimited\"";   
            OleDbConnection conn = new OleDbConnection(strCon);
            conn.Open();
            string sqlstring = string.Format(@"SELECT * FROM [" + fileName+"]");
            OleDbDataAdapter da = new OleDbDataAdapter(sqlstring, conn);
            DataTable dt = new DataTable();
            da.Fill(dt); 
            conn.Close();
            packageData.DataSource = dt.DefaultView;
        }

        private void package_Click(object sender, EventArgs e) //分类打包
        {
            DataTable dt = (DataTable)this.packageData.DataSource; 
            for (int i = 0; i < this.packageData.RowCount-1; i++)
            {
                string str = dt.Rows[i]["材料"].ToString();
                string  []thickness = str.Split(' ');
               // tickness= System.Text.RegularExpressions.Regex.Replace(tickness, @"[^0-9]+", "");
               
                Wood  data = new Wood(i + 1, 
                                      (double.Parse)(dt.Rows[i]["宽度"].ToString()),
                                      (double.Parse)(dt.Rows[i]["长度"].ToString()),
                                      (double.Parse)(thickness[2]),
                                      (int.Parse)(dt.Rows[i]["数量"].ToString()
                                      ));
                //MessageBox.Show(data.num+"");
                dataList.Add(data);
            }
             deal();
           
              /*dt.Columns.Add("分类打包", typeof(int));
              dt.Columns[packageData.ColumnCount-1].ColumnName = "分类打包";*/
              for (int i = 0; i < this.packageData.RowCount-1; i++)
              {

                  dt.Rows[i][packageData.ColumnCount-2] = dist[i].packageNumber;

              }
            
              sourceList.Clear();
              dist.Clear();
              result.Clear();
              dealList.Clear();
              dataList.Clear();
              wrongData.Clear();
 
          
        }
        #region  //保存文件为xlsx
        private void saveFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //利用流导出Exce
            saveFileDialog.Filter = "Execl files (*.xls)|*.xls";
            saveFileDialog.FileName = "已分类";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.CreatePrompt = true;
            saveFileDialog.Title = "另存信息";
            saveFileDialog.ShowDialog();
            Stream myStream;
            try
            {
                myStream = saveFileDialog.OpenFile();
            }
            catch
            {
                return;
            }
            StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));
            string str = "";
            try
            {
                //写标题
                for (int i = 0; i < this.packageData.ColumnCount; i++)
                {
                    if (i > 0)
                    {
                        str += "\t";
                    }
                    str += this.packageData.Columns[i].HeaderText;
                }
                sw.WriteLine(str);
                //写内容
                for (int j = 0; j < this.packageData.Rows.Count; j++)
                {
                    string tempStr = "";
                    for (int k = 0; k < this.packageData.Columns.Count; k++)
                    {
                        if (k > 0)
                        {
                            tempStr += "\t";
                        }
                        tempStr += this.packageData.Rows[j].Cells[k].Value.ToString();
                    }
                    sw.WriteLine(tempStr);
                }
                sw.Close();
                myStream.Close();
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.ToString());
            }
            finally
            {
                sw.Close();
                myStream.Close();
            }
           
        }
        #endregion

        public void dicernWood(Wood wood)
        {
            if (wood.width >= 800 && wood.length >= 800)
            {
                if (wood.thickness < 25)
                {
                    wood.range = 2;
                    wood.value = 0.50*wood.num;
                    //two.Add(wood);
                }
                else
                {
                    wood.range = 1;
                    wood.value = 1;
                   // one.Add(wood);
                }
            }
            else if (wood.width < 800 && wood.width >= 600 && wood.length < 2440 && wood.length >= 1500)
            {

                wood.range = 2;
                wood.value = 0.50 * wood.num;
                // two.Add(wood);
            }
            else if (wood.width < 600 && wood.width >= 350 && wood.length < 2440 && wood.length >= 1900)
            {

                wood.value = 0.50 * wood.num;
                wood.range = 2;
                // two.Add(wood);
            }
          
            else if (wood.width < 800 && wood.width >= 600 && wood.length < 1500 && wood.length >= 800)
            {
                wood.value = 0.33 * wood.num;
                wood.range = 3;
               // three.Add(wood);
            }
            else if (wood.width < 600 && wood.width >= 350 && wood.length < 1900 && wood.length >= 1500)
            {
                wood.range = 3;
                wood.value = 0.33 * wood.num;
                // three.Add(wood);
            }
           
            else if (wood.width < 350 && wood.width >=200&&wood.length < 2440 &&wood.length>=1990)
            {
                wood.range = 3;
                wood.value = 0.33 * wood.num;
                // three.Add(wood);
            }
            else if (wood.width < 200 && wood.length < 2440)
            {
                wood.range = 30;
                wood.value = 0.033 * wood.num;
            }
            else if (wood.width < 800 && wood.width >= 600 && wood.length < 800)
            {
                if (wood.thickness > 25)
                {
                    wood.range = 3;
                    wood.value = 0.33 * wood.num;
                    //three.Add(wood);
                }
                else
                {
                    wood.range = 4;
                    wood.value = 0.25 * wood.num;
                    //four.Add(wood);
                }
            }
         
            else if (wood.width < 600 &&wood.width>=350&& wood.length >= 800 && wood.length < 1500)
            {
                if (wood.thickness > 25)
                {
                    wood.range = 3;
                    wood.value = 0.33 * wood.num;
                    //three.Add(wood);
                }
                else
                {
                    wood.range = 4;
                    wood.value = 0.25 * wood.num;
                    //four.Add(wood);
                }
            }
            /*else if (wood.width < 450 && wood.width >= 320 && wood.length < 1200)
            {
                if (wood.thickness > 25)
                {
                    three.Add(wood);
                }
                else
                {
                    four.Add(wood);
                }
            }*/
            else if (wood.width < 600 &&wood.width>=200&& wood.length <= 800)
            {
                if (wood.thickness > 25)
                {
                    wood.range = 4;
                    wood.value = 0.25 * wood.num;
                    //four.Add(wood);
                }
                else
                {
                    wood.range = 5;
                    wood.value = 0.20 * wood.num;
                    //five.Add(wood);
                }
            }
            else
            {
                wood.range = 0;
                wood.value = 0 * wood.num;
                //wrongData.Add(wood);
            }

            sourceList.Add(wood);
        }

        public void deal()
        {
            Wood newWood = new Wood(0, 0, 0, 0,0);
            int count = 1;
            foreach (Wood wood in dataList)
            {
                dicernWood(wood);
            }

            for (int i = 0; i < sourceList.Count; i++)
            {
                dealList.Clear();
                if (sourceList[i].range > 0 && sourceList[i].packageNumber == 0)
                {
                    dealList.Add(sourceList[i]);
           
                        for (int j = 0; j < sourceList.Count; j++)
                        {
                            if (dealList.Count == sourceList[i].range)
                            {
                                for (int t = 0; t < dealList.Count; t++)
                                {
                                    int index = sourceList.IndexOf(dealList[t]);
                                    sourceList[index].packageNumber = count;
                                }
                                count++;
                                break;
                            }
                            else
                            {
                                if (sourceList[j].width == sourceList[i].width && sourceList[j].length == sourceList[i].length && sourceList[j].thickness == sourceList[i].thickness && sourceList[j].packageNumber == 0 && sourceList[j].id != sourceList[i].id)
                                {
                                    dealList.Add(sourceList[j]);
                                }
                            }
                        }
                    }

            }
            for (int i = 0; i < sourceList.Count; i++)
            {
                Boolean flag = false;
                if (sourceList[i].range > 0 && sourceList[i].packageNumber == 0)
                {
                    for (int k = sourceList[i].range - 1; k >=1; k--)
                    {
                        double total = sourceList[i].value;
                        dealList.Clear();
                        dealList.Add(sourceList[i]);
                        bool note = true;
                        for (int j = 0; j < sourceList.Count; j++)
                        {
                                if (dealList.Count >= k)
                                {
                                if (sourceList[j].id != sourceList[i].id&&sourceList[j].packageNumber==0)
                                {
                                    /*if (sourceList[i].range == 30)
                                    {
                                        note = false;
                                    }*/
                                    int check = 0;
                                    if (sourceList[i].range != 30)
                                    {
                                        for (int z = 0; z < dealList.Count; z++)
                                        {
                                            if (sourceList[j].length - dealList[z].length > 162 || sourceList[j].length - dealList[z].length < -162)
                                            {
                                                break;
                                            }
                                           /* if (sourceList[j].range != 30 && note == false)
                                            {
                                                break;
                                            }*/
                                            else
                                            {
                                                check++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (int z = 0; z < dealList.Count; z++)
                                        {
                                            if (sourceList[j].length - dealList[z].length > 162 || sourceList[j].length - dealList[z].length < -162||sourceList[j].range!=30)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                check++;
                                            }
                                        }
                                    }
                                        if (check == dealList.Count)
                                        {
                                            total += sourceList[j].value;
                                            if (total <= 1)
                                            {
                                                //if (count == 18)
                                                //{
                                                //    MessageBox.Show(total.ToString() + ":" + sourceList[j].id);
                                                //}
                                                dealList.Add(sourceList[j]);

                                            }
                                            else
                                            {
                                                total -= sourceList[j].value;
                                            }
                                        }
                                }
                            }
                            else
                            {
                                if (sourceList[j].width == sourceList[i].width && sourceList[j].length == sourceList[i].length && sourceList[j].thickness == sourceList[i].thickness && sourceList[j].packageNumber == 0 && sourceList[j].id != sourceList[i].id)
                                {
                                    dealList.Add(sourceList[j]);
                                    total += sourceList[j].value;
                                    
                                }
                            }

                                      
                                
                        }

                        if (dealList.Count >= k)
                        {
                            flag = true;
                        }

                        if (flag == true)
                        {
                            for (int t = 0; t < dealList.Count; t++)
                            {
                                int index = sourceList.IndexOf(dealList[t]);
                                sourceList[index].packageNumber = count;
                                
                            }
                            
                            count++;
                            flag = false;
                            break;
                        }
                        
                    }

                }
            }

            foreach(Wood wood in sourceList)
            {
                if (wood.packageNumber > 0)
                {
                    result.Add(wood);
                }
                else
                {
                    if (wood.range == 2)
                    {
                        two.Add(wood);
                    }else if (wood.range == 3)
                    {
                        three.Add(wood);
                    }else if(wood.range == 4)
                    {
                        four.Add(wood);
                    }else if(wood.range == 5)
                    {
                        five.Add(wood);
                    }else if(wood.range == 0)
                    {
                        wrongData.Add(wood);
                    }
                }
            }

                while (one.Count > 0)
                {
                    newWood = one[0];
                    one.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    count++;
                }

             while (two.Count > 1)
             {
                 for (int i = 0; i < 2; i++)
                 {
                     newWood = two[0];
                     two.RemoveAt(0);
                     newWood.packageNumber = count;
                     dist.Add(newWood);
                 }
                 count++;
             }

             while (three.Count > 2)
             {
                 for (int i = 0; i < 3; i++)
                 {
                     newWood = three[0];
                     three.RemoveAt(0);
                     newWood.packageNumber = count;
                     dist.Add(newWood);
                 }
                 count++;
             }

             while (four.Count > 3)
             {
                 for (int i = 0; i < 4; i++)
                 {
                     newWood = four[0];
                     four.RemoveAt(0);
                     newWood.packageNumber = count;
                     dist.Add(newWood);
                 }
                 count++;
             }

             while (five.Count > 4)
             {
                 for (int i = 0; i < 5; i++)
                 {
                     newWood = five[0];
                     five.RemoveAt(0);
                     newWood.packageNumber = count;
                     dist.Add(newWood);
                 }
                 count++;
             }
           while (two.Count > 0 && four.Count > 1)
            {
                if (Math.Abs(two[0].length - four[0].length) <= 162 && Math.Abs(two[0].length - four[1].length) <= 162 && Math.Abs(four[0].length - four[1].length) <= 162)
                {
                    newWood = two[0];
                    two.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    for (int i = 0; i < 2; i++)
                    {
                        newWood = four[0];
                        four.RemoveAt(0);
                        newWood.packageNumber = count;
                        dist.Add(newWood);
                    }
                    count++;
                }
                else
                {
                    break;
                }
            }

          

            while (two.Count > 0 && five.Count > 0 && four.Count > 0)
            {
                if (Math.Abs(two[0].length - five[0].length) <= 162 && Math.Abs(two[0].length - four[0].length) <= 162 && Math.Abs(four[0].length - five[0].length) <= 162)
                {
                    newWood = two[0];
                    two.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    newWood = five[0];
                    five.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    newWood = four[0];
                    four.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    count++;
                }
                else
                {
                    break;
                }
            }

            while (three.Count > 0 && five.Count > 2)
            {
                if (Math.Abs(three[0].length - five[0].length) <= 162 && Math.Abs(three[0].length - five[1].length) <=162 && Math.Abs(five[0].length - five[1].length) <=162)
                {
                    newWood = three[0];
                    three.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    for (int i = 0; i < 2; i++)
                    {
                        newWood = five[0];
                        five.RemoveAt(0);
                        newWood.packageNumber = count;
                        dist.Add(newWood);
                    }
                    count++;
                }
                else
                {
                    break;
                }
            }

            while (two.Count > 0 && five.Count > 1)
            {
                if (Math.Abs(two[0].length - five[0].length) <= 162 && Math.Abs(two[0].length - five[1].length) <= 162 && Math.Abs(five[0].length - five[1].length) <= 162)
                {
                    newWood = two[0];
                    two.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    for (int i = 0; i < 1; i++)
                    {
                        newWood = five[0];
                        five.RemoveAt(0);
                        newWood.packageNumber = count;
                        dist.Add(newWood);
                    }
                    count++;
                }
                else
                {
                    break;
                }
            }

            while (four.Count > 0 && five.Count > 2)
            {
                if (Math.Abs(four[0].length - five[0].length) <= 162 && Math.Abs(four[0].length - five[1].length) <= 162 && Math.Abs(five[0].length - five[1].length) <= 162)
                {
                    newWood = four[0];
                    four.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    for (int i = 0; i < 3; i++)
                    {
                        newWood = five[0];
                        five.RemoveAt(0);
                        newWood.packageNumber = count;
                        dist.Add(newWood);
                    }
                    count++;
                }
                else
                {
                    break;
                }
            }

            while (two.Count > 0 && three.Count > 0)
            {
                if (Math.Abs(two[0].length - three[0].length) <= 162)
                {
                    newWood = two[0];
                    two.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    newWood = three[0];
                    three.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    count++;
                }
                else
                {
                    break;
                }
            }

            while (four.Count > 1 && three.Count > 0)
            {
                if (Math.Abs(three[0].length - four[0].length) <= 162 && Math.Abs(three[0].length - four[1].length) <= 162 && Math.Abs(four[0].length - four[1].length) <= 162)
                {
                    newWood = three[0];
                    three.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    for (int i = 0; i < 2; i++)
                    {
                        newWood = four[0];
                        four.RemoveAt(0);
                        newWood.packageNumber = count;
                        dist.Add(newWood);
                    }
                    count++;
                }
            }

            while (five.Count > 3)
            {
                if (Math.Abs(five[0].length - five[1].length) <= 162 && Math.Abs(five[0].length - five[2].length) <= 162 && Math.Abs(five[0].length - five[3].length) <= 162 && Math.Abs(five[1].length - five[2].length) <= 162 && Math.Abs(five[1].length - five[3].length) <= 162 && Math.Abs(five[2].length - five[3].length) <= 162)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        newWood = five[0];
                        five.RemoveAt(0);
                        newWood.packageNumber = count;
                        dist.Add(newWood);
                    }
                    count++;
                }
                else
                {
                    break;
                }
            }

            while (two.Count > 0 && four.Count > 0)
            {
                if (Math.Abs(two[0].length - four[0].length) <= 162)
                {
                    newWood = two[0];
                    two.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    newWood = four[0];
                    four.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    count++;
                }
                else
                {
                    break;
                }
            }

            while (four.Count > 2)
            {
                if (Math.Abs(four[0].length - four[1].length) <= 162 && Math.Abs(four[0].length - four[2].length) <= 162 && Math.Abs(four[1].length - four[2].length) <= 162)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        newWood = four[0];
                        four.RemoveAt(0);
                        newWood.packageNumber = count;
                        dist.Add(newWood);
                    }
                    count++;
                }
                else
                {
                    break;
                }
            }

            while (three.Count > 0 && five.Count > 1)
            {
                if (Math.Abs(three[0].length - five[0].length) <= 162 && Math.Abs(three[0].length - five[1].length) <= 162 && Math.Abs(five[0].length - four[1].length) <= 162)
                {
                    newWood = three[0];
                    three.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    for (int i = 0; i < 2; i++)
                    {
                        newWood = five[0];
                        five.RemoveAt(0);
                        newWood.packageNumber = count;
                        dist.Add(newWood);
                    }
                    count++;
                
                }
                else
                {
                    break;
                }
            }

            while (four.Count > 1 && five.Count > 0)
            {
                if (Math.Abs(five[0].length - four[0].length) <= 162 && Math.Abs(five[0].length - four[1].length) <= 162 && Math.Abs(four[0].length - four[1].length) <= 162)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        newWood = four[0];
                        four.RemoveAt(0);
                        newWood.packageNumber = count;
                        dist.Add(newWood);
                    }
                    newWood = five[0];
                    five.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    count++;
                }
                else
                {
                    break;
                }
            }

            while (two.Count > 0 && five.Count > 0)
            {
                if (Math.Abs(two[0].length - five[0].length) <= 162)
                {
                    newWood = two[0];
                    two.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    newWood = five[0];
                    five.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    count++;
                }
                else
                {
                    break;
                }
            }

            while (four.Count > 0 && five.Count > 1)
            {
                if (Math.Abs(four[0].length - five[0].length) <= 162 && Math.Abs(four[0].length - five[1].length) <= 162 && Math.Abs(five[0].length - five[1].length) <= 162)
                {
                    newWood = four[0];
                    four.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    for (int i = 0; i < 2; i++)
                    {
                        newWood = five[0];
                        five.RemoveAt(0);
                        newWood.packageNumber = count;
                        dist.Add(newWood);
                    }
                    count++;
                }
                else
                {
                    break;
                }
            }

            while (three.Count > 1)
            {
                if (Math.Abs(three[0].length - three[1].length) <= 162)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        newWood = three[0];
                        three.RemoveAt(0);
                        newWood.packageNumber = count;
                        dist.Add(newWood);
                    }
                    count++;
                }
                else { 
                        break;
                    
                }
            }

            while (five.Count > 2)
            {
                if (Math.Abs(five[0].length - five[1].length) <= 162 && Math.Abs(five[0].length - five[2].length) <= 162 && Math.Abs(five[1].length - five[2].length) <= 162)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        newWood = five[0];
                        five.RemoveAt(0);
                        newWood.packageNumber = count;
                        dist.Add(newWood);
                    }
                    count++;
                }
                else
                {
                    break;
                }
            }

            while (three.Count > 0 && four.Count > 0)
            {
                if (Math.Abs(three[0].length - four[0].length) <= 162)
                {
                    newWood = three[0];
                    three.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    newWood = four[0];
                    four.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    count++;
                }
                else
                {
                    break;
                }
            }

            while (three.Count > 0 && five.Count > 0)
            {
                if (Math.Abs(three[0].length - five[0].length) <= 162)
                {
                    newWood = three[0];
                    three.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    newWood = five[0];
                    five.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    count++;
                }
                else
                {
                    break;
                }
            }
            while (four.Count > 1)
            {
                if (Math.Abs(four[0].length - four[1].length) <= 162)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        newWood = four[0];
                        four.RemoveAt(0);
                        newWood.packageNumber = count;
                        dist.Add(newWood);
                    }
                    count++;
                }
                else
                {
                    break;
                }
            }

            while (four.Count > 0 && five.Count > 0)
            {
                if (Math.Abs(four[0].length - five[0].length) <= 162)
                {
                    newWood = four[0];
                    four.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    newWood = five[0];
                    five.RemoveAt(0);
                    newWood.packageNumber = count;
                    dist.Add(newWood);
                    count++;

                }
                else
                {
                    break;
                }
            }

            while (five.Count > 1)
            {
                if (Math.Abs(five[0].length - five[1].length) <= 162)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        newWood = five[0];
                        five.RemoveAt(0);
                        newWood.packageNumber = count;
                        dist.Add(newWood);
                    }
                    count++;
                }
                else
                {
                    break;
                }
            }

          //  MessageBox.Show(two.Count + ":" + three.Count + ":" + four.Count + ":" + five.Count + ":");

            if (two.Count > 0)
            {

                newWood = two[0];
                two.RemoveAt(0);
                newWood.packageNumber = count;
                dist.Add(newWood);
            }
            if (three.Count > 0)
            {
                newWood = three[0];
                three.RemoveAt(0);
                newWood.packageNumber = count;
                dist.Add(newWood);
            }
            if (four.Count > 0)
            {
                newWood = four[0];
                four.RemoveAt(0);
                newWood.packageNumber = count;
                dist.Add(newWood);
            }
            if (five.Count > 0)
            {
                newWood = five[0];
                five.RemoveAt(0);
                newWood.packageNumber = count;
                dist.Add(newWood);
            }

            dist.AddRange(wrongData);
            dist.AddRange(result);
            dist.Sort();
            
            //foreach (Wood wood in dist)
            //{
            //    MessageBox.Show(wood.length + "," + wood.width + "," + wood.thickness + "," + wood.packageNumber);

            //}*/
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        
    }
}
