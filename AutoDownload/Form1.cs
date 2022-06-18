using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading;


namespace AutoDownload
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(IntPtr classname, string title); // extern method: FindWindow

        [DllImport("user32.dll")]
        static extern void MoveWindow(IntPtr hwnd, int X, int Y,
            int nWidth, int nHeight, bool rePaint); // extern method: MoveWindow

        [DllImport("user32.dll")]
        static extern bool GetWindowRect
            (IntPtr hwnd, out Rectangle rect); // extern method: GetWindowRect

        

        public Form1()
        {
            InitializeComponent();
            Dictionary<string, string> AuthorList = new Dictionary<string, string>();
            //Dictionary comboSource = new Dictionary();
            AuthorList.Add("1", "CB");
            AuthorList.Add("2", "CS");
            AuthorList.Add("3", "CS SCH");
            AuthorList.Add("4", "DSS");
            AuthorList.Add("5", "DS");
            AuthorList.Add("6", "MRK");
            AuthorList.Add("7", "INS");
            AuthorList.Add("8", "CL");
            comboBox1.DataSource = new BindingSource(AuthorList, null);
            comboBox1.DisplayMember = "Value";
            comboBox1.ValueMember = "Key";

            dateTimePicker3.Format = DateTimePickerFormat.Custom;
            dateTimePicker3.CustomFormat = "HH:mm:ss";

            dateTimePicker4.Format = DateTimePickerFormat.Custom;
            dateTimePicker4.CustomFormat = "HH:mm:ss";

            label2.Size = new System.Drawing.Size(200, 30);
            label2.AutoSize =  true;
            
            progressBar1.Minimum = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fname = textBox1.Text;
            if (fname == "")
            {
                //MessageBox.Show("Please Input Folder Name");
                int x = this.Width;
                Point fx = this.Location;
                FindAndMoveMsgBox(fx.X + x / 3, fx.Y, true, "Alert");
                MessageBox.Show("Please Input Folder Name", "Alert");

            }
            else
            {
                //for get IP address from client
                string selected = this.comboBox1.GetItemText(this.comboBox1.SelectedItem);
                string iptarget = "";
                switch (selected)
                {
                    case "CB":
                        iptarget = @"\\172.21.86.150\d$\PICT\";
                        break;
                    case "CS":
                        iptarget = @"\\172.21.86.47\d$\PICT\";
                        break;
                    case "CS SCH":
                        iptarget = @"\\172.21.86.48\d$\PICT\";
                        break;
                    case "DSS":
                        iptarget = @"\\172.21.86.50\d$\PICT\";
                        break;
                    case "DS":
                        iptarget = @"\\172.21.86.44\d$\PICT\";
                        break;
                    case "MRK":
                        iptarget = @"\\172.21.86.56\d$\PICT\";
                        break;
                    case "INS":
                        iptarget = @"\\172.21.86.52\d$\PICT\";
                        break;
                    case "CL":
                        iptarget = @"\\172.21.86.58\d$\System32\Windows\";
                        break;
                }
                string dest = @"D:\" + fname + @"\";

                // For get Date
                string theDate1 = dateTimePicker1.Value.ToString("yyyy-MM-dd") + " " + dateTimePicker3.Value.ToString("HH:mm:ss");
                string theDate2 = dateTimePicker2.Value.ToString("yyyy-MM-dd") + " " + dateTimePicker4.Value.ToString("HH:mm:ss");
                DateTime dt = DateTime.Parse(theDate1);
                DateTime dt2 = DateTime.Parse(theDate2);
                TimeSpan rt = dt2 - dt;
                int tothour = rt.Days * 24 * 60 * 60 + rt.Hours * 60 * 60 + rt.Minutes * 60 + rt.Seconds;
                
                string dx = "";
                int tot =0;
                if (tothour > 0)
                {
                    label2.Visible = true;
                    progressBar1.Maximum = tothour;   
                    Directory.CreateDirectory(dest);
                    for (int i = 1; i <= tothour; i++)
                    {
                        DateTime d2 = dt.AddSeconds(i);
                        string ssname = "Cap " + d2.ToString("yyyy-MM-dd HH-mm-ss") + ".png";
                        string target = iptarget + ssname;
                        if (File.Exists(target))
                        {
                            File.Copy(target, dest + ssname, true);
                           
                            label2.Text = "Copy ... "+ssname;
                            label2.Refresh();
                            tot++;
                        }
                        
                        progressBar1.Value = i;


                    }
                    int x = this.Width;
                    Point fx = this.Location;
                    FindAndMoveMsgBox(fx.X + x / 3, fx.Y, true, "Alert");
                    MessageBox.Show("Copy Done : " + tot + " File","Alert");
                    //MessageBox.Show("Copy Done : "+ tot +" File");
                }
                else
                {
                    int x = this.Width;
                    Point fx = this.Location;
                    FindAndMoveMsgBox(fx.X + x / 3, fx.Y, true, "Alert");
                    MessageBox.Show("Please Check Date and Time", "Alert");
                }


            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string fname = textBox1.Text;
           
            if (fname == "")
            {
                int t = this.Width;
                Point fx1 = this.Location;
                FindAndMoveMsgBox(fx1.X + t / 3, fx1.Y, true, "Alert");
                MessageBox.Show("Folder Not Found", "Alert");
            }
            else
            {
                string local = @"D:\" + fname + @"\";
                Process.Start("explorer.exe", local);
            }
            //string c = @"C:\test\";
            
        }

        void FindAndMoveMsgBox(int x, int y, bool repaint, string title)
        {
            Thread thr = new Thread(() => // create a new thread
            {
                IntPtr msgBox = IntPtr.Zero;
                // while there's no MessageBox, FindWindow returns IntPtr.Zero
                while ((msgBox = FindWindow(IntPtr.Zero, title)) == IntPtr.Zero) ;
                // after the while loop, msgBox is the handle of your MessageBox
                Rectangle r = new Rectangle();
                GetWindowRect(msgBox, out r); // Gets the rectangle of the message box
                MoveWindow(msgBox /* handle of the message box */, x, y,
                   r.Width - r.X /* width of originally message box */,
                   r.Height - r.Y /* height of originally message box */,
                   repaint /* if true, the message box repaints */);
            });
            thr.Start(); // starts the thread
        }
    }
}
