using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;


namespace GSM
{
    class TextBoxStreamWriter : TextWriter
    {
        delegate void MyAppendText(string message);
        MyAppendText invoker;

        TextBox _output = null;

        public TextBoxStreamWriter(TextBox output)
        {
            _output = output;
        }


        public override void Write(char value)
        {
            invoker = _output.AppendText;
            base.Write(value);
            if (_output.InvokeRequired)
            {
                _output.Invoke(invoker, value.ToString());
            }
            else
            {
                _output.AppendText(value.ToString());
            }
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }

    }
}
