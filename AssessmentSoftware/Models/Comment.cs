using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssessmentSoftware.Models
{
    public class Comment
    {
       
        [Required]
        public int CommentID { get; set; }
        public int ArticleID { get; set; }
        [DataType(DataType.MultilineText)]
        public string CommentBody { get; set; }
        public DateTime CommentDate { get; set; }
        public string CommentName { get; set; }
    }
}