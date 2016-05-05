using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using _470_Final_AGAIN.Models;
using System.Web.Script.Serialization;
using System.Web.Helpers;

namespace _470_Final_AGAIN.Controllers
{
    

    public class ApiController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> Ingredients
        (
            string Ingredient1,
            string Ingredient2,
            string Ingredient3,
            string Ingredient4,
            string Ingredient5)
        {
            //For 1 and 2 and 3
            TempData["dataToSend"] = await RunAsync(Ingredient1, Ingredient2, Ingredient3);
            //For 3 and 4 and 5
            TempData["dataToSend2nd"] = await RunAsync(Ingredient3, Ingredient4, Ingredient5);

            return RedirectToAction("GetRecipeFromApiController", "TrainingDatas"); 
        }

        static async Task<JsonDataStoreModel> RunAsync(string Ingredient1,string Ingredient2, string Ingredient3)
        {
            using (var client = new HttpClient())
            {
                string url = "api.yummly.com/v1/api/recipes";
                var id = 74924307;
                var key = "20b213df842611544a17ee8d2a08c10b";

                var uriBuilder = new UriBuilder(url);
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["_app_id"] = id.ToString();
                query["_app_key"] = key;

                // Intersection of 1 and 2
                query["q"] = Ingredient1;
                query["q"] = Ingredient2;
                query["q"] = Ingredient3;
                


                //string url2 = url;
                //var uriBuilder2 = new UriBuilder(url);
                //var query2 = HttpUtility.ParseQueryString(uriBuilder.Query);
                ////intersection of 3 & 4 & 5
                //query2["_app_id"] = id.ToString();
                //query2["_app_key"] = key;

                //query2["q"] = Ingredient3;
                //query2["q"] = Ingredient4;
                //query2["q"] = Ingredient5;

                uriBuilder.Query = query.ToString();
                //uriBuilder2.Query = query2.ToString();

                url = uriBuilder.ToString();
                //url2 = uriBuilder2.ToString();

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                
                HttpResponseMessage response =  client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();

                    var res = JsonConvert.SerializeObject(data);
                    var jsonResult= JsonConvert.DeserializeObject<JsonDataStoreModel>(data);

                    return jsonResult;
                }
                else
                {
                    return new JsonDataStoreModel();//placeholder
                }


            }
        }
        
    
    }

    //public class Ref<T>
    //{
    //    public Ref() { }
    //    public Ref(T value) { Value = value; }
    //    public T Value { get; set; }
    //    public override string ToString()
    //    {
    //        T value = Value;
    //        return value == null ? "" : value.ToString();
    //    }
    //    public static implicit operator T(Ref<T> r) { return r.Value; }
    //    public static implicit operator Ref<T>(T value) { return new Ref<T>(value); }
    //}
}
