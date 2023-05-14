using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Net.Mail;
using TaxiManagementSystem.Web.Models;

namespace TaxiManagementSystem.Web.Helpers
{
    public static class Utility
    {
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }

            return dt;
        }

        public static DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {
                oledbConn.Open();
                using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$] WHERE NOT ([CITY Name] = '')", oledbConn))
                {
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    oleda.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    oleda.Fill(ds);

                    dt = ds.Tables[0];
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {

                oledbConn.Close();
            }

            return dt;
        }

        public static void SendEmail(ContactViewModel model)
        {
            using (MailMessage mail = new MailMessage())
            {
                using (SmtpClient client = new SmtpClient())
                {
                    mail.From = new MailAddress(model.Email);
                    mail.To.Add("noreply.taximgmt@gmail.com");
                    mail.To.Add("savio.abdo@gmail.com");
                    mail.ReplyToList.Add(model.Email);
                    mail.IsBodyHtml = true;
                    mail.Subject = "Message from Taxi Management System";
                    string body = "Sent by: <strong>" + model.Name + "</strong><br/>";
                    body += "Email: <strong>" + model.Email + "</strong><br/>";
                    body += "<br/><hr/><br/>";
                    body += "Message: <br/>";
                    body += model.Message;
                    body += "<br/><hr/>";
                    mail.Body = body;
                    client.Send(mail);
                }
            }
        }
    }
}