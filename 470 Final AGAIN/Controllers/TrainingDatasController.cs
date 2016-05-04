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
            Train();
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


        public void Train()
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
              new DecisionVariable("Rating",     3),
              new DecisionVariable("PrepTime",     3)
            };

            int classCount = 2; // 2 possible output values for playing tennis: yes or no

            //Create the decision tree using the attributes and classes
            tree = new DecisionTree(attributes, classCount);
            // Create a new instance of the ID3 algorithm
            ID3Learning id3learning = new ID3Learning(tree);

            // Learn the training instances!
            ViewBag.Salty = "No memes";
            
                
               id3learning.Run(inputs, outputs);


            String answer=  codebook.Translate("ShowUser", tree.Compute(codebook.Translate("No", "No", "No", "No", "No", "No", "3", "<10")));
            ViewBag.Salty = answer;
        }
    }
}
