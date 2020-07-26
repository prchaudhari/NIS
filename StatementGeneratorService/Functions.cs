

namespace StatementGeneratorService
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

        // This function will get triggered/executed immediately on startup, and then every 30 minutes thereafter.
        public static void ProcessQueueBasedOnTimer([TimerTrigger("0 */30 * * * *", RunOnStartup = true)] TimerInfo timer)
        {
            Console.WriteLine("This should run every 30 minutes");
            ApiBaseAddress = ConfigurationManager.AppSettings["ApiBaseAddress"];
            RunStatementGenerationSchedule();
        }

        public static void RunStatementGenerationSchedule()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ApiBaseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("TenantCode", "00000000-0000-0000-0000-000000000000");

            try
            {
                var response = client.PostAsync("Schedule/RunSchedule", null).Result;
                WriteToFile("Response from RunSchedule: " + response);
            }
            catch (Exception ex)
            {
                WriteToFile("Exception at RunSchedule: " + ex);
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
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
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