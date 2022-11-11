using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSM
{
    public partial class Form1 : Form
    {
        ManualResetEvent mr;
        WorkPort WorkP;
        CommandClass CommandCL384;
        Thread CConnectBatton;

        TextWriter _writer = null;


        /// <summary>
        ///  Фиксирует соединение с COM-портом, и меняеть от татуса активные кнопки
        /// </summary>
        public void ChekConnectBatton()
        {
            Action action = () =>
            {
                if (serialPort1.IsOpen)
                {
                    ConnectCOM.Enabled = false;
                    toolStripButton2.Enabled = true;
                }
                else
                {
                    ConnectCOM.Enabled = true;
                    toolStripButton2.Enabled = false;
                }
            };

            while (true)
            {
                if (InvokeRequired)
                {
                    Invoke(action);
                }
                else action();
            }
        }

        /// <summary>
        /// Загружает последний задействованный COM-порт
        /// </summary>
        private void ComboBoxLoad()
        {
            NamberPY.Text = Properties.Settings.Default.NamberPY;           // Загрузка NamberPY
            NamberPhone.Text = Properties.Settings.Default.NamberPhone;     // Загрузка NamberPhone
            comboBox2.Text = Properties.Settings.Default.CBox2;             // Загрузка ComboBox2

            // Загрузка ComboBox1, с отслеживанием имееться ли такой элемент
            string txt = Properties.Settings.Default.CBox1;
            string[] arr = SerialPort.GetPortNames();
            for (int i=0;i<arr.Length; i++)
                if (txt == arr[i])
                    comboBox1.Text = txt;
        }

        /// <summary>
        /// Настраивает serialPort1 
        /// </summary>
        private void SetSerialPort()
        {
            serialPort1.PortName = comboBox1.Text;                  // Имя порта пример: COM1
            serialPort1.BaudRate = Int32.Parse(comboBox2.Text);     // Скорость обмена данных
            serialPort1.WriteTimeout = 300;                         // Время ожидание записи
            serialPort1.ReadTimeout = 100;                          // Время ожидание ответа
        }

        private async Task DesConnectAsync()
        {
            await Task.Run(() => CommandCL384.CloseConnect());
            serialPort1.Close();
            Properties.Settings.Default.ATDStatys = false;
            Console.WriteLine("Модем отключён");
            return;
        }




        public Form1()
        {
            WorkP = new WorkPort(); // Класс для работа с COM-портом

            CommandCL384 = new CommandClass();
            CommandCL384.f1 = this;
            CommandCL384.WorkP = WorkP;

            //As_ReadPortAllTime = new Thread(WorkP.ReadPortAllTime);
            mr = new ManualResetEvent(true);

            InitializeComponent();

            CConnectBatton = new Thread(ChekConnectBatton);
            CConnectBatton.Start();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            _writer = new TextBoxStreamWriter(textBox2);
            Console.SetOut(_writer);
            Console.WriteLine("--- Страт программы ---");

            ComboBoxLoad();
            
        }
        private async void button1_ClickAsync(object sender, EventArgs e)
        {

            DLMSClass Dlms = new DLMSClass();
            Dlms.DLMS();


            //if (serialPort1.IsOpen)
            //{
            //    CommandCL384.WorkP = WorkP;
            //    await Task.Run(() => CommandCL384.test_1());

            //}

        }

        private async void Form1_FormClosingAsync(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                e.Cancel = true;
                await DesConnectAsync();
                serialPort1.Close();
            }
            e.Cancel = false;
            CConnectBatton.Abort();
            Properties.Settings.Default.ATDStatys = false;
            Properties.Settings.Default.Save();
         }


        private void comboBox1_MouseDown(object sender, MouseEventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(SerialPort.GetPortNames());
        }


        private async void ConnectCOM_ClickAsync(object sender, EventArgs e)
        {

            SetSerialPort(); // Настройка COM-порта
            WorkP.SP = serialPort1; // Передаёт SerialPort в класс

            await Task.Run(() => WorkP.PortOpen());
           
        }

        private async void toolStripButton2_ClickAsync(object sender, EventArgs e)
        {
            await DesConnectAsync();
        }

        
        private void comboBox1_TextChanged(object sender, EventArgs e)  // Вознекает когда меняеться Text в comboBox1
        {
            Properties.Settings.Default.CBox1 = comboBox1.Text; //Сохраняет в память
        }
        private void comboBox2_TextChanged(object sender, EventArgs e)  // Вознекает когда меняеться Text в comboBox2
        {
            Properties.Settings.Default.CBox2 = comboBox2.Text; //Сохраняет в память
        }
        private void NamberPhone_TextChanged(object sender, EventArgs e)   // Вознекает когда меняеться Text в NamberPhone
        {
            Properties.Settings.Default.NamberPhone = NamberPhone.Text; //Сохраняет в память
        }
        private void textBox1_TextChanged(object sender, EventArgs e)       // Вознекает когда меняеться Text в NamberPY
        {
            Properties.Settings.Default.NamberPY = NamberPY.Text; //Сохраняет в память
        }
    }


}
