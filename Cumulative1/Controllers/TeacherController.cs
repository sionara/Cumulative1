using System;
using System.Collections.Generic;
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
    }
}