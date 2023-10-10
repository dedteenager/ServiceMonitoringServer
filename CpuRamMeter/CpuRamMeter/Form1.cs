

using Microsoft.Win32;
using System;
using System.ServiceProcess;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Remoting.Lifetime;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.IO;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Mail;

namespace CpuRamMeter
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public string path = @"\CpuRamLog\log.txt";
        public Form1()
        {
            InitializeComponent();
            
            using (FileStream fs = File.Create(path))
                openFileDialog1.Filter = "Execute files(*.exe)|*.exe";
            File.AppendAllText(path, Environment.NewLine);
            File.AppendAllText(path, "Программа запущена " +"Дата: " + DateTime.Now.ToString("HH:mm:ss ") + DateTime.Now.ToString("dd MMMM yyyy"));
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            float fcpu = perfCounterCPU.NextValue();
            float fram = perfCounterRAM.NextValue();
            

            metroProgressBarCPU.Value = (int)fcpu;
            metroProgressBarRAM.Value = (int)fram;


            metroLabelCPU.Text = string.Format("{0:0.00}%", fcpu);
            metroLabelRAM.Text = string.Format("{0:0.00}%", fram);


            chart1.Series["CPU"].Points.AddY(fcpu);
            chart1.Series["RAM"].Points.AddY(fram);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            timer.Start();
        }



        private void button1_Click(object sender, EventArgs e)
        {

            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            registryKey.SetValue(txtName.Text, txtPath.Text);
            MessageBox.Show(
        "Программа " + txtName.Text + " добавлена в автозагрузку",
        "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            File.AppendAllText(path, Environment.NewLine);
            File.AppendAllText(path, $"Программа "+txtName.Text+" добавлена в автозагрузку " + "Дата: " + DateTime.Now.ToString("HH:mm:ss ") + DateTime.Now.ToString("dd MMMM yyyy"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            registryKey.DeleteValue(txtName.Text, false);
            MessageBox.Show(
        "Программа " + txtNameDel.Text + " удалена из автозагрузки",
        "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            File.AppendAllText(path, Environment.NewLine);
            File.AppendAllText(path, $"Программа " + txtNameDel.Text + " удалена из автозагрузки " + "Дата: " + DateTime.Now.ToString("HH:mm:ss ") + DateTime.Now.ToString("dd MMMM yyyy"));
        }

        ///путь к файлу для автозагрузки
        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            txtPath.Text = filename;
        }

        //проверка на наличие в автозагрузке
        private void button3_Click(object sender, EventArgs e)
        {
            bool check = IsStartupEnabled();
            if (check)
            {
                MessageBox.Show(
            "Программа " + txtCheck.Text + " есть в автозагрзке",
            "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {
                MessageBox.Show(
            "Программы " + txtCheck.Text + " нет в автозагрзке",
            "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            bool IsStartupEnabled()
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                return registryKey.GetValue(txtCheck.Text) != null;
            }
        }

        //очистка ДНС-кэша
        private void button5_Click(object sender, EventArgs e)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "ipconfig /flushdns";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();

            File.AppendAllText(path, Environment.NewLine);
            File.AppendAllText(path, $"Очистка ДНС кэша" + DateTime.Now.ToString("HH:mm:ss ") + DateTime.Now.ToString("dd MMMM yyyy"));
        }

        //очистка temp
        private void button6_Click(object sender, EventArgs e)
        {

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c del /s /q %temp%";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();
            File.AppendAllText(path, Environment.NewLine);
            File.AppendAllText(path, $"Очистка ДНС кэша" + DateTime.Now.ToString("HH:mm:ss ") + DateTime.Now.ToString("dd MMMM yyyy"));
        }

        //перезапуск DNS
        private void button7_Click(object sender, EventArgs e)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c net stop dnscache";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();

            Process process2 = new Process();
            ProcessStartInfo startInfo2 = new ProcessStartInfo();
            startInfo2.WindowStyle = ProcessWindowStyle.Normal;
            startInfo2.FileName = "cmd.exe";
            startInfo2.Arguments = "/c net start dnscache";
            startInfo2.RedirectStandardOutput = true;
            startInfo2.UseShellExecute = false;
            process.StartInfo = startInfo2;
            process2.Start();

            File.AppendAllText(path, Environment.NewLine);
            File.AppendAllText(path, $"Перезапущен DNS" + DateTime.Now.ToString("HH:mm:ss ") + DateTime.Now.ToString("dd MMMM yyyy"));

        }

        private void button8_Click(object sender, EventArgs e)
        {

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c ipconfig /release";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();

            File.AppendAllText(path, Environment.NewLine);
            File.AppendAllText(path, $"Отключен Интернет" + DateTime.Now.ToString("HH:mm:ss ") + DateTime.Now.ToString("dd MMMM yyyy"));

        }

        private void button9_Click(object sender, EventArgs e)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c ipconfig /renew";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();
            File.AppendAllText(path, Environment.NewLine);
            File.AppendAllText(path, $"Включен Интернет" + DateTime.Now.ToString("HH:mm:ss ") + DateTime.Now.ToString("dd MMMM yyyy"));

        }

        private void button10_Click(object sender, EventArgs e)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c cleanmgr /sageset";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();

            File.AppendAllText(path, Environment.NewLine);
            File.AppendAllText(path, $"Проведена очистка диска" + DateTime.Now.ToString("HH:mm:ss ") + DateTime.Now.ToString("dd MMMM yyyy"));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ServiceController service = new ServiceController(txtService.Text);
            if (service.Status != ServiceControllerStatus.Running)
            {
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMinutes(1));
                Console.WriteLine("Служба была успешно запущена!");
            }
            else
            {
                Console.WriteLine("Служба уже запущена!");
            }
            File.AppendAllText(path, Environment.NewLine);
            File.AppendAllText(path, $"Запуск службы {txtService.Text}" + DateTime.Now.ToString("HH:mm:ss ") + DateTime.Now.ToString("dd MMMM yyyy"));
        }

        private void button12_Click(object sender, EventArgs e)
        {
            ServiceController service = new ServiceController(txtService.Text);
            if (service.Status != ServiceControllerStatus.Stopped)
            {
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMinutes(1));
                Console.WriteLine("Служба была успешно остановлена!");
            }
            else
            {
                Console.WriteLine("Служба уже остановлена!");
            }
            File.AppendAllText(path, Environment.NewLine);
            File.AppendAllText(path, $"Остановка службы {txtService.Text}" + DateTime.Now.ToString("HH:mm:ss ") + DateTime.Now.ToString("dd MMMM yyyy"));
        }

        private void button13_Click(object sender, EventArgs e)
        {
            File.AppendAllText(path, Environment.NewLine);
            File.AppendAllText(path, $"Отключение электропитания {txtService.Text}" + DateTime.Now.ToString("HH:mm:ss ") + DateTime.Now.ToString("dd MMMM yyyy"));
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c shutdown /s";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.Start();

        }
    }


}
