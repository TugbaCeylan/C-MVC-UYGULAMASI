using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCOneMagic.Models
{
    public class CommentDTO
    {
        public string FirsName { get; set; }
        public string LastName { get; set; }
        public string Text { get; set; }
        public DateTime PublishDate { get; set; }
    }
}