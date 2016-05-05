using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _470_Final_AGAIN.Models;
using Accord.Statistics.Filters;
using Accord.MachineLearning.DecisionTrees;
using Accord.Math;
using Accord.MachineLearning.DecisionTrees.Learning;
using Accord;
using RestSharp;
using RestSharp.Serializers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace _470_Final_AGAIN.Controllers
{
    public class TrainingDatasController : Controller
    {
        DataTable data = new DataTable("Mitchell's Tennis Example");
        Codification codebook;
        DecisionTree tree;

        private TrainingDBContext db = new TrainingDBContext();

        // GET: TrainingDatas
        public ActionResult Index()
        {
            
            return View(db.TrainingDatas.ToList());
        }

        // GET: TrainingDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainingData trainingData = db.TrainingDatas.Find(id);
            if (trainingData == null)
            {
                return HttpNotFound();
            }
            return View(trainingData);
        }

        // GET: TrainingDatas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrainingDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Salty,Sour,Sweet,Bitter,Meaty,Piquant,Rating,PrepTime,ShowUser")] TrainingData trainingData)
        {
            if (ModelState.IsValid)
            {
                db.TrainingDatas.Add(trainingData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trainingData);
        }

        // GET: TrainingDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainingData trainingData = db.TrainingDatas.Find(id);
            if (trainingData == null)
            {
                return HttpNotFound();
            }
            return View(trainingData);
        }

        // POST: TrainingDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Salty,Sour,Sweet,Bitter,Meaty,Piquant,Rating,PrepTime,ShowUser")] TrainingData trainingData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trainingData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trainingData);
        }

        // GET: TrainingDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TrainingData trainingData = db.TrainingDatas.Find(id);
            if (trainingData == null)
            {
                return HttpNotFound();
            }
            return View(trainingData);
        }

        // POST: TrainingDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrainingData trainingData = db.TrainingDatas.Find(id);
            db.TrainingDatas.Remove(trainingData);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public void Train(TrainingData toTest)
        {
            data.Columns.Add("ID","Salty","Sour","Sweet","Bitter","Meaty","Piquant","Rating","PrepTime","ShowUser");

            List<TrainingData> t = db.TrainingDatas.ToList();
            for(int i = 0; i < t.Count; i++)
            {
                data.Rows.Add(t.ElementAt(i).ID, t.ElementAt(i).Salty, t.ElementAt(i).Sour, t.ElementAt(i).Sweet, t.ElementAt(i).Bitter, t.ElementAt(i).Meaty, t.ElementAt(i).Piquant, t.ElementAt(i).Rating.ToString(), t.ElementAt(i).PrepTime, t.ElementAt(i).ShowUser);
            }


            codebook = new Codification(data, "Salty", "Sour", "Sweet", "Bitter", "Meaty", "Piquant", "Rating", "PrepTime","ShowUser");

            // Translate our training data into integer symbols using our codebook:
            DataTable symbols = codebook.Apply(data);
            int[][] inputs = symbols.ToArray<int>("Salty", "Sour", "Sweet", "Bitter", "Meaty", "Piquant", "Rating", "PrepTime");
            int[] outputs = symbols.ToArray<int>("ShowUser");

            DecisionVariable[] attributes =
            {
              new DecisionVariable("Salty",     3),
              new DecisionVariable("Sour",     3),
              new DecisionVariable("Sweet",     3),
              new DecisionVariable("Bitter",     3),
              new DecisionVariable("Meaty",     3),
              new DecisionVariable("Piquant",     3),
              new DecisionVariable("Rating",     5),
              new DecisionVariable("PrepTime",     3)
            };

            int classCount = 2; // 2 possible output values for playing tennis: yes or no

            //Create the decision tree using the attributes and classes
            tree = new DecisionTree(attributes, classCount);
            // Create a new instance of the ID3 algorithm
            ID3Learning id3learning = new ID3Learning(tree);
             id3learning.Run(inputs, outputs);


            String answer=  codebook.Translate("ShowUser", tree.Compute(codebook.Translate(toTest.Salty, toTest.Sour, toTest.Sweet, toTest.Bitter, toTest.Meaty, toTest.Piquant, toTest.Rating, toTest.PrepTime)));
            toTest.ShowUser = answer;
            //ViewBag.Salty = answer;
                  
        }

        // GET: Recipe
        public JsonResult GetRecipeFromApiController()
        {

            var fromApi = TempData["dataToSend"] as JsonDataStoreModel; // for 1 & 2 & 3
            var fromApi2 = TempData["dataToSend2nd"] as JsonDataStoreModel; // for 3 & 4 & 5

            
            List<TrainingData> testSet = new List<TrainingData>();

           

            foreach(JObject item in fromApi.matches)
            {
                
                    try {

                    TrainingData jj = new TrainingData();
                    jj.Name =  item.SelectToken("id").ToString();
                    var gg = item.SelectToken("flavors");

                    
                   if(Double.Parse(gg.SelectToken("salty").ToString()) < .4)
                        {
                        jj.Salty = "No";
                        }
                    else if (Double.Parse(gg.SelectToken("salty").ToString()) < .6)
                    {
                        jj.Salty = "kinda";
                    }
                    else
                    {
                        jj.Salty = "very";
                    }

                    if (Double.Parse(gg.SelectToken("sour").ToString()) < .4)
                    {
                        jj.Sour = "No";
                    }
                    else if (Double.Parse(gg.SelectToken("sour").ToString()) < .6)
                    {
                        jj.Sour = "kinda";
                    }
                    else
                    {
                        jj.Sour = "very";
                    }

                    if (Double.Parse(gg.SelectToken("sweet").ToString()) < .4)
                    {
                        jj.Sweet = "No";
                    }
                    else if (Double.Parse(gg.SelectToken("sweet").ToString()) < .6)
                    {
                        jj.Sweet = "kinda";
                    }
                    else
                    {
                        jj.Sweet = "very";
                    }
                    if (Double.Parse(gg.SelectToken("bitter").ToString()) < .4)
                    {
                        jj.Bitter = "No";
                    }
                    else if (Double.Parse(gg.SelectToken("bitter").ToString()) < .6)
                    {
                        jj.Bitter = "kinda";
                    }
                    else
                    {
                        jj.Bitter = "very";
                    }

                    if (Double.Parse(gg.SelectToken("meaty").ToString()) < .4)
                    {
                        jj.Meaty = "No";
                    }
                    else if (Double.Parse(gg.SelectToken("meaty").ToString()) < .6)
                    {
                        jj.Meaty = "kinda";
                    }
                    else
                    {
                        jj.Meaty= "very";
                    }


                    if (Double.Parse(gg.SelectToken("piquant").ToString()) < .4)
                    {
                        jj.Piquant = "No";
                    }
                    else if (Double.Parse(gg.SelectToken("piquant").ToString()) < .6)
                    {
                        jj.Piquant = "kinda";
                    }
                    else
                    {
                        jj.Piquant = "very";
                    }
                    jj.Rating = Int32.Parse( item.SelectToken("rating").ToString()).ToString();


                    if (Double.Parse(item.SelectToken("totalTimeInSeconds").ToString()) < 1200)
                    {
                        jj.PrepTime = "Short";
                    }
                    else if (Double.Parse(item.SelectToken("totalTimeInSeconds").ToString()) < 2400)
                    {
                        jj.PrepTime = "Medium";
                    }
                    else
                    {
                        jj.PrepTime = "Long";
                    }
                    

                    testSet.Add(jj);









                } catch (Exception e)
                    {
                        //oops, you dun goofed
                    }
                
            }
            foreach (JObject item in fromApi2.matches)
            {

                try
                {

                    TrainingData jj = new TrainingData();
                    jj.Name = item.SelectToken("id").ToString();
                    var gg = item.SelectToken("flavors");


                    if (Double.Parse(gg.SelectToken("salty").ToString()) < .4)
                    {
                        jj.Salty = "No";
                    }
                    else if (Double.Parse(gg.SelectToken("salty").ToString()) < .6)
                    {
                        jj.Salty = "kinda";
                    }
                    else
                    {
                        jj.Salty = "very";
                    }

                    if (Double.Parse(gg.SelectToken("sour").ToString()) < .4)
                    {
                        jj.Sour = "No";
                    }
                    else if (Double.Parse(gg.SelectToken("sour").ToString()) < .6)
                    {
                        jj.Sour = "kinda";
                    }
                    else
                    {
                        jj.Sour = "very";
                    }

                    if (Double.Parse(gg.SelectToken("sweet").ToString()) < .4)
                    {
                        jj.Sweet = "No";
                    }
                    else if (Double.Parse(gg.SelectToken("sweet").ToString()) < .6)
                    {
                        jj.Sweet = "kinda";
                    }
                    else
                    {
                        jj.Sweet = "very";
                    }
                    if (Double.Parse(gg.SelectToken("bitter").ToString()) < .4)
                    {
                        jj.Bitter = "No";
                    }
                    else if (Double.Parse(gg.SelectToken("bitter").ToString()) < .6)
                    {
                        jj.Bitter = "kinda";
                    }
                    else
                    {
                        jj.Bitter = "very";
                    }

                    if (Double.Parse(gg.SelectToken("meaty").ToString()) < .4)
                    {
                        jj.Meaty = "No";
                    }
                    else if (Double.Parse(gg.SelectToken("meaty").ToString()) < .6)
                    {
                        jj.Meaty = "kinda";
                    }
                    else
                    {
                        jj.Meaty = "very";
                    }


                    if (Double.Parse(gg.SelectToken("piquant").ToString()) < .4)
                    {
                        jj.Piquant = "No";
                    }
                    else if (Double.Parse(gg.SelectToken("piquant").ToString()) < .6)
                    {
                        jj.Piquant = "kinda";
                    }
                    else
                    {
                        jj.Piquant = "very";
                    }
                    jj.Rating = Int32.Parse(item.SelectToken("rating").ToString()).ToString();


                    if (Double.Parse(item.SelectToken("totalTimeInSeconds").ToString()) < 1200)
                    {
                        jj.PrepTime = "Short";
                    }
                    else if (Double.Parse(item.SelectToken("totalTimeInSeconds").ToString()) < 2400)
                    {
                        jj.PrepTime = "Medium";
                    }
                    else
                    {
                        jj.PrepTime = "Long";
                    }


                    testSet.Add(jj);









                }
                catch (Exception e)
                {
                    //oops, you dun goofed
                }

            }

            foreach (TrainingData test in testSet)
            {
                Train(test);
            }
            // REPLACE the code below and make use of data from api. It has an array of 10 recipe matches, total count and others.
            return Json(fromApi, JsonRequestBehavior.AllowGet);
        }
    }
}
