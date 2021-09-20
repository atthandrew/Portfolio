using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Enrollments
    {
        public string Grade { get; set; }
        public uint ClassId { get; set; }
        public string UserId { get; set; }
        public uint EId { get; set; }

        public virtual Classes Class { get; set; }
        public virtual Students User { get; set; }
    }
}
