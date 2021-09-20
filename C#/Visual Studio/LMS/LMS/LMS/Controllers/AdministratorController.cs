using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : CommonController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Department(string subject)
        {
            ViewData["subject"] = subject;
            return View();
        }

        public IActionResult Course(string subject, string num)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            return View();
        }

        /*******Begin code to modify********/

        /// <summary>
        /// Returns a JSON array of all the courses in the given department.
        /// Each object in the array should have the following fields:
        /// "number" - The course number (as in 5530)
        /// "name" - The course name (as in "Database Systems")
        /// </summary>
        /// <param name="subject">The department subject abbreviation (as in "CS")</param>
        /// <returns>The JSON result</returns>
        public IActionResult GetCourses(string subject)
        {
            var query = from c in db.Courses
                        where c.DSubject == subject
                        select new
                        {
                            number = c.CNumber,
                            name = c.CName
                        };
            return Json(query.ToArray());
        }





        /// <summary>
        /// Returns a JSON array of all the professors working in a given department.
        /// Each object in the array should have the following fields:
        /// "lname" - The professor's last name
        /// "fname" - The professor's first name
        /// "uid" - The professor's uid
        /// </summary>
        /// <param name="subject">The department subject abbreviation</param>
        /// <returns>The JSON result</returns>
        public IActionResult GetProfessors(string subject)
        {
            var query = from p in db.Professors
                        where p.Dept == subject
                        select new
                        {
                            lname = p.LName,
                            fname = p.FName,
                            uid = p.UserId
                        };
            return Json(query.ToArray());
        }



        /// <summary>
        /// Creates a course.
        /// A course is uniquely identified by its number + the subject to which it belongs
        /// </summary>
        /// <param name="subject">The subject abbreviation for the department in which the course will be added</param>
        /// <param name="number">The course number</param>
        /// <param name="name">The course name</param>
        /// <returns>A JSON object containing {success = true/false},
        /// false if the Course already exists.</returns>
        public IActionResult CreateCourse(string subject, int number, string name)
        {
            if (number < 0 || number > 9999)
                return Json(new { success = false });

            try
            {
                Courses c = new Courses { CName = name, CNumber = (ushort)number, DSubject = subject };
                db.Add(c);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }



        /// <summary>
        /// Creates a class offering of a given course.
        /// </summary>
        /// <param name="subject">The department subject abbreviation</param>
        /// <param name="number">The course number</param>
        /// <param name="season">The season part of the semester</param>
        /// <param name="year">The year part of the semester</param>
        /// <param name="start">The start time</param>
        /// <param name="end">The end time</param>
        /// <param name="location">The location</param>
        /// <param name="instructor">The uid of the professor</param>
        /// <returns>A JSON object containing {success = true/false}. 
        /// false if another class occupies the same location during any time 
        /// within the start-end range in the same semester, or if there is already
        /// a Class offering of the same Course in the same Semester.</returns>
        public IActionResult CreateClass(string subject, int number, string season, int year, DateTime start, DateTime end, string location, string instructor)
        {
            //t1S        t1E
            //     //t2S        t2E
            //bool overlap = a.start < b.end && b.start < a.end;
            var s = start.TimeOfDay.TotalSeconds;
            var e = end.TimeOfDay.TotalSeconds;

            //TODO: TEST THIS
            var sameSemester = from c in db.Classes
                               where c.Course.CNumber == number && c.SemSeason == season && c.SemYear == year
                               select c;

            //time overlap detection based on: https://stackoverflow.com/questions/13513932/algorithm-to-detect-overlapping-periods
            var sameLocTime = from c in db.Classes
                              where c.Location == location && (c.STime.TotalSeconds <= e && s <= c.ETime.TotalSeconds)
                              select c;

            if (sameSemester.Any() || sameLocTime.Any())
                return Json(new { success = false });

            var courseID = from c in db.Courses
                           where c.CNumber == number && c.DSubject == subject
                           select c;
            try
            {
                Classes newClass = new Classes
                {
                    STime = start.TimeOfDay,
                    ETime = end.TimeOfDay,
                    Location = location,
                    SemSeason = season,
                    ProfId = instructor,
                    SemYear = (ushort)year,
                    CourseId = courseID.First().CourseId
                };

                db.Add(newClass);
                db.SaveChanges();

            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }


        /*******End code to modify********/

    }
}