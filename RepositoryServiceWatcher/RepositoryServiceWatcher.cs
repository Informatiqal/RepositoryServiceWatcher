using System;
using System.ServiceProcess;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Text;
using System.Diagnostics;

namespace RepositoryServiceWatcher
{
    public partial class RepositoryServiceWatcher : ServiceBase
    {
        public RepositoryServiceWatcher()
        {
            InitializeComponent();  
        }

        protected override void OnStart(string[] args) {
            string url = System.Configuration.ConfigurationManager.AppSettings["url"];
            string serviceName = System.Configuration.ConfigurationManager.AppSettings["serviceName"];
            if(serviceName == null)
            {
                serviceName = "QlikSenseRepositoryService";
            }

            if (url == null)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry("Key \"url\" is missing from the config file", EventLogEntryType.Error, 101, 1);
                }
                Environment.Exit(1);
            }


            ExtendedServiceController xServiceController = new ExtendedServiceController(serviceName);
            xServiceController.StatusChanged += (s,e) => xServiceController_StatusChanged(s, e, url);
        }

        private static void xServiceController_StatusChanged(object sender, ServiceStatusEventArgs e, string url)
        {
            var eventLog = new System.Diagnostics.EventLog();


            if (e.Status == ServiceControllerStatus.Running || e.Status == ServiceControllerStatus.Stopped)
            {

                var handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                string json = new JavaScriptSerializer().Serialize(new
                {
                    status = e.Status.ToString()
                });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient(handler);

                try
                {
                    HttpResponseMessage res = client.PostAsync(url, content).Result;
                }
                catch (HttpRequestException ex) {
                    using (EventLog eventLog1 = new EventLog("Application"))
                    {
                        eventLog1.Source = "Application";
                        eventLog1.WriteEntry(ex?.InnerException.Message ?? ex.Message, EventLogEntryType.Warning, 101, 1);
                    }
                }
            }
        }

        protected override void OnStop()
        {
        }
    }
}
