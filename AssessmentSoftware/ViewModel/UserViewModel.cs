using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AssessmentSoftware.ViewModel
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string IsAdmin { get; set; }
        public string IsStuspened { get; set; }
    }
}