using Gurux.Common;
using Gurux.DLMS;
using Gurux.DLMS.Enums;
using Gurux.Serial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM
{
    class DLMSClass
    {
        //void InitSerial()
        //{
        //    GXSerial serial = Media as GXSerial;
        //    byte Terminator = (byte)0x0A;
        //    if (serial != null && InitializeIEC)
        //    {
        //        serial.BaudRate = 300;
        //        serial.DataBits = 7;
        //        serial.Parity = Parity.Even;
        //        serial.StopBits = StopBits.One;
        //    }
        //    Media.Open();
        //    //Query device information.
        //    if (Media != null && InitializeIEC)
        //    {
        //        string data = "/?!\r\n";
        //        if (Trace)
        //        {
        //            Console.WriteLine("HDLC sending:" + data);
        //        }
        //        ReceiveParameters<string> p = new ReceiveParameters<string>()
        //        {
        //            Eop = Terminator,
        //            WaitTime = WaitTime
        //        };
        //        lock (Media.Synchronous)
        //        {
        //            Media.Send(data, null);
        //            if (!Media.Receive(p))
        //            {
        //                //Try to move away from mode E.                        
        //                throw new Exception("Failed to receive reply from the device in given time.");
        //            }
        //            //If echo is used.
        //            if (p.Reply == data)
        //            {
        //                p.Reply = null;
        //                if (!Media.Receive(p))
        //                {
        //                    //Try to move away from mode E.                            
        //                    throw new Exception("Failed to receive reply from the device in given time.");
        //                }
        //            }
        //        }
        //        if (Trace)
        //        {
        //            Console.WriteLine("HDLC received: " + p.Reply);
        //        }
        //        if (p.Reply[0] != '/')
        //        {
        //            p.WaitTime = 100;
        //            Media.Receive(p);
        //            throw new Exception("Invalid responce.");
        //        }
        //        string manufactureID = p.Reply.Substring(1, 3);
        //        //UpdateManufactureSettings(manufactureID);
        //        char baudrate = p.Reply[4];
        //        int BaudRate = 0;
        //        switch (baudrate)
        //        {
        //            case '0':
        //                BaudRate = 300;
        //                break;
        //            case '1':
        //                BaudRate = 600;
        //                break;
        //            case '2':
        //                BaudRate = 1200;
        //                break;
        //            case '3':
        //                BaudRate = 2400;
        //                break;
        //            case '4':
        //                BaudRate = 4800;
        //                break;
        //            case '5':
        //                BaudRate = 9600;
        //                break;
        //            case '6':
        //                BaudRate = 19200;
        //                break;
        //            default:
        //                throw new Exception("Unknown baud rate.");
        //        }
        //        Console.WriteLine("BaudRate is :", BaudRate.ToString());
        //        //Send ACK
        //        //Send Protocol control character
        //        byte controlCharacter = (byte)'2';// "2" HDLC protocol procedure (Mode E)
        //                                          //Send Baudrate character
        //                                          //Mode control character 
        //        byte ModeControlCharacter = (byte)'2';//"2" //(HDLC protocol procedure) (Binary mode)
        //                                              //Set mode E.
        //        byte[] arr = new byte[] { 0x06, controlCharacter, (byte)baudrate, ModeControlCharacter, 13, 10 };
        //        if (Trace)
        //        {
        //            Console.WriteLine("Moving to mode E", BitConverter.ToString(arr));
        //        }
        //        lock (Media.Synchronous)
        //        {
        //            Media.Send(arr, null);
        //            p.Reply = null;
        //            p.WaitTime = 500;
        //            if (!Media.Receive(p))
        //            {
        //                //Try to move away from mode E.
        //                this.ReadDLMSPacket(m_Parser.DisconnectRequest());
        //                throw new Exception("Failed to receive reply from the device in given time.");
        //            }
        //        }
        //        if (serial != null)
        //        {
        //            serial.BaudRate = BaudRate;
        //            serial.DataBits = 8;
        //            serial.Parity = Parity.None;
        //            serial.StopBits = StopBits.One;
        //        }
        //    }
        //}


        public GXDLMSClient joinClient()
        {
            GXDLMSClient client = new GXDLMSClient();

            // Используется логическое имя или ссылка на краткое имя.
            client.UseLogicalNameReferencing = true;

            // Используются транспортные уровни HDLC или COSEM для сетей IPv4 (ОБОЛОЧКА)
            client.InterfaceType = InterfaceType.HDLC;

            // Читать http://www.gurux.org/dlmsAddress
            // чтобы узнать, как подсчитываются адреса клиента и сервера.
            // Некоторые производители могут использовать собственные адреса серверов и клиентов.

            client.ClientAddress = 16;
            client.ServerAddress = 11787;

            client.Authentication = Authentication.HighMD5;
            client.Password = ASCIIEncoding.ASCII.GetBytes("");


            return client;
        }


        public void DLMS()
        {
            GXReplyData reply = new GXReplyData();
            GXDLMSClient client = joinClient();

            byte[] data;
            data = client.SNRMRequest();
            if (data != null)
            {
                ReadDLMSPacket(data, reply);
                //Has server accepted client.
                client.ParseUAResponse(reply.Data);
                Console.WriteLine("Анализ ответа UA удался");
            }

        }

    }
}
