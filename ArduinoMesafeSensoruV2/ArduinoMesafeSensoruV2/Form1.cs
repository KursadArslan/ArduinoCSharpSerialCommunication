using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Windows.Forms.DataVisualization.Charting;
namespace ArduinoMesafeSensoruV2
{
    public partial class Form1 : Form
    {
        string[] portlar = SerialPort.GetPortNames();
        DateTime yeni = DateTime.Now;
        int zaman = 0;
        public Form1()
        {
            InitializeComponent();
            label6.Visible = false;
            label6.Text = "";
            picBaglandi.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach(string port in portlar)
            {
                cmbBoxPort.Items.Add(port);
                cmbBoxPort.SelectedIndex = 0;
            }
            cmbBoxSerial.Items.Add("4800");
            cmbBoxSerial.Items.Add("9600");
            cmbBoxSerial.SelectedIndex = 1;
            picKesildi.Visible = true;
            //label4.Text = "Bağlantı Kapalı";
            this.chart1.Titles.Add("Ölçülen Veriler");
            
        }

        private void btnBaglan_Click(object sender, EventArgs e)
        {
            timer1.Start();
            label6.Visible = true;
            if (serialPort1.IsOpen == false)
            {
                if (cmbBoxPort.Text == "")
                    return;
                serialPort1.PortName = cmbBoxPort.Text;
                serialPort1.BaudRate = Convert.ToInt16(cmbBoxSerial.Text);
                try
                {
                    serialPort1.Open();
                    picBaglandi.Visible = true;
                    picKesildi.Visible = false;
                    //label4.Text = "Bağlantı Açık";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                    throw;
                }
            }
            else
                picBaglandi.Visible = true;
                picKesildi.Visible = false;
                //label4.Text = "Bağlantı Kuruldu";
        }

        private void btnDurdur_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();
                picBaglandi.Visible = false;
                picKesildi.Visible = true;
                //label4.Text = "Bağlantı Kapalı";
            }
            progressBar1.Value = 0;
            label6.Text = "Baglanti Kesildi";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {   // string data=serialPort1.ReadExisting();
                double data = Convert.ToDouble(serialPort1.ReadLine());
                label6.Text = data.ToString() + "" + "santimetre";
                //  int data2 = Convert.ToInt16(serialPort1.ReadExisting());
                double prog = (data * 100) / 200;
                // data2 = ((data2 * 5000) / 1023) / 10;
                progressBar1.Value = 100 - Convert.ToInt16(prog);
                // serialPort1.DiscardInBuffer();//bunu silip farkı gor
                //System.Threading.Thread.Sleep(100);
                this.chart1.Series["Veri"].Points.AddXY(zaman, data);
                zaman += 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                timer1.Stop();

            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort1.IsOpen == true)
                serialPort1.Close();
        }
    }
}
