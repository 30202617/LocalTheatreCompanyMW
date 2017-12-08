using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AssessmentSoftware.Models
{
    public class Article
    {
        
        [Required]
        public int ArticleID { get; set; }
        public string ArticleTitle { get; set; }
        [DataType(DataType.MultilineText)]
        public string ArticleDescription { get; set; }
        public DateTime ArticleDate { get; set; }
        public string UserName { get; set; }
        public virtual List<Comment> Comments { get; set; }


    }
}

