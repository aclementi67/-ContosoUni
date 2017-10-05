using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ContosoUniversity.ViewModels;
using ContosoUniversity.DataAccessLayer;

namespace ContosoUniversity.Controllers
{
    public class HomeController : Controller
    {
        private StudentCRUD studentCRUD = new StudentCRUD();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            var students = studentCRUD.StudentList();
            IQueryable<EnrollmentDateGroup> data = from student in students
                                                   group student by student.EnrollmentDate into dateGroup
                                                   select new EnrollmentDateGroup()
                                                   {
                                                       EnrollmentDate = dateGroup.Key,
                                                       StudentCount = dateGroup.Count()
                                                   };
            return View(data.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                studentCRUD.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}