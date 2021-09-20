using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Submissions
    {
        public DateTime SubTime { get; set; }
        public uint Score { get; set; }
        public string SubContents { get; set; }
        public string UserId { get; set; }
        public uint AId { get; set; }
        public uint SubId { get; set; }

        public virtual Assignments A { get; set; }
        public virtual Students User { get; set; }
    }
}
