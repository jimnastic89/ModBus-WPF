using Modbus;
using ModbusTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace ModbusTester
{
    public partial class MainWindow : UserControl, INotifyPropertyChanged
    {
        public Master MB_Master {  get => Data.MB_Master; set => SetValue(ref Data.MB_Master, value); }
        public string IPAddress { get => Data.IPAddress; set => SetValue(ref Data.IPAddress, value); }
        public string Unit { get => Data.Unit; set => SetValue(ref Data.Unit, value); }
        public string StartAddress { get => Data.StartAddress; set => SetValue(ref Data.StartAddress, value); }
        public string Size { get => Data.Size; set => SetValue(ref Data.Size, value); }
        public List<KeyValuePair<string, string>> Data_TextBox_List { get => Data.Data_TextBox_List; set => SetValue(ref Data.Data_TextBox_List, value); }
        public Data.DataFormat ShowDataAs { get => Data.ShowDataAs; set => SetValue(ref Data.ShowDataAs, value); }
        public bool IsConnected_bool { get => Data.IsConnected_bool; set => SetValue(ref Data.IsConnected_bool, value); }
        public MainWindow()
        {
            InitializeComponent();
            Data.Current = this;
        }

        #region SetValue
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            if (propertyName != null)
                OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            frmStart.Current.btnConnect_Click(sender, e);
        }

        private void ReadCoils_Click(object sender, RoutedEventArgs e)
        {
            frmStart.Current.btnReadCoils_Click(sender, e);
        }

        private void ReadHoldingRegister_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine($"IP: {IPAddress} Unit: {Unit} StartAddress: {StartAddress} Size: {Size}");
            frmStart.Current.btnReadHoldReg_Click(sender, e);
        }

        private void ReadDiscreteInputs_Click(object sender, RoutedEventArgs e)
        {
            frmStart.Current.btnReadDisInp_Click(sender, e);
        }

        private void WriteMultipleRegisters_Click(object sender, RoutedEventArgs e)
        {
            frmStart.Current.btnWriteMultipleReg_Click(sender, e);
        }

        private void WriteMultipleCoils_Click(object sender, RoutedEventArgs e)
        {
            frmStart.Current.btnWriteMultipleCoils_Click(sender, e);
        }

        private void WriteSingleRegister_Click(object sender, RoutedEventArgs e)
        {
            frmStart.Current.btnWriteSingleReg_Click(sender, e);
        }

        private void WriteSingleCoil_Click(object sender, RoutedEventArgs e)
        {
            frmStart.Current.btnWriteSingleCoil_Click(sender, e);
        }

        private void ReadInputRegister_Click(object sender, RoutedEventArgs e)
        {
            frmStart.Current.btnReadInpReg_Click(sender, e);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            switch (radioButton.Content)
            {
                case "Bits":
                    ShowDataAs = Data.DataFormat.Bits;
                    break;
                case "Bytes":
                    ShowDataAs = Data.DataFormat.Bytes;
                    break;
                case "Words":
                    ShowDataAs = Data.DataFormat.Words;
                    break;
            }
            if (frmStart.Current != null)
                frmStart.Current.ShowAs(sender, e);
        }
    }
}
