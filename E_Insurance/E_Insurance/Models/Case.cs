using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Insurance.Models
{
    public class Case
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        //Case
        public  int Id { get; set; }
        [Display(Name="Case Number")]
        public string Case_No { get; set; }
        
        
        //Employee
        
        public string Employee_Surname { get; set; }
        [Display(Name = "First Name")]
        public string Employee_FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string Employee_MiddleName { get; set; }
        [Display(Name = "Salary")]
        public double Salary { get; set; }
        [Display(Name = "Date of Incident")]
        public DateTime Incident_Date { get; set; }
        [Display(Name = "Time of Incident")]
        public string Time_of_Incident { get; set; }
        [Display(Name = "Town  of Incident")]
        public string Town { get; set; }
        [Display(Name = "LGA of Incident")]
        public string LGA { get; set; }
        [Display(Name = "State of Incident")]
        public string State { get; set; }
        [Display(Name = "Date of Report")]
        public DateTime Incident_Report_Date { get; set; }
        [Display(Name = "Type of Incident")]
        public string TypeOfIncident { get; set; }
        [Display(Name = "Employer")]
        public int Employer_Id { get; set; }


        public virtual Employer Employer { get; set; }
        
    }
}