using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Insurance.Models
{
    public class ComplianceActivity
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       
        public void Approve(int? compliance_Id)
        {
            Compliance comp = db.Compliances.Single(r => r.Compliance_Id == compliance_Id);
            if (compliance_Id != null)
            {
                comp.Approve = true;
                db.SaveChanges();
            }
        }
    }
}