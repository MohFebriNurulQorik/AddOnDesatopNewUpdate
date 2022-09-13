using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ScaleAddon
{
    public partial class MainForm : Form
    {
        //public static string ConnectionString = "Data Source= DESKTOP-LG5JEVK\\SQLEXPRESS; Initial Catalog=ScaleAddon;user=sa; Password=Demo1234";
        public static string ConnectionString = $"Data Source= {Properties.Settings.Default.DBServer}; Initial Catalog={Properties.Settings.Default.DBScheme};user={Properties.Settings.Default.DBUser}; Password={Properties.Settings.Default.DBPassword}";

        //public static string AcumaticaSiteURL = null;
        //public static string AcumaticaEndpointName = null;
        //public static string AcumaticaEndpointVersion = null;
        //public static string AcumaticaUser = null;
        //public static string AcumaticaPassword = null;
        //public static string AcumaticaTenant = null;
        //public static string AcumaticaBranch = null;
        //public static string AcumaticaLocale = null;

        public static string ClientID = Properties.Settings.Default.ClientID;

        public static UserModel Userlog = new UserModel();
        public static ScaleComModel ScaleCom = new ScaleComModel();
        public static ScaleCalibrationModel ScaleCalibration = new ScaleCalibrationModel();
        public static AcumaticaCredModel AcumaticaCred = new AcumaticaCredModel();
        public static WarehouseModel Warehouse = new WarehouseModel();
        public static FiscalInfo FiscalInfo = new FiscalInfo();

        public Controls.ucUsers ControlUser;
        public Controls.ucVendor ControlVendor;
        public Controls.ucWarehouse ControlWarehouse;
        public Controls.ucItemAttribute ControlItemAttribute;
        public Controls.ucTobaccoProcess ControlTobaccoProcess;
        public Controls.ucTobaccoGrade ControlTobaccoGrade;
        public Controls.ucTobaccoPrice ControlTobaccoPrice;
        public Controls.ucInventory ControlInventory;
        public Controls.ucInventoryImport ControlInventoryImport;
        public Controls.ucSubItem ControlSubItem;

        public Controls.ucBuyingRegistration ControlBuyingRegistration;
        public Controls.ucBuyingQC ControlBuyingQC;
        public Controls.ucBuyingProcess ControlBuyingProcess;
        public Controls.ucPurchaseInvoice ControlPurchaseInvoice;
        public Controls.ucReclassDirect ControlReclassDirect;
        public Controls.ucReclassProcess ControlReclassProcess;
        //public Controls.ucFermentDirect ControlFermentDirect;

        //public Controls.ucBirIN ControlBirIN;
        //public Controls.ucBirOUT ControlBirOUT;
        //public Controls.ucPras1IN ControlPras1IN;
        //public Controls.ucPras2IN ControlPras2IN;
        //public Controls.ucSortasiIN ControlSortasiIN;
        //public Controls.ucFinalSortasiIN ControlFinalSortasiIN;
        //public Controls.ucButtIN ControlButtIN;
        //public Controls.ucButtOUT ControlButtOUT;
        //public Controls.ucStripIN ControlStripIN;
        //public Controls.ucLooseIN ControlLooseIN;
        //public Controls.ucSortasiPackingIN ControlSortasiPackingIN;
        //public Controls.ucTempPackingIN ControlTempPackingIN;
        //public Controls.ucFinalPackingIN ControlFinalPackingIN;
        //public Controls.ucFermentasiIN ControlFermentasiIN;
        public Controls.ucProcessingINGeneric ControlProcessingINGeneric;
        public Controls.ucReturnProcessingINGeneric ucReturnProcessingINGeneric;
        public Controls.ucReturnBuying ucReturnBuying;
        public Controls.ucDirectPacking ControlDirectPacking;
        public Controls.ucDirectTempPacking ControlDirectTempPacking;

        public Controls.ucProcessingOUTGeneric ControlProcessingOUTGeneric;
        public Controls.ucInventoryStock ControlStock;
        public Controls.ucInventoryStockReport ControlStockReport;
        public Controls.ucWeightAdjust ControlWeightAdjust;
        public Controls.ucInventoryAdjust ControlInventoryAdjust;
        public Controls.ucDispatchOUT ControlDispatchOUT;
        public Controls.ucDispatchIN ControlDispatchIN;
        public Controls.ucShipmentInfo ControlShipmentInfo;

        public Forms.ScaleCalibration2 scaleCalibration2; 

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            loadSetting();
            this.Text = $"Universal Leaf [{Warehouse.Descr}]";
            doLogin();
        }

        private void loadSetting()
        {
            var WarehouseID = "";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dtSettings = new DataTable();
                try
                {
                    //string query = $"select * from AppSettings WHERE  SettingID='WarehouseID'";
                    string query = $"SELECT * FROM AppSettings WHERE ClientID = '{ClientID}'";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtSettings);

                    foreach (DataRow setting in dtSettings.Rows)
                    {
                        switch ((string)setting[0])
                        {
                            case "WarehouseID":
                                WarehouseID = (string)setting[1];
                                break;

                            case "AcumaticaSiteURL":
                                AcumaticaCred.AcumaticaSiteURL = (string)setting[1];
                                break;

                            case "AcumaticaTenant":
                                AcumaticaCred.AcumaticaTenant = (string)setting[1];
                                break;

                            case "AcumaticaEndpointName":
                                AcumaticaCred.AcumaticaEndpointName = (string)setting[1];
                                break;

                            case "AcumaticaEndpointVersion":
                                AcumaticaCred.AcumaticaEndpointVersion = (string)setting[1];
                                break;

                            case "AcumaticaInvLocation":
                                AcumaticaCred.AcumaticaInvLocation = (string)setting[1];
                                break;

                            case "AcumaticaUser":
                                AcumaticaCred.AcumaticaUser = (string)setting[1];
                                break;

                            case "AcumaticaPassword":
                                AcumaticaCred.AcumaticaPassword = (string)setting[1];
                                break;

                            case "ComPort":
                                ScaleCom.Port = (string)setting[1];
                                break;

                            case "ComBaudrate":
                                ScaleCom.Baudrate = (string)setting[1];
                                break;

                            case "ComParity":
                                ScaleCom.Parity = (string)setting[1];
                                break;

                            case "ComDatabits":
                                ScaleCom.Databits = (string)setting[1];
                                break;

                            case "ComStopbits":
                                ScaleCom.Stopbits = (string)setting[1];
                                break;

                            case "ComTerminator":
                                ScaleCom.Terminator = (string)setting[1];
                                break;

                            case "ComPrefix":
                                ScaleCom.Prefix = (string)setting[1];
                                break;

                            case "ComPostfix":
                                ScaleCom.Postfix = (string)setting[1];
                                break;

                            case "ComManual":
                                ScaleCom.Manual = Convert.ToBoolean(setting[1]);
                                break;

                            case "FiscalMonth":
                                FiscalInfo.StartingFiscalMonth = Convert.ToInt32(setting[1]);
                                DateTime currentDate = DateTime.Now;
                                FiscalInfo.CurrentFiscalYear = currentDate.AddMonths(-FiscalInfo.StartingFiscalMonth).AddMonths(1).Year;
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }//end using connection

            Warehouse.ClearWarehouse();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    string query = $"select * from WarehouseSite WHERE WarehouseID='{WarehouseID}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        Warehouse.WarehouseID = reader.GetValue(0).ToString();
                        Warehouse.Descr = reader.GetValue(1).ToString();
                        Warehouse.Company = reader.GetValue(3).ToString();
                        Warehouse.AddressLine1 = reader.GetValue(4).ToString();
                        Warehouse.AddressLine2 = reader.GetValue(5).ToString();
                        Warehouse.Phone1 = reader.GetValue(6).ToString();
                        Warehouse.Phone2 = reader.GetValue(7).ToString();
                        Warehouse.Branch = reader.GetValue(8).ToString();

                        reader.Close();
                    }
                    else
                    {
                        reader.Close();

                        MessageBox.Show("Cannot find warehouse info", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }//end using connection

            ScaleCalibration.ClearCalibration();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    string query = $"select * from ScaleCalibration WHERE WarehouseID='{WarehouseID}' AND ClientID = '{ClientID}' ORDER BY DocumentDate Desc";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        ScaleCalibration.DocumentID = reader.GetValue(0).ToString();
                        ScaleCalibration.DocumentDate = reader.GetValue(1).ToString();
                        ScaleCalibration.WarehouseID = reader.GetValue(3).ToString();
                        ScaleCalibration.ClientID = reader.GetValue(4).ToString();
                        ScaleCalibration.CreatorID = reader.GetValue(5).ToString();

                        reader.Close();
                    }
                    else
                    {
                        reader.Close();

                        MessageBox.Show("Cannot find calibration info", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }//end using connection
        }

        private void doLogin()
        {
            Userlog.ClearUser();
            toolStripStatusLabel1.Text = $"Logged out";
            MainPanel.Controls.Clear();
            resetAccess();

            LoginForm MyLogin = new LoginForm
            {
                ConnectionString = ConnectionString,

                Warehouse = Warehouse,
                ClientID = ClientID
            };
            MyLogin.ShowDialog();

            if (!MyLogin.LoginCancel)
            {
                string loginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                toolStripStatusLabel1.Text = $"Login as {Userlog.Fullname} on {ClientID} at {loginTime} [App version {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}-R2ULT190822]";
            }
            else
            {
                Application.Exit();
            }

            //admin only
            if (Userlog.UserRoles.Contains("ADMIN"))
            {
                masterDataToolStripMenuItem.Visible = true;
            }
            else
            {
                masterDataToolStripMenuItem.Visible = false;
            }

            //buying station
            if (Userlog.UserRoles.Contains("BUYING-REG") || Userlog.UserRoles.Contains("BUYING-BUY") || Userlog.UserRoles.Contains("BUYING-INV"))
            {
                buyingStationToolStripMenuItem.Visible = true;
            }
            else
            {
                buyingStationToolStripMenuItem.Visible = false;
            }

            if (Userlog.UserRoles.Contains("BUYING-REG"))
            {
                registrationToolStripMenuItem.Visible = true;
                buyingQCToolStripMenuItem.Visible = true;
            }
            else
            {
                registrationToolStripMenuItem.Visible = false;
                buyingQCToolStripMenuItem.Visible = false;
            }

            if (Userlog.UserRoles.Contains("BUYING-BUY"))
            {
                buyingProcessToolStripMenuItem.Visible = true;
            }
            else
            {
                buyingProcessToolStripMenuItem.Visible = false;
            }

            if (Userlog.UserRoles.Contains("BUYING-INV"))
            {
                purchaseInvoiceToolStripMenuItem.Visible = true;
            }
            else
            {
                purchaseInvoiceToolStripMenuItem.Visible = false;
            }

            //processing station
            if (Userlog.UserRoles.Contains("PROCESS"))
            {
                processingToolStripMenuItem.Visible = true;
            }
            else
            {
                processingToolStripMenuItem.Visible = false;
            }

            //Inventory station
            if (Userlog.UserRoles.Contains("INVENTORY") || Userlog.UserRoles.Contains("SHIPMENT"))
            {
                inventoryStockToolStripMenuItem.Visible = true;
            }
            else
            {
                inventoryStockToolStripMenuItem.Visible = false;
            }

            if (Userlog.UserRoles.Contains("SUPERVISOR"))
            {
                weightAdjustmentToolStripMenuItem.Visible = true;
                inventoryAdjustmentToolStripMenuItem.Visible = true;
            }
            else
            {
                inventoryAdjustmentToolStripMenuItem.Visible = false;
                weightAdjustmentToolStripMenuItem.Visible = false;
            }

            if (Userlog.UserRoles.Contains("INVENTORY"))
            {
                tobaccoStockToolStripMenuItem.Visible = true;
                toolStripSeparator5.Visible = true;
                inventoryImportToolStripMenuItem.Visible = true;

                //weightAdjustmentToolStripMenuItem.Visible = true;
                //inventoryUnpackToolStripMenuItem.Visible = true;
                //reworkPackToolStripMenuItem.Visible = true;
                toolStripSeparator6.Visible = true;
                dispatchOUTToolStripMenuItem.Visible = true;
                dispatchINToolStripMenuItem.Visible = true;
            }
            else
            {
                tobaccoStockToolStripMenuItem.Visible = false;
                toolStripSeparator5.Visible = false;
                inventoryImportToolStripMenuItem.Visible = false;
                //weightAdjustmentToolStripMenuItem.Visible = false;
                //inventoryUnpackToolStripMenuItem.Visible = false;
                //reworkPackToolStripMenuItem.Visible = false;
                toolStripSeparator6.Visible = false;
                dispatchOUTToolStripMenuItem.Visible = false;
                dispatchINToolStripMenuItem.Visible = false;
            }

            //Inventory shipment station
            if (Userlog.UserRoles.Contains("SHIPMENT"))
            {
                toolStripSeparator7.Visible = true;
                shipmentInfoToolStripMenuItem.Visible = true;
            }
            else
            {
                toolStripSeparator7.Visible = false;
                shipmentInfoToolStripMenuItem.Visible = false;
            }

            //setting station
            if (Userlog.UserRoles.Contains("SETTING"))
            {
                settingsToolStripMenuItem.Visible = true;
            }
            else
            {
                settingsToolStripMenuItem.Visible = false;
            }
        }//end do login

        private void resetAccess()
        {
            masterDataToolStripMenuItem.Visible = false;
            buyingStationToolStripMenuItem.Visible = false;
            processingToolStripMenuItem.Visible = false;
            inventoryStockToolStripMenuItem.Visible = false;
            settingsToolStripMenuItem.Visible = false;
        }

        private void resetPanel(UserControl userControl)
        {
            MainPanel.Controls.Clear();
            MainPanel.Controls.Add(userControl);
        }

        #region Access

        private void userProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.UserProfile MyProfile = new Forms.UserProfile
            {
                UserLog = Userlog,
                ConnectionString = ConnectionString
            };
            MyProfile.ShowDialog();

            if (MyProfile.PassChanged == true)
            {
                MainPanel.Controls.Clear();
                doLogin();
            }
        }//end profile

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            doLogin();
        }//end logout

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Ingin keluar dari aplikasi?", "Keluar Aplikasi", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
                Application.Exit();
            }
        }//end exit app

        #endregion Access

        #region Master Data

        private void userListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlUser = new Controls.ucUsers
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString
            };
            MainPanel.Controls.Add(ControlUser);
        }

        private void vendorListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlVendor = new Controls.ucVendor
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred
            };
            MainPanel.Controls.Add(ControlVendor);
        }

        private void warehouseListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlWarehouse = new Controls.ucWarehouse
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ClientID = ClientID
            };
            MainPanel.Controls.Add(ControlWarehouse);
        }

        private void itemAttributeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlItemAttribute = new Controls.ucItemAttribute
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred
            };
            MainPanel.Controls.Add(ControlItemAttribute);
        }

        private void tobaccoProcessListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlTobaccoProcess = new Controls.ucTobaccoProcess
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred
            };
            MainPanel.Controls.Add(ControlTobaccoProcess);
        }

        private void tobaccoGradeListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlTobaccoGrade = new Controls.ucTobaccoGrade
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred
            };
            MainPanel.Controls.Add(ControlTobaccoGrade);
        }

        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlInventory = new Controls.ucInventory
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred
            };
            MainPanel.Controls.Add(ControlInventory);
        }

        private void subItemSegmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlSubItem = new Controls.ucSubItem
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred
            };
            MainPanel.Controls.Add(ControlSubItem);
        }

        private void tobaccoPriceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlTobaccoPrice = new Controls.ucTobaccoPrice
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred
            };
            MainPanel.Controls.Add(ControlTobaccoPrice);
        }

        #endregion Master Data

        #region Settings

        private void userDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlUser = new Controls.ucUsers
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString
            };
            MainPanel.Controls.Add(ControlUser);
        }

        private void acumaticaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Forms.SetupAcumatica setupAcumatica = new Forms.SetupAcumatica
            {
                AcumaticaChanged = false,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ClientID = ClientID
            };
            setupAcumatica.ShowDialog();

            if (setupAcumatica.AcumaticaChanged == true)
            {
                AcumaticaCred = setupAcumatica.AcumaticaCred;
            }
        }

        private void scaleCommunicationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Forms.SetupScale setupScale = new Forms.SetupScale
            {
                ScaleChanged = false,
                ConnectionString = ConnectionString,
                ScaleCom = ScaleCom,
                ClientID = ClientID
            };
            setupScale.ShowDialog();

            if (setupScale.ScaleChanged == true)
            {
                ScaleCom = setupScale.ScaleCom;
            }
        }

        private void scaleCalibrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.ScaleCalibrationSetting scaleCalibration = new Forms.ScaleCalibrationSetting
            {
                ConnectionString = ConnectionString,
                Warehouse = Warehouse,
                Userlog = Userlog,
                ClientID = ClientID,
                ScaleCalibration = ScaleCalibration
            };
            scaleCalibration.ShowDialog();

            ScaleCalibration = scaleCalibration.ScaleCalibration;
        }

        private void numberingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.SetupNumbering setupNumbering = new Forms.SetupNumbering
            {
                ConnectionString = ConnectionString
            };
            setupNumbering.ShowDialog();
        }

        private void fiscalSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.SetupFiscal setupFiscal = new Forms.SetupFiscal
            {
                ConnectionString = ConnectionString,
                FiscalInfo = FiscalInfo,
                ClientID = ClientID
            };
            setupFiscal.ShowDialog();

            if (setupFiscal.FiscalChanged == true)
            {
                FiscalInfo = setupFiscal.FiscalInfo;
            }
        }

        #endregion Settings

        #region Buying

        private void registrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlBuyingRegistration = new Controls.ucBuyingRegistration
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                Userlog = Userlog
            };
            MainPanel.Controls.Add(ControlBuyingRegistration);
        }

        private void buyingProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlBuyingProcess = new Controls.ucBuyingProcess
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlBuyingProcess);
        }

        private void purchaseInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlPurchaseInvoice = new Controls.ucPurchaseInvoice
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlPurchaseInvoice);
        }

        private void buyingQCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlBuyingQC = new Controls.ucBuyingQC
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlBuyingQC);

        }

        #endregion Buying

        #region Process

        private void reclassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlReclassDirect = new Controls.ucReclassDirect
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlReclassDirect);
        }

        private void reclassProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlReclassProcess = new Controls.ucReclassProcess
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlReclassProcess);
        }

        private void birINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingINGeneric = new Controls.ucProcessingINGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                tempProcess = "BR",
                tempProcessDescr = "Bir-Bir",
                AcumaticaReasonCode = "ISSUEBIR",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingINGeneric);
        }

        private void birOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingOUTGeneric = new Controls.ucProcessingOUTGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                tempProcess = "BR",
                tempProcessDescr = "Bir-Bir",
                PriceScenario = 2,
                AcumaticaReasonCode = "RECEIPTBIR",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingOUTGeneric);
        }

        private void buttedINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingINGeneric = new Controls.ucProcessingINGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                tempProcess = "BT",
                tempProcessDescr = "Butting",
                AcumaticaReasonCode = "ISSUEBUTTED",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingINGeneric);
        }

        private void buttedOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingOUTGeneric = new Controls.ucProcessingOUTGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                tempProcess = "BT",
                tempProcessDescr = "Butting",
                PriceScenario = 1,
                AcumaticaReasonCode = "RECEIPTBUTTED",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingOUTGeneric);
        }

        private void strippedINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingINGeneric = new Controls.ucProcessingINGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                tempProcess = "ST",
                tempProcessDescr = "Stripping",
                AcumaticaReasonCode = "ISSUESTRIPPED",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingINGeneric);
        }

        private void strippedOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingOUTGeneric = new Controls.ucProcessingOUTGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                tempProcess = "ST",
                tempProcessDescr = "Stripping",
                PriceScenario = 1,
                AcumaticaReasonCode = "RECEIPTSTRIPPED",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingOUTGeneric);
        }

        private void loosingINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingINGeneric = new Controls.ucProcessingINGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                tempProcess = "LS",
                tempProcessDescr = "Loosing",
                AcumaticaReasonCode = "ISSUELOOSE",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingINGeneric);
        }

        private void loosingOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingOUTGeneric = new Controls.ucProcessingOUTGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                tempProcess = "LS",
                tempProcessDescr = "Loosing",
                PriceScenario = 1,
                AcumaticaReasonCode = "RECEIPTLOOSE",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingOUTGeneric);
        }

        private void pras1INToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingINGeneric = new Controls.ucProcessingINGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                tempProcess = "P1",
                tempProcessDescr = "Pras 1",
                AcumaticaReasonCode = "ISSUEPRAS1",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingINGeneric);
        }

        private void pras1OUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingOUTGeneric = new Controls.ucProcessingOUTGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                tempProcess = "P1",
                tempProcessDescr = "Pras 1",
                PriceScenario = 2,
                AcumaticaReasonCode = "RECEIPTPRAS1",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingOUTGeneric);
        }

        private void pras2INToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingINGeneric = new Controls.ucProcessingINGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                tempProcess = "P2",
                tempProcessDescr = "Pras 2",
                AcumaticaReasonCode = "ISSUEPRAS2",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingINGeneric);
        }

        private void pras2OUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingOUTGeneric = new Controls.ucProcessingOUTGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                tempProcess = "P2",
                tempProcessDescr = "Pras 2",
                PriceScenario = 2,
                AcumaticaReasonCode = "RECEIPTPRAS2",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingOUTGeneric);
        }

        private void sortasiINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingINGeneric = new Controls.ucProcessingINGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                tempProcess = "SO",
                tempProcessDescr = "Sortasi",
                AcumaticaReasonCode = "ISSUESORTASI",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingINGeneric);
        }

        private void sortasiOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingOUTGeneric = new Controls.ucProcessingOUTGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                tempProcess = "SO",
                tempProcessDescr = "Sortasi",
                PriceScenario = 2,
                AcumaticaReasonCode = "RECEIPTSORTASI",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingOUTGeneric);
        }

        private void finalSortasiINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingINGeneric = new Controls.ucProcessingINGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                tempProcess = "FN",
                tempProcessDescr = "Final Sortasi",
                AcumaticaReasonCode = "ISSUEFINALSORT",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingINGeneric);
        }

        private void finalSortasiOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingOUTGeneric = new Controls.ucProcessingOUTGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                tempProcess = "FN",
                tempProcessDescr = "Final Sortasi",
                PriceScenario = 2,
                AcumaticaReasonCode = "RECEIPTFINALSORT",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingOUTGeneric);
        }

        private void sortasiPackingINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingINGeneric = new Controls.ucProcessingINGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                tempProcess = "SP",
                tempProcessDescr = "Sortasi Packing",
                AcumaticaReasonCode = "ISSUESORTPACK",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingINGeneric);
        }

        private void sortasiPackingOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingOUTGeneric = new Controls.ucProcessingOUTGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                tempProcess = "SP",
                tempProcessDescr = "Sortasi Packing",
                PriceScenario = 2,
                AcumaticaReasonCode = "RECEIPTSORTPACK",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingOUTGeneric);
        }

        private void temporaryPackingINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingINGeneric = new Controls.ucProcessingINGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                tempProcess = "TP",
                tempProcessDescr = "Temporary Packing",
                AcumaticaReasonCode = "ISSUETEMPPACK",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingINGeneric);
        }

        private void temporaryPackingOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingOUTGeneric = new Controls.ucProcessingOUTGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                tempProcess = "TP",
                tempProcessDescr = "Temporary Packing",
                PriceScenario = 1,
                AcumaticaReasonCode = "RECEIPTTEMPPACK",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingOUTGeneric);
        }

        private void finalPackingINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingINGeneric = new Controls.ucProcessingINGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                tempProcess = "PC",
                tempProcessDescr = "Final Packing",
                AcumaticaReasonCode = "ISSUEFINPACK",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingINGeneric);
        }

        private void finalPackingOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingOUTGeneric = new Controls.ucProcessingOUTGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                tempProcess = "PC",
                tempProcessDescr = "Final Packing",
                PriceScenario = 1,
                AcumaticaReasonCode = "RECEIPTFINPACK",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingOUTGeneric);
        }

        private void fermentationINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingINGeneric = new Controls.ucProcessingINGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                tempProcess = "FM",
                tempProcessDescr = "Fermenting",
                AcumaticaReasonCode = "ISSUEFERMENT",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingINGeneric);
        }

        private void fermentationOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlProcessingOUTGeneric = new Controls.ucProcessingOUTGeneric
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                tempProcess = "FM",
                tempProcessDescr = "Fermenting",
                PriceScenario = 1,
                AcumaticaReasonCode = "RECEIPTFERMENT",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlProcessingOUTGeneric);
        }


        #endregion Process

        #region inventory

        private void tobaccoStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlStock = new Controls.ucInventoryStock
            {
                Warehouse = Warehouse,
                Userlog = Userlog,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred
            };
            MainPanel.Controls.Add(ControlStock);
        }
        private void stockReportToolStripMenuItem_Click(object sender, EventArgs e)
        {

            MainPanel.Controls.Clear();
            ControlStockReport = new Controls.ucInventoryStockReport
            {
                Warehouse = Warehouse,
                Userlog = Userlog,
                ConnectionString = ConnectionString
            };
            MainPanel.Controls.Add(ControlStockReport);
        }
        private void inventoryImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlInventoryImport = new Controls.ucInventoryImport
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                Userlog = Userlog,
                FiscalInfo = FiscalInfo,
                AcumaticaReasonCode = "RECEIPTIMPORT",
                ClientID = ClientID
            };
            MainPanel.Controls.Add(ControlInventoryImport);
        }

        private void weightAdjustmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlWeightAdjust = new Controls.ucWeightAdjust
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlWeightAdjust);
        }
        private void dispatchOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlDispatchOUT = new Controls.ucDispatchOUT
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                Userlog = Userlog,
                FiscalInfo = FiscalInfo,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration
            };
            MainPanel.Controls.Add(ControlDispatchOUT);
        }

        private void dispatchINToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlDispatchIN = new Controls.ucDispatchIN
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                Userlog = Userlog,
                FiscalInfo = FiscalInfo,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration
            };
            MainPanel.Controls.Add(ControlDispatchIN);
        }

        private void shipmentInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlShipmentInfo = new Controls.ucShipmentInfo
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                Userlog = Userlog,
                FiscalInfo = FiscalInfo,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration
            };
            MainPanel.Controls.Add(ControlShipmentInfo);
        }



        #endregion inventory

        private void directPackingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlDirectPacking = new Controls.ucDirectPacking
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlDirectPacking);
        }

        private void directTemporaryPackingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlDirectTempPacking = new Controls.ucDirectTempPacking
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlDirectTempPacking);
        }

        private void scaleCalibrationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Forms.ScaleCalibration2 scaleCalibration2 = new Forms.ScaleCalibration2
            {
                ConnectionString = ConnectionString,
                Warehouse = Warehouse,
                ScaleCom = ScaleCom,
                Userlog = Userlog,
                ClientID = ClientID,
                ScaleCalibration = ScaleCalibration
            };
            scaleCalibration2.ShowDialog();
        }

        private void scaleCalibration2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Forms.ScaleCalibration2 scaleCalibration2 = new Forms.ScaleCalibration2
            {
                ConnectionString = ConnectionString,
                Warehouse = Warehouse,
                ScaleCom = ScaleCom,
                Userlog = Userlog,
                ClientID = ClientID,
                ScaleCalibration = ScaleCalibration
            };
            scaleCalibration2.ShowDialog();
        }

        private void inventoryAdjustmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ControlInventoryAdjust = new Controls.ucInventoryAdjust
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ControlInventoryAdjust);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
                MainPanel.Controls.Clear();
                ucReturnProcessingINGeneric = new Controls.ucReturnProcessingINGeneric
                {
                    Warehouse = Warehouse,
                    ConnectionString = ConnectionString,
                    AcumaticaCred = AcumaticaCred,
                    ScaleCom = ScaleCom,
                    tempProcess = "RBY",
                    tempProcessDescr = "Return Buying",
                    AcumaticaReasonCode = "ISSUE",
                    Userlog = Userlog,
                    FiscalInfo = FiscalInfo
                };
                MainPanel.Controls.Add(ucReturnProcessingINGeneric);
        }

        private void buyingReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainPanel.Controls.Clear();
            ucReturnBuying = new Controls.ucReturnBuying
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                tempProcess = "RBY",
                tempProcessDescr = "Return Buying",
                AcumaticaReasonCode = "ISSUE",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            MainPanel.Controls.Add(ucReturnBuying);
        }
    }
}