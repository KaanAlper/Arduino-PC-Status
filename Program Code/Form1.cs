using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenHardwareMonitor.Hardware;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PC_Status
{
    
    public partial class Form1 : Form
    {
        int i = 0;   
        static string data;
        Computer c = new Computer()
        {
            GPUEnabled = true,
            CPUEnabled = true
        };
        float value1;
        float value2;

        private SerialPort port = new SerialPort();
        public Form1()
        {
            InitializeComponent();
            Init();
            c.Open();
        }
        private void Init()
        {
            try
            {
                notifyIcon1.Visible = false;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.DataBits = 8;
                port.Handshake = Handshake.None;
                port.RtsEnable = true;
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    comboBoxPorts.Items.Add(port);
                }
                port.BaudRate = 9600;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!port.IsOpen)
                {
                    port.PortName = comboBoxPorts.Text;
                    port.Open();
                    timer1.Interval = Convert.ToInt32(comboBox2.Text);
                    timer1.Enabled = true;
                    toolStripStatusLabel1.Text = "Sending data...";
                    label2.Text = "Connected";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            Status();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                try
                {
                    notifyIcon1.ShowBalloonTip(500, "Arduino", toolStripStatusLabel1.Text, ToolTipIcon.Info);

                }
                catch (Exception ex)
                {

                }
                this.Hide();
            }


        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }
        private void Status()
        {
            foreach (var hardwadre in c.Hardware)
            {

                if (hardwadre.HardwareType == HardwareType.GpuAti)
                {
                    hardwadre.Update();
                    foreach (var sensor in hardwadre.Sensors)
                        if (sensor.SensorType == SensorType.Temperature)
                        {

                            value1 = sensor.Value.GetValueOrDefault();
                        }

                }

                if (hardwadre.HardwareType == HardwareType.CPU)
                {
                    hardwadre.Update();
                    foreach (var sensor in hardwadre.Sensors)
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            value2 = sensor.Value.GetValueOrDefault();

                        }

                }

            }
            try
            {
                port.Write(value1 + "*" + value2 + "#");
            }
            catch (Exception ex)
            {
                timer1.Stop();
                MessageBox.Show(ex.Message);
                toolStripStatusLabel1.Text = "Arduino's not responding...";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.BackColor = System.Drawing.Color.Transparent;
            label2.BackColor = System.Drawing.Color.Transparent;
            label3.BackColor = System.Drawing.Color.Transparent;
            button1.FlatStyle = FlatStyle.Flat;
            button1.BackColor = Color.Transparent;
            button1.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button1.FlatAppearance.MouseOverBackColor = Color.Transparent;


        }

        private void ButtonDisconnect_Click(object sender, EventArgs e)
        {
            

                try
                {

                    port.Write("DIS*");
                    port.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                label2.Text = "Disconnected";
                timer1.Enabled = false;
                toolStripStatusLabel1.Text = "Connect to Arduino...";
                data = "";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (i < 5)
            {
                i++;
            }
            if (i == 5)
            {
                string websiteName = "https://www.uncensoredhentai.moe"; //or simply write the webiste name instead of textBox1.Text      
                System.Diagnostics.Process.Start(websiteName);
                i = 0;
            }

        }
    }
}
