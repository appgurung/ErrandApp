using ErrandApp.UI.FluentValidator;
using FluentValidation.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErrandApp.Models
{
    [Validator(typeof(CreateErrandeeValidator))]
    public class CreateErrandee
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string PhoneNo { get; set; }

        public string Password { get; set; }

        public string Location { get; set; }

        public string Gender { get; set; }
    }

    [Validator(typeof(CreateErranderValidator))]
    public class CreateErrander
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        public string PhoneNo { get; set; }
    }
}