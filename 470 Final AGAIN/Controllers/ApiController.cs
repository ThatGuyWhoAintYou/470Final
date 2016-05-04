using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace _470_Final_AGAIN.Controllers
{

    public class ApiController : Controller
    {
        [HttpPost]
        public ActionResult Ingredients
        (
            string Ingredient1,
            string Ingredient2,
            string Ingredient3,
            string Ingredient4,
            string Ingredient5)
        {
            RunAsync().Wait();
            return RedirectToAction("RunAsync", "Api"); // change to recipe controller to consume recipes returned from api
        }

        static async Task RunAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.yummly.com/v1/");

                //string url = "api/recipes";
                //var id = 74924307;
                //var key = "20b213df842611544a17ee8d2a08c10b";

                //var uriBuilder = new UriBuilder(url);
                //var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                //query["_app_id"] = id.ToString();
                //query["_app_key"] = key;
                //query["q"] = "salt";
                //uriBuilder.Query = query.ToString();

                //url = uriBuilder.ToString();
                ////"http://somesite.com:80/news.php?article=1&lang=en&action=login1&attempts=11"

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET


                HttpResponseMessage response = await client.GetAsync("api/recipes?_app_id=74924307&_app_key=20b213df842611544a17ee8d2a08c10b&&q=salt");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Success");
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject(data);
                }


            }
        }
    }
}

