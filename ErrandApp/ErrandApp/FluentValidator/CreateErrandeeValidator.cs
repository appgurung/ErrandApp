using ErrandApp.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErrandApp.UI.FluentValidator
{

    public class CreateErrandeeValidator : AbstractValidator<CreateErrandee>
    {
        public CreateErrandeeValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email can not be blank");

            RuleFor(x => x.FullName)
              .NotEmpty()
              .WithMessage("Full name can not be blank");

            RuleFor(x => x.Gender)
              .NotEmpty()
              .WithMessage("Gender can not be blank");

            RuleFor(x => x.Location)
              .NotEmpty()
              .WithMessage("Location can not be blank");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password cannot be blank.")
                .Length(0, 4)
                .WithMessage("Password lenght too short")
                .Matches("Passsword")
                .WithMessage("Password must not contain password");


            RuleFor(x => x.PhoneNo)
               .NotEmpty()
               .WithMessage("Phone No cannot be blank.");


            RuleFor(x => x.UserName)
              .NotEmpty() 
              .WithMessage("User name cannot be blank.");

        }
    }
}