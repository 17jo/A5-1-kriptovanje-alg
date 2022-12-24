using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace zia51
{
    public partial class Form1 : Form
    {
        private string output;
        private A51 a51;
        private bool fswOn;
        private FileSystemWatcher fsw;
        private string key = "0011100011000110011000110011100111001110101110011001100011101001";
        public Form1()
        {
            InitializeComponent();
            a51 = new A51(key);
            output = AppDomain.CurrentDomain.BaseDirectory + "output\\";
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string pathIn = AppDomain.CurrentDomain.BaseDirectory + "input";
            label3.Text ="WATCHER PATH: "+ pathIn;
            fsw= new FileSystemWatcher(pathIn);

            //fsw.Filter = ".txt";
            fsw.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.Attributes;
            fsw.EnableRaisingEvents = true;
            fsw.IncludeSubdirectories=true;
            fswOn = true;
            button3.BackColor = Color.Green;
            
            this.Text = pathIn;
            fsw.Changed +=Fsw_Changed;
            fsw.Created += Fsw_Changed;
            //fsw.Deleted += Fsw_Changed;
            fsw.Renamed += Fsw_Renamed;           
        }

        private void Fsw_Renamed(object sender, RenamedEventArgs e)
        {
            MessageBox.Show (e.Name);
        }

        private void Fsw_Changed(object sender, FileSystemEventArgs e)
        {
            string text = File.ReadAllText(e.FullPath);

            byte[] bytes = Encoding.ASCII.GetBytes(text);
            BitArray bits = new BitArray(bytes);

            BitArray arr;
            a51.Load();
            arr = a51.Crypt(bits);
            string outputCoded = "";
            for (int i = 0; i < arr.Length; i++)
                outputCoded += Convert.ToInt32(arr[i]);
            output = AppDomain.CurrentDomain.BaseDirectory + "output\\" + e.Name;
            FileStream fs = File.Create(output);
            fs.Close();
            File.WriteAllText(output,outputCoded);
            MessageBox.Show("PREPOZNAT FAJL,I KODIRAN JE U:" + output);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message = textBox1.Text;

            a51.Load();
            //message into bits
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            BitArray bits = new BitArray(bytes);           

            BitArray arr;
            arr = a51.Crypt(bits);
            textBox2.Text = "";
            for (int i = 0; i < arr.Length; i++)
                textBox2.Text +=Convert.ToInt32(arr[i]);           
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ul = textBox2.Text;
            BitArray bits = new BitArray(ul.Length);
            for (int i=0;i<ul.Length;i++)
            {
                bits[i] = Convert.ToBoolean(ul[i] - '0');
            }

            a51.Load();
            //A51 a51 = new A51(key);            
            //dekodiranje            
            BitArray a = a51.Crypt(bits);
            byte[] byteA = new byte[(a.Length - 1) / 8 + 1];
            a.CopyTo(byteA, 0);
            string textA = Encoding.ASCII.GetString(byteA);
            textBox2.Text = textA;
            
        }

        private void button3_Click(object sender, EventArgs e)
        {       
            if(fswOn == true)
            {
                button3.Text = "FileSystemWatcher is OFF";
                button3.BackColor = Color.Red;
                fsw.EnableRaisingEvents = false;
                button4.Visible = true;
                button5.Visible = true;
                label3.Visible = false;
            }
            else
            {
                button3.Text = "FileSystemWatcher is ON";
                button3.BackColor = Color.Green;
                fsw.EnableRaisingEvents = true;
                button4.Visible = false;
                button5.Visible = false;
                label3.Visible = true;
            }            
            fswOn = !fswOn;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text|*.txt|All|*.*";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string message = File.ReadAllText(ofd.FileName);
                a51.Load();
                //message into bits
                byte[] bytes = Encoding.ASCII.GetBytes(message);
                BitArray bits = new BitArray(bytes);
                BitArray arr;
                arr = a51.Crypt(bits);
                string textA = "";
                for (int i = 0; i < arr.Length; i++)
                    textA += Convert.ToInt32(arr[i]);

                output = AppDomain.CurrentDomain.BaseDirectory + "output\\" + ofd.SafeFileName;
                FileStream fs = File.Create(output);
                fs.Close();
                File.WriteAllText(output, textA);
                MessageBox.Show("KODIRANO U:" + output);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text|*.txt|All|*.*";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {  
                string ul = File.ReadAllText(ofd.FileName);
                BitArray bits = new BitArray(ul.Length);
                for (int i = 0; i < ul.Length; i++)
                {
                    bits[i] = Convert.ToBoolean(ul[i] - '0');
                }
                a51.Load();                       
                BitArray a = a51.Crypt(bits);
                byte[] byteA = new byte[(a.Length - 1) / 8 + 1];
                a.CopyTo(byteA, 0);
                string textA = Encoding.ASCII.GetString(byteA); 


                output = AppDomain.CurrentDomain.BaseDirectory + "dekodirano\\" + ofd.SafeFileName;
                FileStream fs = File.Create(output);
                fs.Close();
                File.WriteAllText(output, textA);
                MessageBox.Show("DEKODIRANO U:" + output);
            }
        }
    }
}
