using FluentValidation;

namespace KnightFrank.Hub.LandRegistry.Fv
{
    public class SampleInputValidator : AbstractValidator<SampleInput>
    {
        public SampleInputValidator()
        {
            RuleFor(model => model.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(10)
                .MinimumLength(5);
            RuleFor(model => model.Value)
                .GreaterThan(0);
            RuleFor(model => model)
                .Must(HasOneClassField)
                .WithMessage("There can be only ONE Class");
        }

        private bool HasOneClassField(SampleInput model)
        {
            bool rtn = false;
            if( model.Class1 != null)
            {
                rtn = true;
            }
            if (model.Class2 != null)
            {
                if (rtn == true) return false;
                rtn = true;
            }
            if( model.Class3 != null)
            {
                if (rtn == true) return false;
                rtn = true;
            }

            return rtn;
        }
    }
}
