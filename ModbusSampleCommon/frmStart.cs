using System;
using System.Net;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using ModbusTCP;
using ModbusTester;
using System.Windows.Forms.Integration;
using System.Diagnostics;
using System.Collections.Generic;

namespace Modbus
{
    public class frmStart : Form
    {
        private Master MBmaster;
        private TextBox txtData;
        private Label labData;
        private byte[] data;
        public static frmStart Current;

        private ElementHost host;
        private IContainer components;

        public frmStart()
        {
            InitializeComponent();
            Current = this;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code
        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.host = new ElementHost();
            this.SuspendLayout();
            // 
            // host
            // 
            this.host.Dock = DockStyle.Top;
            this.host.Location = new Point(0, 0);
            this.host.Name = "host";
            this.host.Size = new Size(841, 471);
            this.host.TabIndex = 0;
            this.host.Child = new MainWindow();
            // 
            // frmStart
            // 
            this.AutoScaleBaseSize = new Size(5, 15);
            this.ClientSize = new Size(841, 471);
            this.Controls.Add(this.host);
            //this.Controls.Add(this.Data_GroupBox);
            this.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmStart";
            this.Text = "ModbusTCP Tester V1.3";
            this.Closing += new CancelEventHandler(this.frmStart_Closing);
            this.Load += new EventHandler(this.frmStart_Load);
            //this.SizeChanged += new EventHandler(this.frmStart_Resize);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new frmStart());
        }


        // ------------------------------------------------------------------------
        // Program start
        // ------------------------------------------------------------------------
        private void frmStart_Load(object sender, EventArgs e)
        {
            // Set standard format byte, make some textboxes
            data = new byte[0];
            //ResizeData();
        }

        // ------------------------------------------------------------------------
        // Program stop
        // ------------------------------------------------------------------------
        private void frmStart_Closing(object sender, CancelEventArgs e)
        {
            if (MBmaster != null)
            {
                MBmaster.Dispose();
                MBmaster = null;
            }
            Application.Exit();
        }

        // ------------------------------------------------------------------------
        // Button connect
        // ------------------------------------------------------------------------
        public void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // Create new modbus master and add event functions
                MBmaster = new Master(Data.IPAddress, 502, true);
                MBmaster.OnResponseData += new Master.ResponseData(MBmaster_OnResponseData);
                MBmaster.OnException += new Master.ExceptionData(MBmaster_OnException);

                // Show additional fields, enable watchdog
                Data.Current.IsConnected_bool = true;
            }
            catch (SystemException error)
            {
                MessageBox.Show(error.Message);
            }
        }

        // ------------------------------------------------------------------------
        // Button read coils
        // ------------------------------------------------------------------------
        public void btnReadCoils_Click(object sender, EventArgs e)
        {
            ushort ID = 1;
            byte unit = Convert.ToByte(Data.Unit);
            ushort StartAddress = ReadStartAdr();
            UInt16 Length = Convert.ToUInt16(Data.Size);

            MBmaster.ReadCoils(ID, unit, StartAddress, Length);
        }

        // ------------------------------------------------------------------------
        // Button read discrete inputs
        // ------------------------------------------------------------------------
        public void btnReadDisInp_Click(object sender, EventArgs e)
        {
            ushort ID = 2;
            byte unit = Convert.ToByte(Data.Unit);
            ushort StartAddress = ReadStartAdr();
            UInt16 Length = Convert.ToUInt16(Data.Size);

            MBmaster.ReadDiscreteInputs(ID, unit, StartAddress, Length);
        }

        // ------------------------------------------------------------------------
        // Button read holding register
        // ------------------------------------------------------------------------
        public void btnReadHoldReg_Click(object sender, EventArgs e)
        {
            ushort ID = 3;
            byte unit = Convert.ToByte(Data.Unit);
            ushort StartAddress = ReadStartAdr();
            UInt16 Length = Convert.ToUInt16(Data.Size);

            MBmaster.ReadHoldingRegister(ID, unit, StartAddress, Length);
        }

        // ------------------------------------------------------------------------
        // Button read holding register
        // ------------------------------------------------------------------------
        public void btnReadInpReg_Click(object sender, EventArgs e)
        {
            ushort ID = 4;
            byte unit = Convert.ToByte(Data.Unit);
            ushort StartAddress = ReadStartAdr();
            UInt16 Length = Convert.ToUInt16(Data.Size);

            MBmaster.ReadInputRegister(ID, unit, StartAddress, Length);
        }

        // ------------------------------------------------------------------------
        // Button write single coil
        // ------------------------------------------------------------------------
        public void btnWriteSingleCoil_Click(object sender, EventArgs e)
        {
            ushort ID = 5;
            byte unit = Convert.ToByte(Data.Unit);
            ushort StartAddress = ReadStartAdr();
            data = GetData(1);
            Data.Size = "1";

            MBmaster.WriteSingleCoils(ID, unit, StartAddress, Convert.ToBoolean(data[0]));
        }

        // ------------------------------------------------------------------------
        // Button write multiple coils
        // ------------------------------------------------------------------------	
        public void btnWriteMultipleCoils_Click(object sender, EventArgs e)
        {
            ushort ID = 6;
            byte unit = Convert.ToByte(Data.Unit);
            ushort StartAddress = ReadStartAdr();
            UInt16 Length = Convert.ToUInt16(Data.Size);

            data = GetData(Convert.ToUInt16(Data.Size));
            MBmaster.WriteMultipleCoils(ID, unit, StartAddress, Length, data);
        }

        // ------------------------------------------------------------------------
        // Button write single register
        // ------------------------------------------------------------------------
        public void btnWriteSingleReg_Click(object sender, EventArgs e)
        {
            ushort ID = 7;
            byte unit = Convert.ToByte(Data.Unit);
            ushort StartAddress = ReadStartAdr();

            if
                (Data.ShowDataAs == Data.DataFormat.Bits) data = GetData(16);
            else if
                (Data.ShowDataAs == Data.DataFormat.Bytes) data = GetData(2);
            else
                data = GetData(1);

            Data.Size = "1";
            txtData.Text = data[0].ToString();

            MBmaster.WriteSingleRegister(ID, unit, StartAddress, data);
        }

        // ------------------------------------------------------------------------
        // Button write multiple register
        // ------------------------------------------------------------------------	
        public void btnWriteMultipleReg_Click(object sender, EventArgs e)
        {
            ushort ID = 8;
            byte unit = Convert.ToByte(Data.Unit);
            ushort StartAddress = ReadStartAdr();

            data = GetData(Convert.ToByte(Data.Size));
            MBmaster.WriteMultipleRegister(ID, unit, StartAddress, data);
        }

        // ------------------------------------------------------------------------
        // Event for response data
        // ------------------------------------------------------------------------
        private void MBmaster_OnResponseData(ushort ID, byte unit, byte function, byte[] values)
        {
            // ------------------------------------------------------------------
            // Seperate calling threads
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Master.ResponseData(MBmaster_OnResponseData), new object[] { ID, unit, function, values });
                return;
            }

            // ------------------------------------------------------------------------
            // Identify requested data
            switch (ID)
            {
                case 1:
                    //Data_GroupBox.Text = "Read coils";
                    data = values;
                    ShowAs(null, null);
                    break;
                case 2:
                    //Data_GroupBox.Text = "Read discrete inputs";
                    data = values;
                    ShowAs(null, null);
                    break;
                case 3:
                    //Data_GroupBox.Text = "Read holding register";
                    data = values;
                    ShowAs(null, null);
                    break;
                case 4:
                    //Data_GroupBox.Text = "Read input register";
                    data = values;
                    ShowAs(null, null);
                    break;
                case 5:
                    //Data_GroupBox.Text = "Write single coil";
                    break;
                case 6:
                    //Data_GroupBox.Text = "Write multiple coils";
                    break;
                case 7:
                    //Data_GroupBox.Text = "Write single register";
                    break;
                case 8:
                    //Data_GroupBox.Text = "Write multiple register";
                    break;
            }
        }

        // ------------------------------------------------------------------------
        // Modbus TCP slave exception
        // ------------------------------------------------------------------------
        private void MBmaster_OnException(ushort id, byte unit, byte function, byte exception)
        {
            string exc = "Modbus error: ";
            switch (exception)
            {
                case Master.excIllegalFunction:
                    exc += "Illegal function"; break;
                case Master.excIllegalDataAdr:
                    exc += "Illegal data address"; break;
                case Master.excIllegalDataVal:
                    exc += "Illegal data value"; break;
                case Master.excSlaveDeviceFailure:
                    exc += "Slave device failure"; break;
                case Master.excAck:
                    exc += "Acknowledge failure"; break;
                case Master.excGatePathUnavailable:
                    exc += "Gateway path unavailbale"; break;
                case Master.excExceptionTimeout:
                    exc += "Slave timed out"; break;
                case Master.excExceptionConnectionLost:
                    exc += "Connection lost"; break;
                case Master.excExceptionNotConnected:
                    exc += "Not connected"; break;
            }

            MessageBox.Show(exc, "Modbus slave exception");
        }

        // ------------------------------------------------------------------------
        // Read start address
        // ------------------------------------------------------------------------
        private ushort ReadStartAdr()
        {
            // Convert hex numbers into decimal
            if (Data.StartAddress.IndexOf("0x", 0, Data.StartAddress.Length) == 0)
            {
                string str = Data.StartAddress.Replace("0x", "");
                ushort hex = Convert.ToUInt16(str, 16);
                return hex;
            }
            else
            {
                return Convert.ToUInt16(Data.StartAddress);
            }
        }

        // ------------------------------------------------------------------------
        // Read values from textboxes
        // ------------------------------------------------------------------------
        private byte[] GetData(int num)
        {
            bool[] bits = new bool[num];
            byte[] data = new Byte[num];
            int[] word = new int[num];

            // ------------------------------------------------------------------------
            // Convert data
            for (int i = 0; i < Data.Current.Data_TextBox_List.Count; i++)
            {
                int x = Convert.ToInt16(Data.Current.Data_TextBox_List[i].Key);
                if (Data.ShowDataAs == Data.DataFormat.Bits)
                {
                    if ((x <= bits.GetUpperBound(0)) && (Data.Current.Data_TextBox_List[i].Value != "")) bits[x] = Convert.ToBoolean(Convert.ToByte(Data.Current.Data_TextBox_List[i].Value));
                    else break;
                }
                if (Data.ShowDataAs == Data.DataFormat.Bytes)
                {
                    if ((x <= data.GetUpperBound(0)) && (Data.Current.Data_TextBox_List[i].Value != "")) data[x] = Convert.ToByte(Data.Current.Data_TextBox_List[i].Value);
                    else break;
                }
                if (Data.ShowDataAs == Data.DataFormat.Words)
                {
                    if ((x <= word.GetUpperBound(0)) && (Data.Current.Data_TextBox_List[i].Value != ""))
                    {
                        try { word[x] = Convert.ToInt16(Data.Current.Data_TextBox_List[i].Value); }
                        catch (SystemException) { word[x] = Convert.ToUInt16(Data.Current.Data_TextBox_List[i].Value); };
                    }
                    else break;
                }
            }

            if (Data.ShowDataAs == Data.DataFormat.Bits)
            {
                int numBytes = (num / 8 + (num % 8 > 0 ? 1 : 0));
                data = new Byte[numBytes];
                BitArray bitArray = new BitArray(bits);
                bitArray.CopyTo(data, 0);
            }
            if (Data.ShowDataAs == Data.DataFormat.Words)
            {
                data = new Byte[num * 2];
                for (int x = 0; x < num; x++)
                {
                    byte[] dat = BitConverter.GetBytes((short)IPAddress.HostToNetworkOrder((short)word[x]));
                    data[x * 2] = dat[0];
                    data[x * 2 + 1] = dat[1];
                }
            }
            return data;
        }

        // ------------------------------------------------------------------------
        // Show values in selected way
        // ------------------------------------------------------------------------
        public void ShowAs(object sender, EventArgs e)
        {
            bool[] bits = new bool[1];
            int[] word = new int[1];

            // Convert data to selected data type
            if (Data.ShowDataAs == Data.DataFormat.Bits)
            {
                BitArray bitArray = new BitArray(data);
                bits = new bool[bitArray.Count];
                bitArray.CopyTo(bits, 0);
            }
            if (Data.ShowDataAs == Data.DataFormat.Words)
            {
                if (data.Length < 2) return;
                int length = data.Length / 2 + Convert.ToInt16(data.Length % 2 > 0);
                word = new int[length];
                for (int x = 0; x < length; x += 1)
                {
                    word[x] = data[x * 2] * 256 + data[x * 2 + 1];
                }
            }

            // ------------------------------------------------------------------------
            // Create new data list
            List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
            for (int i = 0; i < Data.Current.Data_TextBox_List.Count; i++)
            {
                string newValue = "";

                if (Data.ShowDataAs == Data.DataFormat.Bits)
                    newValue = i <= bits.GetUpperBound(0) ? $"{Convert.ToByte(bits[i])}" : "";
                else if (Data.ShowDataAs == Data.DataFormat.Bytes)
                    newValue = i <= data.GetUpperBound(0) ? $"{data[i]}" : "";
                else if (Data.ShowDataAs == Data.DataFormat.Words)
                    newValue = i <= word.GetUpperBound(0) ? $"{word[i]}" : "";

                tempList.Add(new KeyValuePair<string, string>($"{i}", newValue));
            }
            Data.Current.Data_TextBox_List = tempList;
        }

    }
}
