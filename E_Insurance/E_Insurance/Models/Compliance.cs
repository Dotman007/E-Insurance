using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace E_Insurance.Models
{
    public class Compliance
    {
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Key]
        public int Compliance_Id { get; set; }
        [Display(Name="Company Name")]
        public string CompanyName  { get; set; }
        [Display(Name="Registration No.")]
        public string RegistrationNo { get; set; }
        public int Employer_Id { get; set; }

        public bool Approve { get; set; }
        

        public virtual Employer Employer { get; set; }
    }
}