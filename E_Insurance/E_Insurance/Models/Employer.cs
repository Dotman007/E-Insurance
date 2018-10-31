using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Insurance.Models
{
    public class Employer
    {
        //Particulars of Business starts here
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Employer_Id { get; set; }
            
        [Required]
        [Display(Name="Employer's Name")]
        public string Employer_Name { get; set; }

        [Required]
        [Display(Name="Incorporation Number")]
        public string Incorporation_No { get; set; }


       
        public string Registration_No { get; set; }

        [Display(Name = "CAC Document")]
        public string CAC { get; set; }

        [Display(Name = "Company Logo")]
        public string Company_Logo { get; set; }
        [Required]
        [Display(Name="Address")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        [Required]
        [Display(Name="Street No.")]
        public string Street_No { get; set; }
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }
        [Required]
        [Display(Name = "LGA")]
        public string Local_Govt { get; set; }
        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Postal Address")]
        public string Postal_Address { get; set; }

        [Required]
        
        [Display(Name = "Telephone No."),DataType(DataType.PhoneNumber)]
        public string Telephone_No { get; set; }

        [Required]
        [Display(Name = "Fax No.")]
        public string Fax_No { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Format")]
        public string Email { set; get; }

        //Particulars of Business ends here



        //Particulars of Organizations owner starts here

         
        [Required]
        [Display(Name="Surname")]
        public string SurName { get; set; }

        public string Password { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string  FirstName { get; set; }
        [Required]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Required]
        [Display(Name = "Position")]
        public string Position { get; set; }
        [Required]
        [Display(Name = "Telephone No")]
        public string Tel_No { get; set; }
        [Required]
        [Display(Name = "Mode of Identification")]
        public string Mode_Of_Identification { get; set; }

        [Required]
        [Display(Name="Business Sector")]
        public string Business_Sector { get; set; }

        //Business Sector Categories ends here
    }
}