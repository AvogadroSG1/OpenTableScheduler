using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;

using System.Text;
//using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using Peter.Google.Gmail;
using Peter.OpenTable.Models;
using System.Linq;

namespace Peter.OpenTable
{
    class Program
    {
        // static OpenTableRequest OpenTableRequest { get; set; }

        private static readonly HttpClient client = new HttpClient();
        private static readonly GmailSupport GmailSupport = new GmailSupport();
        private static OpenTableReservationRequest OpenTableReservationRequest { get; set; }

        private string OpenTableFindRequest = "https://www.opentable.com/widget/reservation/restaurant-search?query=fireston&pageSize=100";

        async static Task Main(string[] args)
        {
            setupHttpClient();
            await determineRunTimeArgs(args);
            await makeReservationRequest();
        }
        async static Task determineRunTimeArgs(string[] args)
        {
            if ((args?.Length ?? 0) != 3)
            {
                OpenTableReservationRequest = new OpenTableReservationRequest()
                {
                    restuarantID = 1005595,
                    covers = 2,
                    dateTime = new DateTime(2021, 07, 11, 19, 00, 00).ToString("yyyy-MM-ddTHH:mm")
                };
            }
            else
            {
                DateTime posDate;
                if (!DateTime.TryParse(args[2], out posDate))
                {
                    string[] dateTime = args[2].Split(" ");
                    int[] date = dateTime[0].Split("/").Select(d => Convert.ToInt32(d)).ToArray();
                    int[] time = dateTime[1].Split(":").Select(t => Convert.ToInt32(t)).ToArray();
                    posDate = new DateTime(date[0], date[1], date[2], time[0], time[1], time[2]);
                }

                Console.WriteLine("Search for a restaruant:");
                string toFind = Console.ReadLine();

                int rid = await makeRestaurantFindRequest(toFind);

                OpenTableReservationRequest = new OpenTableReservationRequest()
                {
                    restuarantID = rid,
                    covers = Convert.ToInt32(args[1]),
                    dateTime = posDate.ToString("yyyy-MM-ddTHH:mm")
                };
            }
        }

        static void setupHttpClient()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("*/*"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
        }

        async static Task makeReservationRequest()
        {
            string formattedRequest = JsonSerializer.Serialize(OpenTableReservationRequest);

            HttpResponseMessage response = await client.PostAsync(
                $"https://www.opentable.com/restaurant/profile/{OpenTableReservationRequest.restuarantID}/search",
                new StringContent(
                    formattedRequest,
                    Encoding.UTF8,
                    "application/json"));

            var postResponse = await response.Content.ReadAsStringAsync();
            postResponse = fixKnownIssues(postResponse);
            var reservationJson = JsonSerializer.Deserialize<OpenTableAPIResponseBase>(postResponse);

            string message = string.Empty;
            List<Time> times = reservationJson.Availability?.Times ?? reservationJson?.SameDayAvailability?.Times ?? new List<Time>();
            string sameDayTime = string.Join(", ", times?.Select(t => $"{t.DateTime.Date.ToShortDateString()}: {t.TimeString}")) ?? "None";
            var nextTimeObject = reservationJson?.MultiDaysAvailability?.Timeslots?.FirstOrDefault();
            string nextTime = nextTimeObject != null ?
            $"{nextTimeObject?.Date} @ {string.Join(",", nextTimeObject.Times.Select(t => t.TimeString)) ?? string.Empty}"
            : "No other times listed.";

            message = $"Same Day Times: {sameDayTime}\r\n\r\nNext Available Time: {nextTime}\r\n\r\nI'll check again in a little bit and let you know my findings.";
            GmailSupport.SetMessage("kendralaster34@gmail.com", "Open Table Request Information", message);
            await GmailSupport.SendMail();

            Console.WriteLine(message);
        }

        async static Task<int> makeRestaurantFindRequest(string search)
        {
            OpenTableRestaurantQueryRequest req = new OpenTableRestaurantQueryRequest()
            {
                query = search,
                pageSize = 10
            };
            HttpResponseMessage response = await client.GetAsync(req.ToString());
            string stringResponse = await response.Content.ReadAsStringAsync();
            OpenTableRestaurantQueryResponse results = JsonSerializer.Deserialize<OpenTableRestaurantQueryResponse>(stringResponse);

            Console.WriteLine("These are the resturaunt matching your name:");
            int index = 1;

            results.Items.ForEach(i =>
            {
                Console.WriteLine($"{index++}:  Restaurant: {i.Name} of {i.AddressResponse.City}, {i.AddressResponse.Province} ({i.Rid})");
            });

            Console.WriteLine("Which of the above is the resturaunt you're interested in?");
            int choice = Convert.ToInt32(Console.ReadLine());

            return results.Items.ElementAt(choice - 1).Rid;

        }


        private static string fixKnownIssues(string jsonString)
        {
            Dictionary<string, string> issues = new Dictionary<string, string>();
            issues.Add("\"noAvailabilityRestaurants\":\"}", "\"noAvailabilityRestaurants\":null}");
            issues.Add("\"noAvailabilityRestaurants\":\"\"}", "\"noAvailabilityRestaurants\":null}");
            issues.Add("\"noAvailabilityRestaurants\":\"\"", "\"noAvailabilityRestaurants\":null");

            issues.ToList().ForEach(pair =>
            {
                jsonString = jsonString.Replace(pair.Key, pair.Value);
            }
            );

            return jsonString;
        }
    }



}

// if (reservationDiv.FirstChild.InnerHtml.Contains("there’s no online availability", StringComparison.InvariantCultureIgnoreCase))
// {
//     Console.WriteLine("No Openings");
//     var nextOpening = reservationDiv
//         .ChildNodes.FindFirst("div")
//         .ChildNodes.FindFirst("ul")
//         .ChildNodes.FindFirst("li")
//         .ChildNodes.FindFirst("span")
//         .ChildNodes.FindFirst("div");
//     Console.WriteLine($"Next Opening: {nextOpening.InnerHtml}");
// }
// else
// {
//     Console.WriteLine("Opening");
// }