using ErrandApp.UI.FluentValidator;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErrandApp.Models
{
    [Validator(typeof(LoginValidator))]
    public class Login
    {
        public string Identity { get; set; }

        public string Password { get; set; }
    }
}