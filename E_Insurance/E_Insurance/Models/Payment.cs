using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace E_Insurance.Models
{
    public class Payment
    {
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        [Key]
        public int Payment_Id { get; set; }
        [Display(Name="Amount")]
        public double Amount { get; set; }
        [Display(Name="Employee Salary")]
        public double Employee_Salary { get; set; }
        public int Employer_Id { get; set; }

        public virtual Employer Employer { get; set; }
    }
}