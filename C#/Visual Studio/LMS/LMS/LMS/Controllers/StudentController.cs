using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LMS.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : CommonController
    {
        /// <summary>
        /// Dictionary containing letter grade -> grade point mappings
        /// </summary>
        private static Dictionary<string, double> letterGPAs = new Dictionary<string, double>
            {
                {"A", 4.0 },
                {"A-", 3.7},
                {"B+", 3.3},
                {"B", 3},
                {"B-", 2.7},
                {"C+", 2.3},
                {"C", 2},
                {"C-", 1.7},
                {"D+", 1.3},
                {"D", 1},
                {"D-", 0.7},
                {"E", 0}
            };

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Catalog()
        {
            return View();
        }

        public IActionResult Class(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult Assignment(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }


        public IActionResult ClassListings(string subject, string num)
        {
            System.Diagnostics.Debug.WriteLine(subject + num);
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            return View();
        }


        /*******Begin code to modify********/

        /// <summary>
        /// Returns a JSON array of the classes the given student is enrolled in.
        /// Each object in the array should have the following fields:
        /// "subject" - The subject abbreviation of the class (such as "CS")
        /// "number" - The course number (such as 5530)
        /// "name" - The course name
        /// "season" - The season part of the semester
        /// "year" - The year part of the semester
        /// "grade" - The grade earned in the class, or "--" if one hasn't been assigned
        /// </summary>
        /// <param name="uid">The uid of the student</param>
        /// <returns>The JSON array</returns>a@
        public IActionResult GetMyClasses(string uid)
        {
            var classes = from enroll in db.Enrollments
                          where enroll.UserId == uid
                          join cls in db.Classes on enroll.ClassId equals cls.ClassId
                          join course in db.Courses on cls.CourseId equals course.CourseId
                          select new
                          {
                              subject = course.DSubject,
                              number = course.CNumber,
                              name = course.CName,
                              season = cls.SemSeason,
                              year = cls.SemYear,
                              grade = enroll.Grade
                          };

            return Json(classes.ToArray());
        }

        /// <summary>
        /// Returns a JSON array of all the assignments in the given class that the given student is enrolled in.
        /// Each object in the array should have the following fields:
        /// "aname" - The assignment name
        /// "cname" - The category name that the assignment belongs to
        /// "due" - The due Date/Time
        /// "score" - The score earned by the student, or null if the student has not submitted to this assignment.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="uid"></param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentsInClass(string subject, int num, string season, int year, string uid)
        {
            var query = from course in db.Courses
                        where course.CNumber == num && course.DSubject == subject
                        from cls in db.Classes
                        where cls.CourseId == course.CourseId && cls.SemSeason == season && cls.SemYear == year
                        from cat in db.AssignmentCategories
                        where cls.ClassId == cat.ClassId
                        from assg in db.Assignments
                        where assg.AcId == cat.AcId
                        join sub in db.Submissions
                        on new { A = assg.AId, B = uid } equals new { A = sub.AId, B = sub.UserId }
                        into asgns
                        from assignment in asgns.DefaultIfEmpty()
                        select new
                        {
                            aname = assg.Name,
                            cname = cat.AcName,
                            due = assg.DueDate,
                            score = assignment == null ? null : (uint?)assignment.Score
                        };

            return Json(query.ToArray());
        }



        /// <summary>
        /// Adds a submission to the given assignment for the given student
        /// The submission should use the current time as its DateTime
        /// You can get the current time with DateTime.Now
        /// The score of the submission should start as 0 until a Professor grades it
        /// If a Student submits to an assignment again, it should replace the submission contents
        /// and the submission time (the score should remain the same).
        /// Does *not* automatically reject late submissions.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The new assignment name</param>
        /// <param name="uid">The student submitting the assignment</param>
        /// <param name="contents">The text contents of the student's submission</param>
        /// <returns>A JSON object containing {success = true/false}.</returns>
        public IActionResult SubmitAssignmentText(string subject, int num, string season, int year,
          string category, string asgname, string uid, string contents)
        {
            Submissions toSubmit;
            var assignment = from course in db.Courses
                             where course.CNumber == num && course.DSubject == subject
                             from cls in db.Classes
                             where cls.CourseId == course.CourseId && cls.SemSeason == season && cls.SemYear == year
                             from cat in db.AssignmentCategories
                             where cls.ClassId == cat.ClassId && cat.AcName == category
                             from assg in db.Assignments
                             where assg.AcId == cat.AcId && assg.Name == asgname
                             select assg;

            //query for an existing submission
            var submission = from sub in db.Submissions
                             where sub.UserId == uid && sub.AId == assignment.First().AId
                             select sub;
            try
            {
                var now = DateTime.Now;
                //If there was a submission already, update the contents and time
                if (submission.Any())
                {
                    toSubmit = submission.First();
                    toSubmit.SubContents = contents;
                    toSubmit.SubTime = now;
                    db.Update(toSubmit);
                }
                //Otherwise, add a new submission with all of the data
                else
                {
                    toSubmit = new Submissions
                    {
                        SubContents = contents,
                        SubTime = now,
                        AId = assignment.First().AId,
                        UserId = uid,
                        Score = 0
                    };
                    db.Add(toSubmit);
                }
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }


        /// <summary>
        /// Enrolls a student in a class.
        /// </summary>
        /// <param name="subject">The department subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester</param>
        /// <param name="year">The year part of the semester</param>
        /// <param name="uid">The uid of the student</param>
        /// <returns>A JSON object containing {success = {true/false},
        /// false if the student is already enrolled in the Class.</returns>
        public IActionResult Enroll(string subject, int num, string season, int year, string uid)
        {
            var c = (from course in db.Courses
                     where course.DSubject == subject && course.CNumber == num
                     join cls in db.Classes on course.CourseId equals cls.CourseId
                     where cls.SemSeason == season && cls.SemYear == year
                     select cls).First();

            Enrollments e = new Enrollments { ClassId = c.ClassId, UserId = uid, Grade = "--" };
            try
            {
                db.Add(e);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        /// <summary>
        /// Calculates a student's GPA
        /// A student's GPA is determined by the grade-point representation of the average grade in all their classes.
        /// Assume all classes are 4 credit hours.
        /// If a student does not have a grade in a class ("--"), that class is not counted in the average.
        /// If a student does not have any grades, they have a GPA of 0.0.
        /// Otherwise, the point-value of a letter grade is determined by the table on this page:
        /// https://advising.utah.edu/academic-standards/gpa-calculator-new.php
        /// </summary>
        /// <param name="uid">The uid of the student</param>
        /// <returns>A JSON object containing a single field called "gpa" with the number value</returns>
        public IActionResult GetGPA(string uid)
        {
            var enrollments = from e in db.Enrollments
                              where e.UserId == uid
                              select e;

            double gpaNumerator = 0.0;
            int gpaDenominator = 0;
            foreach (var e in enrollments)
            {
                if (letterGPAs.ContainsKey(e.Grade))
                {
                    gpaNumerator += letterGPAs[e.Grade];
                    gpaDenominator += 1;
                }
            }
            var ret = new
            {
                gpa = gpaDenominator == 0 ? 0.0 : gpaNumerator / gpaDenominator
            };
            return Json(ret);
        }

        /*******End code to modify********/

    }
}