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
    public class ComplianceController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly HttpContext _context = System.Web.HttpContext.Current;

        // GET: /Compliance/
        //[Authorize(Roles="Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var emp = db.Compliances.Include(c => c.Employer);
            return View(emp.ToList());
        }

        // GET: /Compliance/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Compliance compliance = db.Compliances.Find(id);
            if (compliance == null)
            {
                return HttpNotFound();
            }
            return View(compliance);
        }

        // GET: /Compliance/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Compliance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Compliance_Id,CompanyName,RegistrationNo,Employer_Id")] Compliance compliance)
        {
            if (ModelState.IsValid)
            {
                if(_context.Session["Employer_Id"] == null)
                {
                    return RedirectToAction("EmployerLogin", "Employer");
                }
                int id = (int)_context.Session["Employer_Id"];
                compliance.Employer_Id = id;
                db.Compliances.Add(compliance);
                db.SaveChanges();
                return RedirectToAction("ComplianceSuccess");
            }

            return View(compliance);
        }


        public ViewResult ComplianceSuccess()
        {
            @ViewBag.ComplianceMsg = "Your Request was Successful";

            return View();

        }
        public ActionResult ComplianceApproval(int? id)
        {
            ComplianceActivity compActivity = new ComplianceActivity();
            Compliance comp = db.Compliances.Find(id);
            if (comp.Approve != true)
            {
                comp.Approve = true;
                db.SaveChanges();
            }
            return View();
        }
        //[Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin")]
        public ViewResult ApproveCompliance(int id)
        {

            Compliance claim = db.Compliances.Find(id);


            return View(claim);
        }

        public ViewResult ViewApproval(int id)
        {

            Compliance comp = db.Compliances.Find(id);


            return View(comp);
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
            var claims = db.Compliances.Find(id);
            if (claims == null)
            {
                return HttpNotFound();
            }
            return View(claims);
        }
        public ActionResult ViewApprovals()
        {
            int id = (int)(_context.Session["Employer_Id"]);
            var claims = db.Compliances.Include(emp => emp.Employer).Where(emp => emp.Employer_Id == id).ToList();

            return View(claims);
        }
        // GET: /Compliance/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Compliance compliance = db.Compliances.Find(id);
            if (compliance == null)
            {
                return HttpNotFound();
            }
            return View(compliance);
        }

        // POST: /Compliance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Compliance_Id,CompanyName,RegistrationNo,EmployerId")] Compliance compliance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(compliance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(compliance);
        }

        // GET: /Compliance/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Compliance compliance = db.Compliances.Find(id);
            if (compliance == null)
            {
                return HttpNotFound();
            }
            return View(compliance);
        }

        // POST: /Compliance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Compliance compliance = db.Compliances.Find(id);
            db.Compliances.Remove(compliance);
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
