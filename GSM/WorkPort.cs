using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Text.RegularExpressions;

namespace GSM
{
    class WorkPort
    {

        public SerialPort SP { get; set; }



        /// <summary>
        /// Отрпавляет команду в COM-порт и в консоль
        /// </summary>
        /// <param name="command"></param>
        public string WritePortByte(byte[] command, bool WaitAnswer = false, int time = 60)
        {
            SP.Write(command,0,command.Length);

            string str = Encoding.Default.GetString(command);
            Console.WriteLine($"Отправлено -> {str}");
            if (WaitAnswer)
                return ReadPortAllTime(time);
            return "";
        }

        /// <summary>
        /// Отрпавляет команду в COM-порт и в консоль
        /// </summary>
        /// <param name="command"></param>
        public string WritePort(string command, bool WaitAnswer = false, int time = 60)
        {
            try
            {
                SP.Write(command + "\r\n");
                Console.WriteLine($"Отправлено -> {command}");
                if (WaitAnswer)
                    return ReadPortAllTime(time);
            }
            catch
            {

            }
            return "";
        }


        /// <summary>
        /// Открывает соединение с COM-портом
        /// </summary>
        public void PortOpen()
        {
            while (SP.IsOpen != true)  //Выполнять пока порт не открыт
            {
                try
                {
                    SP.Open();
                    Console.WriteLine("Подключение к модему выполнено успешно");
                }
                catch (UnauthorizedAccessException _ex)
                {
                    Console.WriteLine($"Ошибка: {_ex.Message}");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        /// Читает постоянно все данные полученые с COM-порта.
        /// </summary>
        public string ReadPortAllTime(int time)
        {
            var now = DateTime.Now;
            while ((DateTime.Now - now).Seconds < time)
            {
                try
                {
                    string text = SP.ReadExisting();
                    if (text.Length > 0)
                    {
                        text = string.Join("", Regex.Split(text, @"(?:\r\n|\n|\r)"));
                        Console.WriteLine($"Получено -> {text}");
                        return text;
                    }
                    else { System.Threading.Thread.Sleep(100); }
                }
                catch
                {

                }
                
            }
            Console.WriteLine($"Привышено время ожидание команды");
            return "";
        }

    }

}
