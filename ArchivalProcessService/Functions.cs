
namespace ArchivalProcessService
{

    #region References

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Azure.WebJobs;

    #endregion

    public class Functions
    {
        static string ApiBaseAddress;
        static string TenantCode;

        // This function will get triggered/executed every day (daily) at 00.00.
        public static void ProcessQueueBasedOnTimer([TimerTrigger("0 0 0 * * *", RunOnStartup = false)] TimerInfo timer)
        {
            WriteToFile("This should run every day (daily) at 00.00");
            ApiBaseAddress = ConfigurationManager.AppSettings["ApiBaseAddress"];
            TenantCode = ConfigurationManager.AppSettings["TenantCode"];
            WriteToFile("App Base Address: " + ApiBaseAddress + " TenantCode: " + TenantCode);
            RunArchivalProcessSchedule();
            WriteToFile("Execution done..!! ");
        }

        public static void RunArchivalProcessSchedule()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ApiBaseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("TenantCode", TenantCode);

            try
            {
                var response = client.PostAsync("ArchivalProcess/RunArchivalProcess", null).Result;
                Console.WriteLine("Response from Run Archival Process API: " + response);
                WriteToFile("Response from Run Archival Process API: " + response);
                WriteToFile("Response Content from Run Archival Process API: " + response.Content);
            }
            catch (Exception ex)
            {
                WriteToFile("Exception at Run Archival Process Schedule: " + ex);
                throw ex;
            }
        }

        public static void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ArchiveLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
