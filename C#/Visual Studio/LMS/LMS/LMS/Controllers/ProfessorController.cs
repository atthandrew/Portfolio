using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Controllers
{
    [Authorize(Roles = "Professor")]
    public class ProfessorController : CommonController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Students(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
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

        public IActionResult Categories(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult CatAssignments(string subject, string num, string season, string year, string cat)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
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

        public IActionResult Submissions(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Grade(string subject, string num, string season, string year, string cat, string aname, string uid)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            ViewData["uid"] = uid;
            return View();
        }

        /*******Begin code to modify********/


        /// <summary>
        /// Returns a JSON array of all the students in a class.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "dob" - date of birth
        /// "grade" - the student's grade in this class
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetStudentsInClass(string subject, int num, string season, int year)
        {
            var query = from course in db.Courses
                        where course.DSubject == subject && course.CNumber == num
                        join cls in db.Classes on course.CourseId equals cls.CourseId
                        where cls.SemSeason == season && cls.SemYear == year
                        join enroll in db.Enrollments on cls.ClassId equals enroll.ClassId
                        join student in db.Students on enroll.UserId equals student.UserId
                        select new
                        {
                            fname = student.FName,
                            lname = student.LName,
                            uid = student.UserId,
                            dob = student.Dob,
                            grade = enroll.Grade
                        };

            return Json(query.ToArray());
        }



        /// <summary>
        /// Returns a JSON array with all the assignments in an assignment category for a class.
        /// If the "category" parameter is null, return all assignments in the class.
        /// Each object in the array should have the following fields:
        /// "aname" - The assignment name
        /// "cname" - The assignment category name.
        /// "due" - The due DateTime
        /// "submissions" - The number of submissions to the assignment
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class, 
        /// or null to return assignments from all categories</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentsInCategory(string subject, int num, string season, int year, string category)
        {
            var asgns = from course in db.Courses
                        where course.CNumber == num && course.DSubject == subject
                        from cls in db.Classes
                        where cls.CourseId == course.CourseId && cls.SemSeason == season && cls.SemYear == year
                        from cat in db.AssignmentCategories
                        where cls.ClassId == cat.ClassId
                        from assg in db.Assignments
                        where assg.AcId == cat.AcId
                        select new
                        {
                            aname = assg.Name,
                            cname = cat.AcName,
                            due = assg.DueDate,
                            submissions = assg.Submissions.Count
                        };

            if (category != null)
            {
                asgns = from catAsgn in asgns
                        where catAsgn.cname == category
                        select catAsgn;
            }

            return Json(asgns.ToArray());
        }


        /// <summary>
        /// Returns a JSON array of the assignment categories for a certain class.
        /// Each object in the array should have the following fields:
        /// "name" - The category name
        /// "weight" - The category weight
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentCategories(string subject, int num, string season, int year)
        {
            var cats = from course in db.Courses
                       where course.CNumber == num && course.DSubject == subject
                       from c in db.Classes
                       where c.CourseId == course.CourseId && c.SemSeason == season && c.SemYear == year
                       from cat in db.AssignmentCategories
                       where cat.ClassId == c.ClassId
                       select new
                       {
                           name = cat.AcName,
                           weight = cat.Weight
                       };

            return Json(cats.ToArray());
        }

        /// <summary>
        /// Creates a new assignment category for the specified class.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The new category name</param>
        /// <param name="catweight">The new category weight</param>
        /// <returns>A JSON object containing {success = true/false},
        ///	false if an assignment category with the same name already exists in the same class.</returns>
        public IActionResult CreateAssignmentCategory(string subject, int num, string season, int year, string category, int catweight)
        {
            var cls = (from course in db.Courses
                       where course.CNumber == num && course.DSubject == subject
                       from c in db.Classes
                       where c.CourseId == course.CourseId && c.SemSeason == season && c.SemYear == year
                       select c).First();

            AssignmentCategories cat = new AssignmentCategories { AcName = category, ClassId = cls.ClassId, Weight = (byte)catweight };

            try
            {
                db.Add(cat);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        /// <summary>
        /// Creates a new assignment for the given class and category.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The new assignment name</param>
        /// <param name="asgpoints">The max point value for the new assignment</param>
        /// <param name="asgdue">The due DateTime for the new assignment</param>
        /// <param name="asgcontents">The contents of the new assignment</param>
        /// <returns>A JSON object containing success = true/false,
        /// false if an assignment with the same name already exists in the same assignment category.</returns>
        public IActionResult CreateAssignment(string subject, int num, string season, int year, string category, string asgname, int asgpoints, DateTime asgdue, string asgcontents)
        {
            var _class = (from course in db.Courses
                          where course.CNumber == num && course.DSubject == subject
                          from c in db.Classes
                          where c.CourseId == course.CourseId && c.SemSeason == season && c.SemYear == year
                          select c).First();
            var cat = (from cats in db.AssignmentCategories
                       where cats.ClassId == _class.ClassId && cats.AcName == category
                       select cats).First();

            Assignments a = new Assignments { Name = asgname, AContents = asgcontents, AcId = cat.AcId, DueDate = asgdue, MaxPoints = (uint)asgpoints };

            try
            {
                db.Add(a);
                db.SaveChanges();
                var enrollments = from e in db.Enrollments
                                  where e.ClassId == _class.ClassId
                                  select e;
                foreach (var enr in enrollments)
                    UpdateSingleStudentGrades(enr.UserId, _class);

                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }


        /// <summary>
        /// Gets a JSON array of all the submissions to a certain assignment.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "time" - DateTime of the submission
        /// "score" - The score given to the submission
        /// 
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetSubmissionsToAssignment(string subject, int num, string season, int year, string category, string asgname)
        {
            var submissions = from course in db.Courses
                              where course.CNumber == num && course.DSubject == subject
                              from cls in db.Classes
                              where cls.CourseId == course.CourseId && cls.SemSeason == season && cls.SemYear == year
                              from cat in db.AssignmentCategories
                              where cls.ClassId == cat.ClassId && cat.AcName == category
                              from assg in db.Assignments
                              where assg.AcId == cat.AcId && assg.Name == asgname
                              from sub in db.Submissions
                              where sub.AId == assg.AId
                              from student in db.Students
                              where student.UserId == sub.UserId
                              select new
                              {
                                  fname = student.FName,
                                  lname = student.LName,
                                  uid = sub.UserId,
                                  time = sub.SubTime,
                                  score = sub.Score
                              };
            return Json(submissions.ToArray());
        }


        /// <summary>
        /// Set the score of an assignment submission
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <param name="uid">The uid of the student who's submission is being graded</param>
        /// <param name="score">The new score for the submission</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult GradeSubmission(string subject, int num, string season, int year, string category, string asgname, string uid, int score)
        {
            var _class = (from c in db.Courses
                          where c.CNumber == num && c.DSubject == subject
                          from cls in db.Classes
                          where cls.CourseId == c.CourseId && cls.SemSeason == season && cls.SemYear == year
                          select cls).First();

            var submission = from cat in db.AssignmentCategories
                             where _class.ClassId == cat.ClassId && cat.AcName == category
                             from assg in db.Assignments
                             where assg.AcId == cat.AcId && assg.Name == asgname
                             from sub in db.Submissions
                             where sub.AId == assg.AId
                             from student in db.Students
                             where student.UserId == sub.UserId && student.UserId == uid
                             select sub;

            if (!submission.Any())
            {
                return Json(new { success = false });
            }

            try
            {
                submission.First().Score = (uint)score;
                db.Submissions.Update(submission.First());
                db.SaveChanges();
                UpdateSingleStudentGrades(uid, _class);
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        /// <summary>
        /// Returns a JSON array of the classes taught by the specified professor
        /// Each object in the array should have the following fields:
        /// "subject" - The subject abbreviation of the class (such as "CS")
        /// "number" - The course number (such as 5530)
        /// "name" - The course name
        /// "season" - The season part of the semester in which the class is taught
        /// "year" - The year part of the semester in which the class is taught
        /// </summary>
        /// <param name="uid">The professor's uid</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetMyClasses(string uid)
        {
            var classes = from cls in db.Classes
                          where cls.ProfId == uid
                          join course in db.Courses on cls.CourseId equals course.CourseId
                          select new
                          {
                              subject = course.DSubject,
                              number = course.CNumber,
                              name = course.CName,
                              season = cls.SemSeason,
                              year = cls.SemYear
                          };
            return Json(classes.ToArray());
        }

        /// <summary>
        /// Class that stores some useful data for autograding, mainly created because it makes for easier reading compared to a new double[3]
        /// </summary>
        class CategoryScores
        {
            public double weight { get; set; }
            public double earnedPts { get; set; }
            public double totalPts { get; set; }
        }

        /// <summary>
        /// Calculates the grade for a particular student in a particular class, based off the formula discussed in the assignment specs.
        /// </summary>
        /// <param name="uid">The student id</param>
        /// <param name="_class">The class the student is enrolled in</param>
        private void UpdateSingleStudentGrades(string uid, Classes _class)
        {
            var enrollment = (from e in db.Enrollments
                              where e.ClassId == _class.ClassId && e.UserId == uid
                              select e).First();

            var categories = from cat in db.AssignmentCategories
                             where cat.ClassId == _class.ClassId
                             select cat;

            var assgns = from cat in categories
                         from assgn in db.Assignments
                         where assgn.AcId == cat.AcId
                         join sub in db.Submissions on new { A = assgn.AId, B = uid } equals new { A = sub.AId, B = sub.UserId }
                         into joined
                         from j in joined.DefaultIfEmpty()
                         select new
                         {
                             cat = cat.AcName,
                             weight = cat.Weight,
                             points = assgn.MaxPoints,
                             score = j == null ? null : (uint?)j.Score
                         };

            Dictionary<string, CategoryScores> categoryScores = new Dictionary<string, CategoryScores>();
            foreach (var score in assgns)
            {
                if (!categoryScores.ContainsKey(score.cat))
                    categoryScores.Add(score.cat,
                        new CategoryScores()
                        {
                            weight = score.weight,
                            earnedPts = score.score == null ? 0 : (double)score.score,
                            totalPts = score.points
                        });
                else
                {
                    categoryScores[score.cat].earnedPts += score.score == null ? 0 : (double)score.score;
                    categoryScores[score.cat].totalPts += score.points;
                }
            }

            double totalWgt = 0.0;
            double totalScore = 0.0;
            foreach (var cat in categoryScores.Keys)
            {
                totalWgt += categoryScores[cat].weight;
                totalScore += (categoryScores[cat].earnedPts / categoryScores[cat].totalPts) * categoryScores[cat].weight;
            }

            totalScore *= 100 / totalWgt;

            enrollment.Grade = getLetterGrade(totalScore);
            db.Update(enrollment);
            db.SaveChanges();
        }


        /// <summary>
        /// Gets the proper letter grade associated with a given score in the range [0.0, 100.0]
        /// </summary>
        private static string getLetterGrade(double score)
        {
            if (score >= 93.0)
                return "A";
            else if (score >= 90)
                return "A-";
            else if (score >= 87)
                return "B+";
            else if (score >= 83)
                return "B";
            else if (score >= 80)
                return "B-";
            else if (score >= 77)
                return "C+";
            else if (score >= 73)
                return "C";
            else if (score >= 70)
                return "C-";
            else if (score >= 67)
                return "D+";
            else if (score >= 63)
                return "D";
            else if (score >= 60)
                return "D-";
            else
                return "E";

        }
    }
}