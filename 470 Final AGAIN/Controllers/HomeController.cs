using _470_Final_AGAIN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _470_Final_AGAIN.Controllers
{
    public class HomeController : Controller
    {

        private TrainingDBContext db = new TrainingDBContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            
                //ViewBag.Users = db.Users.ToList();
                //ViewBag.TrainingDatas = db.TrainingDatas.ToList();


                return View();
        
        }
       

       

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}