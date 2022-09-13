using System;
using System.Data;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class SetupScale : Form
    {
        public Boolean ScaleChanged { get; set; }
        public ScaleComModel ScaleCom { get; set; }
        public string ConnectionString { get; set; }
        public string ClientID { get; set; }
        private Boolean error = false;

        private SerialPort port;
        private string tempSerialReceived = string.Empty;

        public SetupScale()
        {
            InitializeComponent();
        }

        private void SetupScale_Load(object sender, EventArgs e)
        {
            tbClientID.Text = ClientID;
            tbPort.Text = ScaleCom.Port;
            tbBaudrate.Text = ScaleCom.Baudrate;
            tbParity.Text = ScaleCom.Parity;
            tbDatabits.Text = ScaleCom.Databits;
            tbStopbits.Text = ScaleCom.Stopbits;
            tbTerminator.Text = ScaleCom.Terminator;
            tbPrefix.Text = ScaleCom.Prefix;
            tbPostfix.Text = ScaleCom.Postfix;
            checkManual.Checked = ScaleCom.Manual;

            tbOutput.Text = "";

        }

        private void setupPort()
        {
            string comPort = tbPort.Text;
            int baudrate = Convert.ToInt32(tbBaudrate.Text);
            Parity parity = Parity.None;
            string sParity = tbParity.Text;
            switch (sParity)
            {
                case "Even":
                    parity = Parity.Even;
                    break;

                case "mark":
                    parity = Parity.Mark;
                    break;

                case "Odd":
                    parity = Parity.Odd;
                    break;

                case "Space":
                    parity = Parity.Space;
                    break;

                default:
                    parity = Parity.None;
                    break;
            }
            int databit = Convert.ToInt32(tbDatabits.Text);
            StopBits stopBits = StopBits.One;
            string sStopBits = tbStopbits.Text;
            switch (sStopBits)
            {
                case "":
                    stopBits = StopBits.None;
                    break;

                case "0":
                    stopBits = StopBits.None;
                    break;

                case "1.5":
                    stopBits = StopBits.OnePointFive;
                    break;

                case "2":
                    stopBits = StopBits.Two;
                    break;

                default:
                    stopBits = StopBits.One;
                    break;
            }

            port = new SerialPort(comPort, baudrate, parity, databit, stopBits);
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (port != null && port.IsOpen)
            {
                port.Close();
                tbOutput.Text = "Connection Terminated!";
            }
            else
            {
                if (port != null) port.Dispose();
                setupPort();
                port.Open();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            error = false;
            insertSetting("ComPort", tbPort.Text);
            ScaleCom.Port = tbPort.Text;
            insertSetting("ComBaudrate", tbBaudrate.Text);
            ScaleCom.Baudrate = tbBaudrate.Text;
            insertSetting("ComParity", tbParity.Text);
            ScaleCom.Parity = tbParity.Text;
            insertSetting("ComDatabits", tbDatabits.Text);
            ScaleCom.Databits = tbDatabits.Text;
            insertSetting("ComStopbits", tbStopbits.Text);
            ScaleCom.Stopbits = tbStopbits.Text;
            insertSetting("ComTerminator", tbTerminator.Text);
            ScaleCom.Terminator = tbTerminator.Text;
            insertSetting("ComPrefix", tbPrefix.Text);
            ScaleCom.Prefix = tbPrefix.Text;
            insertSetting("ComPostfix", tbPostfix.Text);
            ScaleCom.Postfix = tbPostfix.Text;
            insertSetting("ComManual", checkManual.Checked ? "True" : "False");
            ScaleCom.Manual = checkManual.Checked;

            if (!error)
            {
                MessageBox.Show("Setting saved!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            ScaleChanged = true;
        }

        private void insertSetting(string SettingID, string Val)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    using (SqlCommand UpdateCommand = new SqlCommand("Insert_AppSettings", connection))
                    {
                        connection.Open();
                        UpdateCommand.CommandType = CommandType.StoredProcedure;
                        //receipt
                        UpdateCommand.Parameters.AddWithValue("@SettingID", SettingID);
                        UpdateCommand.Parameters.AddWithValue("@Val", Val);
                        UpdateCommand.Parameters.AddWithValue("@ClientID", ClientID);

                        int Updateresult = UpdateCommand.ExecuteNonQuery();
                    }
                }
                catch
                {
                    error = true;
                    MessageBox.Show("Connection error", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } // using
        }

        private void btnStartTest_Click(object sender, EventArgs e)
        {
            setupPort();

            // Begin com
            setText($"Start serial communication...{Environment.NewLine}", tbOutput);
            try
            {
                port.Open();
            }
            catch (Exception ee)
            {
                setText(ee.Message, tbOutput);
            }
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == System.IO.Ports.SerialData.Eof) return;
            if (!port.IsOpen) return;
            // Show all the incoming data in the port's buffer
            string readMsg = port.ReadLine();

            string prefix = ScaleCom.Prefix;
            string postfix = ScaleCom.Postfix;

            if (readMsg.Contains(prefix))
            {
                if (prefix.Length > 0) { readMsg = readMsg.Replace(prefix, ""); }
                if (postfix.Length > 0) { readMsg = readMsg.Replace(postfix, ""); }
                setText(readMsg, tbOutput);
            }
        }

        private void btnStopTest_Click(object sender, EventArgs e)
        {
            port.Close();
            setText("Connection Terminated.", tbOutput);
        }

        #region ThreadSafe

        public delegate void addTextCallback(string someText, Control tb);

        public delegate void setTextCallback(string someText, Control tb);

        public void addText(string someText, Control tb)
        {
            if (tb.InvokeRequired)
            {
                var d = new addTextCallback(addText);
                this.Invoke(d, new object[] { someText, tb });
            }
            else
            {
                tb.Text += someText;
            }
        }

        public void setText(string someText, Control tb)
        {
            if (tb.InvokeRequired)
            {
                var d = new setTextCallback(setText);
                this.Invoke(d, new object[] { someText, tb });
            }
            else
            {
                tb.Text = someText;
            }
        }

        #endregion ThreadSafe

        private void SetupScale_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (port != null && port.IsOpen)
            {
                //port.Close();
                port.Dispose();
            }
        }
    }
}