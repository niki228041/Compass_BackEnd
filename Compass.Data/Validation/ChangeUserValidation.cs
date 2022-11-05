using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compass.Data.Data.ViewModels;

namespace Compass.Data.Validation
{
    public class ChangeUserValidation : AbstractValidator<ChangeUserVM>
    {
        public ChangeUserValidation()
        {
            RuleFor(c => c.Username).NotEmpty();
            RuleFor(c => c.Surname).NotEmpty();
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Email).EmailAddress();
        }
    }
}
