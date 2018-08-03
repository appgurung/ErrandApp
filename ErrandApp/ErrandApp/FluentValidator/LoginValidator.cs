using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErrandApp.Models;
using FluentValidation;

namespace ErrandApp.UI.FluentValidator
{
    public class LoginValidator : AbstractValidator<Login>  
    {
        public LoginValidator()
        {
            RuleFor(x => x.Identity)
                .NotEmpty()
                .WithMessage("Identity can not be blank");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password cannot be blank.");


        }
    }
}