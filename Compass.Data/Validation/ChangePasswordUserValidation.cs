using Compass.Data.Data.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compass.Data.Validation
{
    public class ChangePasswordUserValidation : AbstractValidator<ChangePasswordUserVM>
    {
        public ChangePasswordUserValidation()
        {
            RuleFor(c => c.Email).NotEmpty();
            RuleFor(c => c.NewPassword).NotEmpty().MinimumLength(6);
            RuleFor(c => c.OldPassword).NotEmpty().MinimumLength(6);
        }
    }
}
