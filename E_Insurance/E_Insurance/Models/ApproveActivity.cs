using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Insurance.Models;

namespace E_Insurance.Models
{
    

    public class ApproveActivity
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void Approve(int? claimId)
        {
            Claim claim = db.Claims.Single(r => r.Claim_Id == claimId);
            if(claimId != null)
            {
                claim.Approve = true;
                db.SaveChanges();
            }
        }
    }
}