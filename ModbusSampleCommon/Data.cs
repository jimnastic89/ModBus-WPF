using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ModbusTester
{
    public static class Data
    {
        public enum DataFormat { Bits, Bytes, Words }
        public static MainWindow Current;
        public static string IPAddress = "10.0.0.14";
        public static string Unit = "3";
        public static string StartAddress = "30051";
        public static string Size = "100";
        public static bool IsConnected_bool = false;
        public static List<KeyValuePair<string, string>> Data_TextBox_List = Enumerable.Range(1, 64)
            .Select(r => new KeyValuePair<string, string>($"{r}", $""))
            .ToList();
        public static DataFormat ShowDataAs = DataFormat.Bytes;
    }
}
