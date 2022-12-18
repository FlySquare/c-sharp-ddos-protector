using System.Data;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;

namespace LunaticSky2_Login_Protector
{
    public partial class Form1 : Form
    {
        string [] dIps = new string[] { };
        string [] Ips = new string[] { };
        public Form1()
        {
            InitializeComponent();
        }

        private void checkIps()
        {
            if (checkIpFile())
            {
                var tempList = dIps.ToList();
                label6.Text = DateTime.UtcNow.ToString();
                string text = System.IO.File.ReadAllText(@"C:\ips.txt");
                richTextBox1.Text = text;
                for(int i = 0;i<= richTextBox1.Lines.Length - 1; i++)
                {
                    if (richTextBox1.Lines[i].ToString() != "")
                    {
                        tempList.Add(richTextBox1.Lines[i].ToString());       
                        dIps = tempList.ToArray();
                    }
                }
                Ips = dIps.Distinct().ToArray();
                richTextBox1.Text = "";
                for (int i = 0; i <= Ips.Length - 1; i++)
                {
                    BanIP("LOGIN BAN:" + Ips[i], Ips[i].ToString());
                    richTextBox1.Text += Ips[i].ToString() + "\n";
                }
                richTextBox1.Text += "\n\n" + (Ips.Length) + " Ips blocked!";
            }
            else
            {
                richTextBox1.Text = "No any attacker ip. You are safe";
            }
        }

        private bool checkIpFile()
        {
            if (File.Exists(@"C:\ips.txt"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label6.Text = DateTime.UtcNow.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Green;
            label4.Text = "Active";
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Red;
            label4.Text = "Not Active";
            timer1.Stop();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            checkIps();
            System.IO.File.Delete(@"C:\ips.txt");
        }

        private void BanIP(string RuleName, string IPAddress)
        {
            if (!string.IsNullOrEmpty(RuleName) && !string.IsNullOrEmpty(IPAddress) && new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                using (Process RunCmd = new Process())
                {
                    RunCmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    RunCmd.StartInfo.FileName = "cmd.exe";
                    RunCmd.StartInfo.Arguments = "/C netsh advfirewall firewall add rule name=\"" + RuleName + "\" dir=in interface=any action=block remoteip=" + IPAddress + "/32";
                    RunCmd.StartInfo.Verb = "runas";
                    RunCmd.Start();
                }
            }
        }

        private void UnBanIP(string RuleName)
        {
            if (!string.IsNullOrEmpty(RuleName) && new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                using (Process RunCmd = new Process())
                {
                    RunCmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    RunCmd.StartInfo.FileName = "cmd.exe";
                    RunCmd.StartInfo.Arguments = "/C netsh advfirewall firewall delete rule name = \"" + RuleName + "\"";
                    richTextBox1.Text = "/C netsh advfirewall firewall delete rule name = \"" + RuleName + "\"";
                    RunCmd.StartInfo.Verb = "runas";
                    RunCmd.Start();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                MessageBox.Show("Please fill the ip box");
            }
            else
            {
                UnBanIP("LOGIN BAN:" + textBox1.Text);
                textBox1.Text = "";
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = "chrome";
            process.StartInfo.Arguments = @"https://iamumut.com/";
            process.Start();
        }
    }
}