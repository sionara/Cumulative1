using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cumulative1.Models
{
    public class Teacher
    {
        public int TeacherId;
        [Required(ErrorMessage = "Please enter teacher name.")]
        public string TeacherFName;
        public string TeacherLName;
        public string EmployeeNumber;
        public DateTime HireDate;
        public double Salary;
    }
}