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
            Teacher selectedteacher = controller.FindTeacher(id);

            return View(selectedteacher);
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
            // FOR DEBUGGING PURPOSE ONLY
            //Debug.WriteLine(fname);
            //Debug.WriteLine(lname);
            //Debug.WriteLine(eNum);
            //Debug.WriteLine(hireDate);
            //Debug.WriteLine(salary);

            // create new instance of Teacher object
            Teacher newTeacher = new Teacher();
            
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

            //call the AddTeacher method from TeacherDataController to create new Teacher and add it to Db.
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

        //GET: /Teacher/Update/{id}
        /// <summary>
        /// Redirects to server-side rendered "Update" view page. Will give current information about Teacher
        /// with Teacherid = id from Db.
        /// </summary>
        /// <param name="id">The id of the Teacher to show</param>
        /// <returns>
        /// A view to update teacher with teacherid = id
        /// </returns>
        public ActionResult Update(int id) // this method will receive POST data from view, and a GET request.
        {
            // find the selected teacher
            TeacherDataController controller = new TeacherDataController();
            Teacher selectedTeacher = controller.FindTeacher(id);

            return View(selectedTeacher);
        }

        /// <summary>
        /// Receives POST request from Update view with new information about an existing Teacher with TeacherId = id.
        /// This controller will then send that info to the appropriate API to update the Teacher in the Db.
        /// Then it will redirect to the Show view for that teacher.
        /// </summary>
        /// <param name="id">Id of Teacher to update</param>
        /// <param name="fname">Teacher first name</param>
        /// <param name="lname">Teahcer last name</param>
        /// <param name="eNum">Teacher employeee number</param>
        /// <param name="hireDate">Teacher hire date</param>
        /// <param name="salary">Teacher salary</param>
        /// <returns> Show view of Teacher with Teacherid = id
        /// </returns>
        /// <example>
        /// POST: /Teacher/Update/1
        /// Form data /POST Data / Request body
        /// {
        /// "TeacherFname": sion
        /// "TeacherLname": lee
        /// "EmployeeNumber": T111
        ///  "HireDate": 12-12-23
        ///  "Salary": 50
        /// }
        /// </example>
        /// 
        [HttpPost]
        public ActionResult Update(int id, string fname, string lname, string eNum, string hireDate, string salary)
        {
            // create new instance of Teacher object
            Teacher TeacherInfo = new Teacher();

            // convert date string into a DateTime object with only the Date
            DateTime newHireDate = DateTime.Parse(hireDate);

            // convert salary string into a double object
            double newSalary = Convert.ToDouble(salary);

            // access the information provided by the HttpPost method.
            TeacherInfo.TeacherFName = fname;
            TeacherInfo.TeacherLName = lname;
            TeacherInfo.EmployeeNumber = eNum;
            TeacherInfo.HireDate = newHireDate;
            TeacherInfo.Salary = newSalary;

            //call the UpdateTeacher method from Teacher API to update the information in Db using the post request.
            TeacherDataController controller = new TeacherDataController();
            controller.UpdateTeacher(id, TeacherInfo);

            return RedirectToAction("show/" + id);
        }
    }
}