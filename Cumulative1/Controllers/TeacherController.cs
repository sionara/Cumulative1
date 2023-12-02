using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cumulative1.Models;

namespace Cumulative1.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        //GET: /Teacher/List?TeacherSearchKey -> This is the place the GET request with this URL will reach
        public ActionResult List(string TeacherSearchKey)
        {
            TeacherDataController controller = new TeacherDataController();
            List<Teacher> Teachers = controller.ListTeachers(TeacherSearchKey);
            return View(Teachers);
        }
        //GET: /Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher teacher = controller.FindTeacher(id);

            return View(teacher);
        }

        //GET: /Teacher/New/
        public ActionResult New()
        {
            return View();
        }

        //GET : /Teacher/Ajax_New
        public ActionResult Ajax_New()
        {
            return View();

        }

        //POST:/Teacher/Create
        [HttpPost]
        public ActionResult Create(string fname, string lname, string eNum, string hireDate, string salary) // HTML only returns strings. HTTP requests do not have data types.
        {
            // check this method is working
            Debug.WriteLine(fname);
            Debug.WriteLine(lname);
            Debug.WriteLine(eNum);
            Debug.WriteLine(hireDate);
            Debug.WriteLine(salary);

            // create new instance of Teacher object
            Teacher newTeacher = new Teacher();
            
            // check if date input is empty to prevent error with Parse and Convert methods.
            if (hireDate == "" || salary == "")
            {
                return RedirectToAction("New");
            }
            // convert date string into a DateTime object with only the Date
            DateTime newHireDate = DateTime.Parse(hireDate);

            // convert salary string into a double object
            double newSalary = Convert.ToDouble(salary);

            // access the information provided by the HttpPost method.
            newTeacher.TeacherFName = fname;
            newTeacher.TeacherLName = lname;
            newTeacher.EmployeeNumber = eNum;
            newTeacher.HireDate = newHireDate;
            newTeacher.Salary = newSalary;

            //call the AddTeacher method from TeacherDataController to add new data to Db.
            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(newTeacher);

            return RedirectToAction("List"); //return to List view. We could try to get Id of Teacher created to redirect to "Show" method, but that is an extra layer of complexity.
        }
        //GET: Teacher/ConfirmDelete/{id}
        public ActionResult ConfirmDelete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher newTeacher = controller.FindTeacher(id);

            return View(newTeacher);
        }
        //POST: /Teacher/Delete/{id}
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            
            return RedirectToAction("List");
        }
    }
}