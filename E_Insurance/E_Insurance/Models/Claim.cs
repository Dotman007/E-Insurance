using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Insurance.Models
{
    public class Claim
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int  Claim_Id { get; set; }
        [Required,Display(Name="Surname")]
        public string Employee_Surname { get; set; }
        [Required, Display(Name = "First Name")]
        public string Employee_FirstName { get; set; }
        [Required, Display(Name = "Middle Name")]
        public string Employee_MiddleName { get; set; }
        [Required, Display(Name = "Status")]
        public string Marital_Status { get; set; }

        [Required, Display(Name = "No. of Spouse")]
        public string Number_Of_Spouse { get; set; }

        [Required, Display(Name = "No. of Children")]
        public string Number_Of_Children { get; set; }
        [Required, Display(Name = "Children Age")]
        public string Age_Of_Children { get; set; }
        [Required, Display(Name = "Employer Name")]

        public string Employer_Name { get; set; }
        [Required, Display(Name = "Employer Address")]
        public string Employer_Address { get; set; }


        //Dependant
        [Required, Display(Name = "Surname")]
        public string Surname { get; set; }
        [Required, Display(Name = "First Name")]
        public string   FirstName { get; set; }

        [Required, Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Required, Display(Name = "Birth Date")]
        public DateTime Date_Of_Birth { get; set; }

        [Required, Display(Name = "First Aid Treatment ?")]
        public bool First_Aid { get; set; }
        [Required, Display(Name = "Report Date")]
        public DateTime Report_Date { get; set; }

        [Required, Display(Name = "First Treatment Date")]
        public DateTime Date_Of_First_Treatment { get; set; }
        [Required, Display(Name = "Gender")]
        public string Gender { get; set; }

        //Health Provider Section
        [Required, Display(Name = "Name")]
        public string Name { get; set; }

        [Required, Display(Name = "Address")]
        public string Address { get; set; }
        [Required, Display(Name = "Specialist Name")]
        public string Name_Of_Specialist { get; set; }

        

        //Treatment Details
        [Required, Display(Name = "Registration Date")]
        public DateTime Registration_Date { get; set; }
        [Required, Display(Name = "Registration Charge")]
        public double Registration_Charge { get; set; }
        [Required, Display(Name = "Consultation Date")]
        public DateTime Consultation_Date { get; set; }
        [Required, Display(Name = "Consultation Charge")]
        public double Consultation_Charge { get; set; }
        [Required, Display(Name = "Admission Date")]
        public DateTime Admission_Date { get; set; }
        [Required, Display(Name = "Admission Charge")]
        public double Admission_Charge { get; set; }
        [Required, Display(Name = "Medical Procedure Date")]
        public DateTime Medical_Procedure_Date { get; set; }
        [Required, Display(Name = "Medical Procedure Charge")]
        public double Medical_Procedure_Charge { get; set; }
        [Required, Display(Name = "Drug Date")]
        public DateTime Drug_Date { get; set; }
        [Required, Display(Name = "Drug Charge")]
        public double Drug_Charge { get; set; }

        [Required, Display(Name = "Laboratory Service Date")]
        public DateTime Laboratory_Service_Date { get; set; }
        [Required, Display(Name = "Laboratory Service Charge")]
        public double Laboratory_Service_Charge { get; set; }

        public double Total { get; set; }



        //Bank details of eligible person(s)
        [Required, Display(Name = "Account Name")]

        public string Name_Of_Account { get; set; }

        [Required, Display(Name = "Account No.")]
        public string Account_No { get; set; }
        [Required, Display(Name = "Sort Code")]
        public string Sort_Code { get; set; }
        [Required, Display(Name = "Bank Name")]
        public string  Bank_Name { get; set; }


        //Documents Uploads
        [Display(Name = "Treatment Reciept")]
        public string Treatment_Receipt { get; set; }
        [Display(Name="Compensation")]
        public double Compensation { get; set; }

        [Display(Name = "Approve")]
        public bool Approve { get; set; }


        [Display(Name="Disapprove")]
        public bool Disapprove { get; set; }


        [Display(Name = "Remuneration")]
        public double Remuneration { get; set; }

        [Display(Name = "Salary")]
        public double Salary { get; set; }


       
        [Display(Name = "Passport")]
        public string Passport_Photograph { get; set; }

         [Display(Name = "Accident Image")]
        public string Accident_Image { get; set; }

        

        public int Employer_Id { get; set; }
        public virtual Employer Employer { get; set; }
    }
}