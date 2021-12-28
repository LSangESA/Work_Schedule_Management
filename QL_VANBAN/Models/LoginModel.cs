using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QL_VANBAN.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Vui lòng nhập UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Password")]
        public string Password { get; set; }
    }
}