using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.Controllers
{
    public class CommonController : Controller
    {
        protected Team83LMSContext db;

        public CommonController()
        {
            db = new Team83LMSContext();
        }

        public void UseLMSContext(Team83LMSContext ctx)
        {
            db = ctx;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// Retreive a JSON array of all departments from the database.
        /// Each object in the array should have a field called "name" and "subject",
        /// where "name" is the department name and "subject" is the subject abbreviation.
        /// </summary>
        /// <returns>The JSON array</returns>
        public IActionResult GetDepartments()
        {
            var query = from dept in db.Departments
                        select new { name = dept.DName, subject = dept.DSubject };

            return Json(query.ToArray());
        }



        /// <summary>
        /// Returns a JSON array representing the course catalog.
        /// Each object in the array should have the following fields:
        /// "subject": The subject abbreviation, (e.g. "CS")
        /// "dname": The department name, as in "Computer Science"
        /// "courses": An array of JSON objects representing the courses in the department.
        ///            Each field in this inner-array should have the following fields:
        ///            "number": The course number (e.g. 5530)
        ///            "cname": The course name (e.g. "Database Systems")
        /// </summary>
        /// <returns>The JSON array</returns>
        public IActionResult GetCatalog()
        {
            var depts = from d in db.Departments
                        select new
                        {
                            subject = d.DSubject,
                            dname = d.DName,
                            courses = from c in db.Courses
                                      where c.DSubject == d.DSubject
                                      select new
                                      {
                                          number = c.CNumber,
                                          cname = c.CName
                                      }
                        };
            return Json(depts.ToArray());
        }

        /// <summary>
        /// Returns a JSON array of all class offerings of a specific course.
        /// Each object in the array should have the following fields:
        /// "season": the season part of the semester, such as "Fall"
        /// "year": the year part of the semester
        /// "location": the location of the class
        /// "start": the start time in format "hh:mm:ss"
        /// "end": the end time in format "hh:mm:ss"
        /// "fname": the first name of the professor
        /// "lname": the last name of the professor
        /// </summary>
        /// <param name="subject">The subject abbreviation, as in "CS"</param>
        /// <param name="number">The course number, as in 5530</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetClassOfferings(string subject, int number)
        {

            var course = from c in db.Courses
                         where c.DSubject == subject && c.CNumber == number
                         select c;

            var classes = from c in course
                          join cls in db.Classes on c.CourseId equals cls.CourseId
                          join prof in db.Professors on cls.ProfId equals prof.UserId
                          select new
                          {
                              season = cls.SemSeason,
                              year = cls.SemYear,
                              location = cls.Location,
                              start = cls.STime,
                              end = cls.ETime,
                              fname = prof.FName,
                              lname = prof.LName
                          };
            return Json(classes.ToArray());
        }


        /// <summary>
        /// This method does NOT return JSON. It returns plain text (containing html).
        /// Use "return Content(...)" to return plain text.
        /// Returns the contents of an assignment.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment in the category</param>
        /// <returns>The assignment contents</returns>
        public IActionResult GetAssignmentContents(string subject, int num, string season, int year, string category, string asgname)
        {
            var asg = (from course in db.Courses
                       where course.CNumber == num && course.DSubject == subject
                       from c in db.Classes
                       where c.CourseId == course.CourseId && c.SemSeason == season && c.SemYear == year
                       from cats in db.AssignmentCategories
                       where cats.ClassId == c.ClassId && cats.AcName == category
                       from a in db.Assignments
                       where a.AcId == cats.AcId && a.Name == asgname
                       select a).First();
            return Content(asg.AContents);
        }


        /// <summary>
        /// This method does NOT return JSON. It returns plain text (containing html).
        /// Use "return Content(...)" to return plain text.
        /// Returns the contents of an assignment submission.
        /// Returns the empty string ("") if there is no submission.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment in the category</param>
        /// <param name="uid">The uid of the student who submitted it</param>
        /// <returns>The submission text</returns>
        public IActionResult GetSubmissionText(string subject, int num, string season, int year, string category, string asgname, string uid)
        {
            var submission = from course in db.Courses
                             where course.CNumber == num && course.DSubject == subject
                             from cls in db.Classes
                             where cls.CourseId == course.CourseId && cls.SemSeason == season && cls.SemYear == year
                             from cat in db.AssignmentCategories
                             where cls.ClassId == cat.ClassId && cat.AcName == category
                             from assg in db.Assignments
                             where assg.AcId == cat.AcId && assg.Name == asgname
                             from sub in db.Submissions
                             where sub.AId == assg.AId && sub.UserId == uid
                             select sub;

            if(!submission.Any())
                return Content("");

            return Content(submission.First().SubContents);
        }


        /// <summary>
        /// Gets information about a user as a single JSON object.
        /// The object should have the following fields:
        /// "fname": the user's first name
        /// "lname": the user's last name
        /// "uid": the user's uid
        /// "department": (professors and students only) the name (such as "Computer Science") of the department for the user. 
        ///               If the user is a Professor, this is the department they work in.
        ///               If the user is a Student, this is the department they major in.    
        ///               If the user is an Administrator, this field is not present in the returned JSON
        /// </summary>
        /// <param name="uid">The ID of the user</param>
        /// <returns>
        /// The user JSON object 
        /// or an object containing {success: false} if the user doesn't exist
        /// </returns>
        public IActionResult GetUser(string uid)
        {

            var student = from s in db.Students
                          where s.UserId == uid
                          select new
                          {
                              fname = s.FName,
                              lname = s.LName,
                              uid = uid,
                              department = s.DSubjectNavigation.DName
                          };
            if (student.Any())
            {
                return Json(student.First());
            }

            var prof = from p in db.Professors
                       where p.UserId == uid
                       select new
                       {
                           fname = p.FName,
                           lname = p.LName,
                           uid = uid,
                           department = p.DeptNavigation.DName
                       };
            if (prof.Any())
            {
                return Json(prof.First());
            }

            var admin = from a in db.Administrators
                        where a.UserId == uid
                        select new
                        {
                            fname = a.FName,
                            lname = a.LName,
                            uid = uid,
                        };
            if (admin.Any())
            {
                return Json(admin.First());
            }


            return Json(new { success = false });
        }
    }
}