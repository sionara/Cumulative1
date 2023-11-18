using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cumulative1.Models;

namespace Cumulative1.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Returns a List of Students from the school DB that has matching first or last names to the 
        /// search key.
        /// </summary>
        /// <param name="SearchKey">The search key for Students </param>
        /// <returns>
        /// GET: Student/List/Sarah -> {[Sarah Valdez]}
        /// </returns>
        public ActionResult List(string SearchKey)
        {
            StudentDataController controller = new StudentDataController();
            List<Student> students = controller.ListStudents(SearchKey);

            return View(students);
        }
        /// <summary>
        /// Returns a specific Student from the School Db based on their Id.
        /// </summary>
        /// <param name="id">Id of Student that was searched.</param>
        /// <returns>
        /// GET: Student/Show/1 -> Sarah Valdez
        /// </returns>
        public ActionResult Show(int id)
        {
            StudentDataController controller = new StudentDataController();
            Student student = controller.FindStudent(id);

            return View(student);
        }

    }
}