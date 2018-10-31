using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Insurance.Models;
using System.IO;

namespace E_Insurance.Controllers
{
    public class ClaimController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly HttpContext _context = System.Web.HttpContext.Current;
        // GET: /Claim/
        [Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            var claims = db.Claims.Include(c => c.Employer);
            return View(claims.ToList());
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
            var claims = db.Claims.Find(id);
            if (claims == null)
            {
                return HttpNotFound();
            }
            return View(claims);
        }

        public ActionResult ViewClaims()

        {
           int  id = (int)(_context.Session["Employer_Id"]);
            var claims = db.Claims.Include(emp=>emp.Employer).Where(emp => emp.Employer_Id == id).ToList();

            return View(claims);
        }

        // GET: /Claim/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claim claim = db.Claims.Find(id);
            if (claim == null)
            {
                return HttpNotFound();
            }
            return View(claim);
        }

        // GET: /Claim/Create
        public ActionResult Create()
        {
            

            //ViewBag.Employer_Id = new SelectList(db.Employers, "Employer_Id", "Employer_Name");
            return View();
        }
        // POST: /Claim/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Claim_Id,Employee_Surname,Employee_FirstName,Employee_MiddleName,Marital_Status,Number_Of_Spouse,Number_Of_Children,Age_Of_Children,Employer_Name,Employer_Address,Surname,FirstName,MiddleName,Date_Of_Birth,First_Aid,Report_Date,Date_Of_First_Treatment,Gender,Name,Address,Name_Of_Specialist,Registration_Date,Registration_Charge,Consultation_Date,Consultation_Charge,Admission_Date,Admission_Charge,Medical_Procedure_Date,Medical_Procedure_Charge,Drug_Date,Drug_Charge,Laboratory_Service_Date,Laboratory_Service_Charge,Total,Name_Of_Account,Account_No,Sort_Code,Bank_Name,Employer_Id,Accident_Image")] Claim claim, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                if(_context.Session["Employer_Id"] == null)
                {
                    return RedirectToAction("EmployerLogin", "Employer");
                }
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {

                        string path = System.IO.Path.Combine(Server.MapPath("/Uploads"), Guid.NewGuid() + Path.GetExtension(file.FileName));
                        file.SaveAs(path);
                        claim.Accident_Image = file.FileName;

                    }
                }
                int id = (int)_context.Session["Employer_Id"];
                
                  claim.Total = GetTotal(claim.Registration_Charge, claim.Consultation_Charge, claim.Admission_Charge
                    , claim.Medical_Procedure_Charge, claim.Drug_Charge, claim.Laboratory_Service_Charge);
               
                claim.Compensation = getCompensation(claim.Registration_Charge, claim.Consultation_Charge, claim.Admission_Charge
                    , claim.Medical_Procedure_Charge, claim.Drug_Charge, claim.Laboratory_Service_Charge,claim.Salary);
                claim.Employer_Id = id;
                db.Claims.Add(claim);
                db.SaveChanges();
                return RedirectToAction("SuccessClaim");
            }

            //ViewBag.Employer_Id = new SelectList(db.Employers, "Employer_Id", "Employer_Name", claim.Employer_Id);
            return View(claim);
        }

        public double getCompensation(double regCharge, double consultCharge, double admiCharge, double medprocCharge, double drugCharge, double labCharge,double salary)
        {
           
            var tot = regCharge + consultCharge + admiCharge + medprocCharge + drugCharge + labCharge * salary * 0.9;
            return tot;
        }
        public ActionResult SuccessClaim()
        {
            if (_context.Session["Employer_Id"] == null)
            {
                return RedirectToAction("EmployerLogin", "Employer");
            }
            @ViewBag.Success = "Your Claim was Successfully Registered, Kindly wait for the Approval of your Claim";
            return View();
        }

        public double GetTotal(double regCharge, double consultCharge, double admisCharge, double medprocCharge, double drugCharge, double labCharge)
        {
            var getTotal = regCharge + consultCharge + admisCharge + medprocCharge + drugCharge + labCharge;
            return getTotal;
        }
       

        public ActionResult Approval(int? id)
        {
            ApproveActivity activity = new ApproveActivity();
            Claim claim = db.Claims.Find(id);
            if(claim.Approve != true)
            {
                claim.Approve = true;
                db.SaveChanges();
            }
            return View();
        }
         
        public ViewResult ViewCompensation(int id)
        {
           
            Claim claim = db.Claims.Find(id);
            
            
            return View(claim);
        }



        public ViewResult ViewApproval(int id)
        {

            Claim claim = db.Claims.Find(id);


            return View(claim);
        }
        // GET: /Claim/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claim claim = db.Claims.Find(id);
            if (claim == null)
            {
                return HttpNotFound();
            }
            ViewBag.Employer_Id = new SelectList(db.Employers, "Employer_Id", "Employer_Name", claim.Employer_Id);
            return View(claim);
        }

        // POST: /Claim/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Claim_Id,Employee_Surname,Employee_FirstName,Employee_MiddleName,Marital_Status,Number_Of_Spouse,Number_Of_Children,Age_Of_Children,Employer_Name,Employer_Address,Surname,FirstName,MiddleName,Date_Of_Birth,First_Aid,Report_Date,Date_Of_First_Treatment,Gender,Name,Address,Name_Of_Specialist,Registration_Date,Registration_Charge,Consultation_Date,Consultation_Charge,Admission_Date,Admission_Charge,Medical_Procedure_Date,Medical_Procedure_Charge,Drug_Date,Drug_Charge,Laboratory_Service_Date,Laboratory_Service_Charge,Total,Name_Of_Account,Account_No,Sort_Code,Bank_Name,Treatment_Receipt,Details_Of_Bills,Passport_Photograph,Police_Report,Employer_Id")] Claim claim)
        {
            if (ModelState.IsValid)
            {
                db.Entry(claim).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Employer_Id = new SelectList(db.Employers, "Employer_Id", "Employer_Name", claim.Employer_Id);
            return View(claim);
        }

        // GET: /Claim/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claim claim = db.Claims.Find(id);
            if (claim == null)
            {
                return HttpNotFound();
            }
            return View(claim);
        }

        // POST: /Claim/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Claim claim = db.Claims.Find(id);
            db.Claims.Remove(claim);
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
