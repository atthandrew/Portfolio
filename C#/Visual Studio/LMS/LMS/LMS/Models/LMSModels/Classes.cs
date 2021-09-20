using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Classes
    {
        public Classes()
        {
            AssignmentCategories = new HashSet<AssignmentCategories>();
            Enrollments = new HashSet<Enrollments>();
        }

        public string SemSeason { get; set; }
        public ushort SemYear { get; set; }
        public uint CourseId { get; set; }
        public uint ClassId { get; set; }
        public string Location { get; set; }
        public TimeSpan STime { get; set; }
        public TimeSpan ETime { get; set; }
        public string ProfId { get; set; }

        public virtual Courses Course { get; set; }
        public virtual Professors Prof { get; set; }
        public virtual ICollection<AssignmentCategories> AssignmentCategories { get; set; }
        public virtual ICollection<Enrollments> Enrollments { get; set; }
    }
}
