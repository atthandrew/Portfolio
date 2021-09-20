using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Students
    {
        public Students()
        {
            Enrollments = new HashSet<Enrollments>();
            Submissions = new HashSet<Submissions>();
        }

        public string UserId { get; set; }
        public string DSubject { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public DateTime Dob { get; set; }

        public virtual Departments DSubjectNavigation { get; set; }
        public virtual ICollection<Enrollments> Enrollments { get; set; }
        public virtual ICollection<Submissions> Submissions { get; set; }
    }
}
