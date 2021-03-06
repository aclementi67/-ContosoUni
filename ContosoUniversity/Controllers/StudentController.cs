﻿using System.Net;
using System.Web.Mvc;
using ContosoUniversity.Models;
using ContosoUniversity.DataAccessLayer;
using System.Data;
using System.Linq;
using PagedList;

namespace ContosoUniversity.Controllers
{
    public class StudentController : Controller
    {
        private StudentCRUD studentCRUD = new StudentCRUD();

        // GET: Student
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (searchString == null)
            {
                searchString = currentFilter; // allow for paging through a search
            } else
            {
                page = 1; // reset the page for new search
            }

            setupViewBag(sortOrder, searchString);

            var students = sortStudentlist(sortOrder, getStudentList(searchString));

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));
        }

        private void setupViewBag(string sortOrder, string searchString)
        {
            base.ViewBag.CurrentSort = sortOrder;
            base.ViewBag.LastNameSortParm = System.String.IsNullOrEmpty(sortOrder) ? "last_name_desc" : "";
            base.ViewBag.FirstNameSortParm = System.String.IsNullOrEmpty(sortOrder) ? "first_name_desc" : "";
            base.ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            base.ViewBag.CurrentFilter = searchString;
        }

        private static System.Collections.Generic.List<Student> sortStudentlist(string sortOrder, IQueryable<Student> students)
        {
            switch (sortOrder)
            {
                case "last_name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "first_name_desc":
                    students = students.OrderByDescending(s => s.FirstMidName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            return students.ToList();
        }

        private IQueryable<Student> getStudentList(string searchString)
        {
            System.Linq.IQueryable<Student> students;
            if (System.String.IsNullOrEmpty(searchString))
            {
                students = studentCRUD.StudentList();
            }
            else
            {
                students = studentCRUD.SearchStudentList(searchString);
            }

            return students;
        }

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = studentCRUD.Read((int)id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName, FirstMidName, EnrollmentDate")]Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (studentCRUD.Create(student))
                        return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(student);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            return Details(id);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                if (studentCRUD.Update(student))
                {
                    return RedirectToAction("Index");
                }
            }
            return View(student);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int? id)
        {
            return Details(id);
        }
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Student student = studentCRUD.Read((int)id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                studentCRUD.Delete(id);
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
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
