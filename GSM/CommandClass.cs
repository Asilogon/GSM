using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;

namespace GSM
{

    class CommandClass
    {
        
        public Form1 f1 { get; set; }
        public WorkPort WorkP { get; set; }


        public void CloseConnect()
        {
            if (Properties.Settings.Default.ATDStatys)
            {
                Thread.Sleep(2000);
                byte[] arr = System.Text.Encoding.ASCII.GetBytes("+++");
                WorkP.WritePortByte(arr);
                Thread.Sleep(2000);
                
            }
            WorkP.WritePort("ATH");

        }

        /// <summary>
        /// Дозваниваться до абонента
        /// </summary>
        /// <returns></returns>
        public bool CallTelephone(string Namber)
        {
            WorkP.WritePort("AT+CBST=71,0,1", true,20);
            string text = WorkP.WritePort($"ATD{Namber}", true);

            if (text == "CONNECT 9600")
            {
                Properties.Settings.Default.ATDStatys = true;
                return true;
            }
            else { return false; };
        }

        public void test_1()
        {
            if (CallTelephone(f1.NamberPhone.Text)) // Дозваниваеться до абонента, если да - то.. если нет - то..
            {
                WorkP.WritePort("00A0E2004080000000000000FC09");
                WorkP.WritePort("10A0E2000020438E", true, 30);

            }

            CloseConnect();
            return;
        }

    }
}
