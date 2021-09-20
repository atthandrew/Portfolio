using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Courses
    {
        public Courses()
        {
            Classes = new HashSet<Classes>();
        }

        public string CName { get; set; }
        public ushort CNumber { get; set; }
        public string DSubject { get; set; }
        public uint CourseId { get; set; }

        public virtual Departments DSubjectNavigation { get; set; }
        public virtual ICollection<Classes> Classes { get; set; }
    }
}
