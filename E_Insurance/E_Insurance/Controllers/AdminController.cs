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
	public class AdminController : Controller
	{
        private readonly ApplicationDbContext db = new ApplicationDbContext();
		private readonly HttpContext _context = System.Web.HttpContext.Current;
		public ActionResult CasesHome()
		{
			if (_context.Session["Admin_Id"] == null)
			{
				return RedirectToAction("AdminLogin", "Admin");
			}
			var cases = db.Cases.Include(c => c.Employer);

			return View(cases.ToList());
		}


		[HttpGet]
		public ActionResult AdminLogin()
		{
			return View();
		}

        [HttpPost]
        public ActionResult AdminLogin(string username, string adminPassword)
        {
            if (username == "" && adminPassword == "")
            {
                ViewBag.Username = "Username required";
                ViewBag.AdminPassword = "Password required";

                return View("AdminLogin");
            }
            if (username == "")
            {
                ViewBag.Username = "Username required";

                return View("AdminLogin");
            }
            if (adminPassword == "")
            {
                ViewBag.AdminPassword = "Password required";

                return View("AdminLogin");
            }

            var admin = db.Admins.SingleOrDefault(u => u.Username == username && u.Password == adminPassword);

            if (admin != null)
            {
                ViewBag.Username = null;
                ViewBag.AdminPassword = null;
                ViewBag.AdminNotExist = null;

                _context.Session["Admin_Username"] = admin.Username;
                _context.Session["Admin_Id"] = admin.Id;

                return RedirectToAction("Index", "Admin");
            }

            _context.Session["Admin_Username"] = null;
            _context.Session["Admin_Id"] = null;

            ViewBag.AdminNotExist = "Invalid Username/Password";

            return View("AdminLogin");
        }
		// GET: /Admin/
		public ActionResult Index()
		{
			if (_context.Session["Id"] == null)
			{
				return RedirectToAction("AdminLogin", "Admin");
			}
			return View(db.Admins.ToList());
		}
		public ActionResult Dashboard()
		{
			if (_context.Session["Id"] == null)
			{
				return RedirectToAction("AdminLogin", "Admin");
			}

			int id = (int)(_context.Session["Id"]);
			var admin = db.Admins.Find(id);
			if (admin == null)
			{
				return HttpNotFound();
			}
			return View(admin);
		}

		// GET: /Admin/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Admin admin = db.Admins.Find(id);
			if (admin == null)
			{
				return HttpNotFound();
			}
			return View(admin);
		}

		// GET: /Admin/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: /Admin/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include="Id,Username,Password")] Admin admin)
		{
			if (ModelState.IsValid)
			{
				db.Admins.Add(admin);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(admin);
		}

		// GET: /Admin/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Admin admin = db.Admins.Find(id);
			if (admin == null)
			{
				return HttpNotFound();
			}
			return View(admin);
		}

		// POST: /Admin/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include="Id,Username,Password")] Admin admin)
		{
			if (ModelState.IsValid)
			{
				db.Entry(admin).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(admin);
		}

		// GET: /Admin/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Admin admin = db.Admins.Find(id);
			if (admin == null)
			{
				return HttpNotFound();
			}
			return View(admin);
		}

		// POST: /Admin/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Admin admin = db.Admins.Find(id);
			db.Admins.Remove(admin);
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
