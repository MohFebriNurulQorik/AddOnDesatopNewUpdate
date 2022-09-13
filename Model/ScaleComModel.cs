namespace ScaleAddon
{
    public class ScaleComModel
    {
        public string Port { get; set; }
        public string Baudrate { get; set; }
        public string Parity { get; set; }
        public string Databits { get; set; }
        public string Stopbits { get; set; }
        public string Terminator { get; set; }
        public string Prefix { get; set; }
        public string Postfix { get; set; }
        public bool Manual { get; set; }
    }
}