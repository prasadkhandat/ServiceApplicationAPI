using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogLibrary
{
    public class FileLogger
    {
        public static void AppendLog(string LogName, string Functionname, string Message)
        {
            try
            {
                string MasterCustID = "LOG";
                string FileName = "";

                FileName = MasterCustID + "_" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".log";

                //Environment.CurrentDirectory = @"E:\SendNowTextAlerts";
                if (!Directory.Exists(Environment.CurrentDirectory + @"\Log"))
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + @"\Log");
                }

                if (LogName.ToLower().Equals("access"))
                {
                    if (!Directory.Exists(Environment.CurrentDirectory + @"\Log\Access"))
                    {
                        Directory.CreateDirectory(Environment.CurrentDirectory + @"\Log\Access");
                    }

                    //if (!File.Exists(Environment.CurrentDirectory + @"\Log\Access\" + MasterCustID + ".log"))
                    if (!File.Exists(Environment.CurrentDirectory + @"\Log\Access\" + FileName))
                    {
                        //FileStream fs = File.Create(Environment.CurrentDirectory + @"\Log\Access\" + MasterCustID + ".log");
                        FileStream fs = File.Create(Environment.CurrentDirectory + @"\Log\Access\" + FileName);
                        fs.Close();
                    }
                    if (MasterCustID.Equals("DNULDate"))
                    {
                        //StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + @"\" + MasterCustID + ".log");
                        StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + @"\" + FileName);
                        sw.Write(Message);
                        sw.Close();

                    }
                    else
                    {
                        //StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + @"\Log\Access\" + MasterCustID + ".log", true);
                        StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + @"\Log\Access\" + FileName, true);
                        sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + Functionname + " " + Message, true);
                        sw.Close();
                    }
                }
                else
                {
                    if (!Directory.Exists(Environment.CurrentDirectory + @"\Log\Error"))
                    {
                        Directory.CreateDirectory(Environment.CurrentDirectory + @"\Log\Error");
                    }

                    if (!File.Exists(Environment.CurrentDirectory + @"\Log\Error\" + MasterCustID + ".log"))
                    {
                        //FileStream fs = File.Create(Environment.CurrentDirectory + @"\Log\Error\" + MasterCustID + ".log");
                        FileStream fs = File.Create(Environment.CurrentDirectory + @"\Log\Error\" + FileName);
                        fs.Close();
                    }

                    //StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + @"\Log\Error\" + MasterCustID + ".log", true);
                    StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + @"\Log\Error\" + FileName, true);
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + Functionname + " " + Message);
                    sw.Close();

                }
            }
            catch (Exception ex)
            {

                Console.Write(ex.Message);
            }
        }
    }
}
