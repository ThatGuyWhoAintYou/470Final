using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _470_Final_AGAIN.Models
{
    public class JsonDataStoreModel
    {
        //public JsonArray criteria { get; set; }
        //public JsonArray attribution { get; set; }
        public JsonArray matches { get; set; }
        public int facetCount { get; set; }
        public int totalMatchCount { get; set; }
     
    }
}