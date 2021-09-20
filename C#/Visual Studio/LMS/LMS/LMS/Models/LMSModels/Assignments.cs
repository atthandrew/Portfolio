using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Assignments
    {
        public Assignments()
        {
            Submissions = new HashSet<Submissions>();
        }

        public string Name { get; set; }
        public uint MaxPoints { get; set; }
        public string AContents { get; set; }
        public DateTime DueDate { get; set; }
        public uint AId { get; set; }
        public uint AcId { get; set; }

        public virtual AssignmentCategories Ac { get; set; }
        public virtual ICollection<Submissions> Submissions { get; set; }
    }
}
