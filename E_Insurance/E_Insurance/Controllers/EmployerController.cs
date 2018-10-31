using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using E_Insurance.Models;
using System.Web.Helpers;
using System.IO;
using System.Configuration;

namespace E_Insurance.Controllers
{
    public class EmployerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly HttpContext _context = System.Web.HttpContext.Current;

        // GET: /Employer/
        public ActionResult Index()
        {
            return View(db.Employers.ToList());
        }

        // GET: /Employer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employer employer = db.Employers.Find(id);
            if (employer == null)
            {
                return HttpNotFound();
            }
            return View(employer);
        }

        // GET: /Employer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Employer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Employer_Id,Employer_Name,Incorporation_No,Registration_No,CAC,Company_Logo,Address,Street_No,City,Local_Govt,State,Postal_Address,Telephone_No,Fax_No,Email,UserName,SurName,Password,FirstName,MiddleName,Position,Tel_No,Mode_Of_Identification,Business_Sector")] Employer employer,IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength  > 0)
                    {
                        
                        string path = System.IO.Path.Combine(Server.MapPath("/Uploads"), Guid.NewGuid() + Path.GetExtension(file.FileName));
                        file.SaveAs(path);
                        employer.CAC = file.FileName;
                        employer.Company_Logo = file.FileName;

                    }
                }
                employer.Registration_No = GenerateRegNo(employer.Employer_Id); //Generate RegNo.
                employer.Password = GeneratePassword();//Generate Password
                db.Employers.Add(employer);
                db.SaveChanges();
                WebMail.SmtpServer = "smtp-mail.outlook.com";
                WebMail.SmtpPort = 587;
                WebMail.SmtpUseDefaultCredentials = true;
                WebMail.EnableSsl = true;
                WebMail.UserName = "no_reply_e-insurance@outlook.com";
                WebMail.Password = "Iyaniwura";
                WebMail.From = "no_reply_e-insurance@outlook.com";
                WebMail.Send(to: employer.Email, subject: "Email Confirmation with Registration No. and Password",
                    body: "Hello " + " " + employer.Employer_Name + " "
                    + "<br />" + "Your 9  Digit Registration Number is :" + " " +
                    employer.Registration_No + "<br /> " + "Your Password is : " + employer.Password + "<br />"
                    + "Copy the link below and paste it on your browser to Login to your Dashboard" + "<br />" + "http://localhost:5555/Employer/EmployerLogin", isBodyHtml: true);
                
                return RedirectToAction("Success");
            }

            return View(employer);
        }
        public ActionResult Success()
        {
            return View();
        }
        public string GenerateRegNo(int id)
        {
            var emp = db.Employers.FirstOrDefault(m => m.Employer_Id == id); 
            var rand = new Random();
            var Nrand = rand.Next(100000000,500000000);
            return emp + Nrand.ToString();

        }
       
        public bool IsMatricNoExist(string regNo)
        {
            var isExist = db.Employers.Where(c => c.Registration_No == regNo);
            return isExist.Count() == 1;
        }

        public string GeneratePassword()
        {
            var allowedChas = "";
            const int length = 8;
            allowedChas += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            allowedChas += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowedChas += "0,1,2,3,4,5,6,7,8,9";
            var arr = allowedChas.Split(',');
            var password = "";
            var rand = new Random();
            for (int i = 0; i < length; i++)
            {
                var temp = arr[rand.Next(0, arr.Length)];
                password += temp;
            }
            return password;
        }

        [HttpGet]
        public ActionResult EmployerLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EmployerLogin(string regNo, string pword)
        {
            if (regNo == "" && pword == "")
            {
                ViewBag.Registration = "Registration No. required";
                ViewBag.Password = "Password required";

                return View("EmployerLogin");
            }
           
            
            if (regNo == "")
            {
                ViewBag.Registration = "Registration No. is required";

                return View("EmployerLogin");
            }
            if (pword == "")
            {
                ViewBag.Password = "Password is required";

                return View("EmployerLogin");
            }

            var employer = db.Employers.SingleOrDefault(m => m.Registration_No == regNo && m.Password == pword);

            if (employer != null)
            {
                ViewBag.Registration = null;
                ViewBag.EmployerPassword = null;
                ViewBag.EmployerNotExist = null;

                _context.Session["registration_No"] = employer.Registration_No;
                _context.Session["employer_Id"] = employer.Employer_Id;
                _context.Session["employer_Name"] = employer.Employer_Name;

                return RedirectToAction("Dashboard", "Employer");
            }

            _context.Session["Registration_No"] = null;
            _context.Session["Employer_Id"] = null;
            _context.Session["Employer_Name"] = null;

            ViewBag.EmployerNotExist = "Invalid Registration No. / Password";

            return View();
        }
        
       
       
        public ActionResult Dashboard()
        {
            if (_context.Session["Employer_Id"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int id = (int)(_context.Session["Employer_Id"]);
            var employer = db.Employers.Find(id);
            if (employer == null)
            {
                return HttpNotFound();
            }
            return View(employer);
        }
        // GET: /Employer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employer employer = db.Employers.Find(id);
            if (employer == null)
            {
                return HttpNotFound();
            }
            return View(employer);
        }

        // POST: /Employer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Employer_Id,Employer_Name,Incorporation_No,Registration_No,CAC,Company_Logo,Address,Street_No,City,Local_Govt,State,Postal_Address,Telephone_No,Fax_No,Email,UserName,SurName,Password,FirstName,MiddleName,Position,Tel_No,Mode_Of_Identification,Construction,Minning,Banking_and_Finance,Manufacturing,Aviation,MDA,Oil_and_Gas,Agriculture,Energy,Telecommunication,Others")] Employer employer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employer);
        }

        // GET: /Employer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employer employer = db.Employers.Find(id);
            if (employer == null)
            {
                return HttpNotFound();
            }
            return View(employer);
        }

        // POST: /Employer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employer employer = db.Employers.Find(id);
            db.Employers.Remove(employer);
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
