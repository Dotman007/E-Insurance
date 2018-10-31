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
    public class PaymentController : Controller
    {
        private readonly HttpContext _context = System.Web.HttpContext.Current;
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Payment/
        [Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            var payments = db.Payments.Include(p => p.Employer);
            return View(payments.ToList());
        }

        public ActionResult ViewPayments()
        {
            int id = (int)(_context.Session["Employer_Id"]);
            var claims = db.Payments.Include(emp => emp.Employer).Where(emp => emp.Employer_Id == id).ToList();

            return View(claims);
        }


        public ActionResult PrintReceipt(int? id)
        {
            if (_context.Session["Employer_Id"] == null)
            {
                return RedirectToAction("EmployerLogin", "Employer");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }


        // GET: /Payment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: /Payment/Create
        public ActionResult Create()
        {
            //ViewBag.Employer_Id = new SelectList(db.Employers, "Employer_Id", "Employer_Name");
            return View();
        }

        // POST: /Payment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Payment_Id,Amount,Employee_Salary,Employer_Id")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                
                if(_context.Session["Employer_Id"] == null)
                {
                    return RedirectToAction("EmployerLogin", "Login");

                }
                int id = (int)_context.Session["Employer_Id"];
                payment.Employer_Id = id;
                db.Payments.Add(payment);
                db.SaveChanges();
                return RedirectToAction("PaymentSuccessful");
            }

            //ViewBag.Employer_Id = new SelectList(db.Employers, "Employer_Id", "Employer_Name", payment.Employer_Id);
            return View(payment);
        }

       

        public ViewResult PaymentSuccessful()
        {
            @ViewBag.PaymentSuccess = "Your Payment was Successful";
            return View();
        }

        // GET: /Payment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Employer_Id = new SelectList(db.Employers, "Employer_Id", "Employer_Name", payment.Employer_Id);
            return View(payment);
        }

        // POST: /Payment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Payment_Id,Amount,Employee_Salary,Employer_Id")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Employer_Id = new SelectList(db.Employers, "Employer_Id", "Employer_Name", payment.Employer_Id);
            return View(payment);
        }

        // GET: /Payment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: /Payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment payment = db.Payments.Find(id);
            db.Payments.Remove(payment);
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
