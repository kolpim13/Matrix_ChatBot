using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot
{
    public static class UART
    {
        private static SerialPort _serial;
        private static byte[] buffer_in;
        private static byte[] buffer_out;

        private static bool flag_ready;

        public static string PortName
        {
            get { return _serial.PortName; }
            set { _serial.PortName = value; }
        }
        public static string[] GetPortNames
        {
            get { return SerialPort.GetPortNames(); }
        }

        public static byte[] Get_buf_in
        {
            get { return buffer_in; }
        }
        public static byte[] Get_buf_out
        {
            get { return buffer_out; }
        }

        static UART() { }

        public static void Init()
        {
            _serial = new SerialPort();
            buffer_in = new byte[256];
            buffer_out = new byte[256];

            _serial.BaudRate = 76800;
            //_serial.BaudRate = 9600;
            _serial.Parity = Parity.None;
            _serial.StopBits = StopBits.One;
            _serial.DataBits = 8;

            _serial.ReadTimeout = 500;
            _serial.WriteTimeout = 500;

            flag_ready = true;
        }
        public static bool Open(string port_name)
        {
            if (_serial.IsOpen)
            {
                return false;
            }
            try
            {
                PortName = port_name;
                _serial.Open();
            }
            catch(ArgumentException ex)
            {
                return false;
            }

            return true;
        }
        public static bool Close()
        {
            if (_serial.IsOpen == false)
            {
                return false;
            }

            _serial.Close();
            return true;
        }

        public static void Write(int len)
        {
            if (flag_ready == true)
            {
                flag_ready = false;
                //_serial.Write(buffer_out, 0, len);
                for (int i = 0; i < len; i++)
                {
                    _serial.Write(buffer_out, i, 1);
                }
                flag_ready = true;
            }
        }
        public static void Write(byte[] array, int shift, int len)
        {
            if (flag_ready == true)
            {
                flag_ready = false;
                //_serial.Write(buffer_out, 0, len);
                for (int i = 0; i < len; i++)
                {
                    _serial.Write(array, shift, 1);
                }
                flag_ready = true;
            }
        }
        public static string Read()
        {
            return _serial.ReadLine();
            //_serial.ReadExisting();
            //string str = _serial.ReadTo("\n");
            //return str;
        }

        // For tests with other core
        public static void Write2(int len)
        {
            _serial.Write(buffer_out, 0, len);
        }
        public static void Write2(byte[] buf, int shift, int len)
        {
            //_serial.Write(buf, shift, len);
            for (int i = shift; i < len + shift; i++)
            {
                _serial.Write(buf, i, 1);
            }
        }
    }
}
