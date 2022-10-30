using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using log4net;

namespace WS.RdvPref
{
   static class Prefecture
    {
        private static readonly ILog log = LogManager.GetLogger("Prefecture");

        public static async Task CheckPremierTitre()
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = "https://www.hauts-de-seine.gouv.fr/ezjscore/call/bookingserver::planning::assign::20682::20687::21";

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

                using (HttpResponseMessage Response = await client.PostAsync(endpoint, new StringContent("")))
                {
                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        var customerJsonString = await Response.Content.ReadAsStringAsync();

                        var freq = Regex.Matches(customerJsonString, ">libre<").Count;

                        if (freq == 1)
                        {
                            log.Info("Pas de RDV");
                        }

                        if (freq > 1)
                        {
                            SendEmail("RDV Premier Titre Disponible");
                            SendEmail("RDV Renouvellement Disponible - mail envoyé");

                        }
                    }
                    else if (Response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        log.Warn("---Forbidden---");
                    }
                }
            }
        }
        //

        public static async Task CheckRenouvellement1()
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = "https://www.hauts-de-seine.gouv.fr/ezjscore/call/bookingserver::planning::assign::20678::20687::21";

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

                using (HttpResponseMessage Response = await client.PostAsync(endpoint, new StringContent("")))
                {
                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        var customerJsonString = await Response.Content.ReadAsStringAsync();
                        Console.WriteLine("Your response data is: " + customerJsonString);
                        var freq = Regex.Matches(customerJsonString, ">libre<").Count;

                        if (freq == 1)
                        {
                            log.Info("Pas de RDV");
                        }

                        if (freq > 1)
                        {
                            SendEmail("RDV Renouvellement 20678::20687");
                            log.Info("RDV Renouvellement Disponible");
                        }
                    }
                    else if (Response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        log.Warn("---Forbidden---");
                    }
                }
            }
        }

        public static async Task CheckRenouvellement2()
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = "https://www.hauts-de-seine.gouv.fr/ezjscore/call/bookingserver::planning::assign::20678::20683::7";

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));

                using (HttpResponseMessage Response = await client.PostAsync(endpoint, new StringContent("")))
                {
                    if (Response.StatusCode == HttpStatusCode.OK)
                    {
                        var customerJsonString = await Response.Content.ReadAsStringAsync();
                        Console.WriteLine("Your response data is: " + customerJsonString);
                        var freq = Regex.Matches(customerJsonString, ">libre<").Count;

                        if (freq == 1)
                        {
                            log.Info("Pas de RDV");
                        }

                        if (freq > 1)
                        {
                            SendEmail("RDV REMISE (RETRAIT) dispobible");
                            log.Info("RDV REMISE (RETRAIT) dispobible");
                        }
                    }
                    else if (Response.StatusCode == HttpStatusCode.Forbidden)
                    {
                        log.Warn("---Forbidden---");
                    }
                }
            }
        }

        public static void SendEmail(string sujet)
        {
            var client = new SmtpClient("smtp.office365.com", 587)
            {
                UseDefaultCredentials = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential("email", "password"),
                EnableSsl = true
            };
            client.Send("emailfrom", "emailto", sujet, sujet);

        }
    }
}
