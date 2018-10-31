using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Insurance.Models;

namespace E_Insurance.Controllers
{
    public class CaseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly HttpContext _context = System.Web.HttpContext.Current;

        // GET: /Case/
        public ActionResult Index()
        {
            var cases = db.Cases.Include(a => a.Employer);
            return View(cases.ToList());
        }


        public ActionResult ViewCases(int? id)
        {
            if (_context.Session["employee_Id"] == null)
            {
                return RedirectToAction("EmployerLogin", "Employer");
            }
            var empl = db.Employers.Find(id);
            var employer = db.Cases.Where(emp => emp.Employer_Id == id);

            return View(employer.ToList());
        }
        // GET: /Case/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            return View(@case);
        }

        // GET: /Case/Create
        public ActionResult Create()
        {
            if (_context.Session["Employer_Id"] == null)
            {
                return RedirectToAction("EmployerLogin", "Employer");
            }
            ViewBag.Employer_Id = new SelectList(db.Employers, "Employer_Id", "Employer_Name");
            return View();
        }

        // POST: /Case/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Case_No,Employee_Surname,Employee_FirstName,Employee_MiddleName,Salary,Incident_Date,Time_of_Incident,Town,LGA,State,Incident_Report_Date,TypeOfIncident,Employer_Id")] Case @case)
        {
            if (ModelState.IsValid)
            {


                @case.Case_No = GenerateCaseNo(@case.Id);

                db.Cases.Add(@case);
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Employer");
            }

            ViewBag.Employer_Id = new SelectList(db.Employers, "Employer_Id", "Employer_Name", @case.Employer_Id);
            return View(@case);
        }



        public string GenerateCaseNo(int id)
        {
            var emp = db.Cases.FirstOrDefault(m => m.Id == id);
            var rand = new Random();
            var Nrand = rand.Next(50000000, 80000000);
            return emp + Nrand.ToString();

        }




        // GET: /Case/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            ViewBag.Employer_Id = new SelectList(db.Employers, "Employer_Id", "Employer_Name", @case.Employer_Id);
            return View(@case);
        }

        // POST: /Case/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Case_No,Employee_Surname,Employee_FirstName,Employee_MiddleName,Salary,Incident_Date,Time_of_Incident,Town,LGA,State,Incident_Report_Date,TypeOfIncident,Employer_Id")] Case @case)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@case).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Employer_Id = new SelectList(db.Employers, "Employer_Id", "Employer_Name", @case.Employer_Id);
            return View(@case);
        }

        // GET: /Case/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            return View(@case);
        }

        // POST: /Case/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Case @case = db.Cases.Find(id);
            db.Cases.Remove(@case);
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
    }
}
