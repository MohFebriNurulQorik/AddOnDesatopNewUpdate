using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.IO.Ports;

namespace ScaleAddon
{
    public class ScaleCalibrationModel
    {
        public string DocumentID { get; set; }
        public string DocumentDate { get; set; }
        public string WarehouseID { get; set; }
        public string ClientID { get; set; }
        public string CreatorID { get; set; }
        public string ConnectionString = $"Data Source= {Properties.Settings.Default.DBServer}; Initial Catalog={Properties.Settings.Default.DBScheme};user={Properties.Settings.Default.DBUser}; Password={Properties.Settings.Default.DBPassword}";

        public bool isActive()
        {
            DateTime docDate = DateTime.Parse(DocumentDate);
            DateTime curDate = DateTime.Now;
            DateTime curDatelatter = DateTime.Now.AddDays(-1);
            if (DocumentID == "")
            {
                return false;
            }
            else
            {

                var curDate1 = DateTime.Parse(curDate.ToString("yyyy MM dd 22:00:00"));
                var curDate2 = DateTime.Parse(curDate.ToString("yyyy MM dd 11:00:00"));
                var curDate3 = DateTime.Parse(curDatelatter.ToString("yyyy MM dd 21:59:59"));
                var curDateNow = DateTime.Parse(curDate.ToString("yyyy MM dd HH:mm:ss"));
                var docDate1 = DateTime.Parse(docDate.ToString("yyyy MM dd HH:mm:ss"));

                Console.WriteLine(curDate.ToString("yyyy MM dd 14:00:00"));
                Console.WriteLine(docDate.ToString("yyyy MM dd HH:mm:ss"));
                TimeSpan duration = curDate.Subtract(docDate);

                
                if (docDate1 < curDate1 && docDate1 > curDate2)
                {
                 
                    if (curDateNow < curDate1)
                    {
                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        {

                            connection.Open();
                            string query = $@"SELECT
                                            count(id) as a
                                        FROM
	                                        ScaleCalibrationDetail
                                

                                WHERE DocumentID='{DocumentID}'
                                        ";
                            SqlCommand command = new SqlCommand(query, connection);


                            Int32 count = (Int32)command.ExecuteScalar();

                            if (count>=9) {
                                return true;
                            }
                            else {
                                return false;
                            }

                            connection.Close();
                        }

                       
                    }
                    else
                    {
                       
                        return false;
                    }
                }
                else if (docDate1 < curDate2 && docDate1 > curDate3 )
                {
                 
                    if (curDateNow < curDate2)
                    {

                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        {

                            connection.Open();
                            string query = $@"SELECT
                                            count(id) as a
                                        FROM
	                                        ScaleCalibrationDetail
                                        WHERE DocumentID='{DocumentID}'
                                        ";
                            SqlCommand command = new SqlCommand(query, connection);


                            Int32 count = (Int32)command.ExecuteScalar();

                            if (count >= 9)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                            connection.Close();
                        }
                    }
                    else
                    {
                       
                        return false;
                    }

                }
                else {
                   
                    return false;
                }
            }
       
        }

        public void ClearCalibration()
        {
            DocumentID = "";
            DocumentDate = "";
            WarehouseID = "";
            ClientID = "";
            CreatorID = "";
        }
    }
}