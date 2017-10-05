using System.Net;
using System.Web.Mvc;
using ContosoUniversity.Models;
using ContosoUniversity.DataAccessLayer;
using System.Data;
using System.Linq;

namespace ContosoUniversity.Controllers
{
    public class StudentController : Controller
    {
        private StudentCRUD studentCRUD = new StudentCRUD();

        // GET: Student
        public ActionResult Index(string sortOrder)
        {
            base.ViewBag.LastNameSortParm = System.String.IsNullOrEmpty(sortOrder) ? "last_name_desc" : "";
            base.ViewBag.FirstNameSortParm = System.String.IsNullOrEmpty(sortOrder) ? "first_name_desc" : "";
            base.ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var students = studentCRUD.StudentList();
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
            return View(students.ToList());
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
