using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;

namespace ContosoUniversity.DataAccessLayer
{
    public class StudentCRUD
    {
        private SchoolContext db = new SchoolContext();

        public List<Student> StudentList()
        {
            return db.Students.ToList();
        }

        public bool Create( Student student)
        {
            bool result = (Read(student.ID) == null);
            if (result)
            {
                db.Students.Add(student);
                db.SaveChanges();
            }
            return result;
        }

        public Student Read(int id)
        {
            return db.Students.Find(id);
        }

        public bool Update(Student student)
        {
            db.Entry(student).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }

        public bool Delete(int ID)
        {
            Student studentToDelete = new Student() { ID = ID };
            db.Entry(studentToDelete).State = EntityState.Deleted; db.SaveChanges();
            db.SaveChanges();
            return true;
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
