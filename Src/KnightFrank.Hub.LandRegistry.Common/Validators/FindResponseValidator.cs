using FluentValidation;
using KnightFrank.Hub.LandRegistry.Common.Models;

namespace KnightFrank.Hub.LandRegistry.Common.Validators
{
    public class FindResponseValidator : AbstractValidator<FindResponse>
    {
        public FindResponseValidator()
        {
            //RuleFor(x => x.BallTotalAvg).GreaterThan(0);
            //RuleFor(x => x.NumBallDrawings).NotNull();
            //RuleFor(x => x.OddBalls).NotNull();
            //RuleFor(x => x.RenatoGianellaOccurrance).NotNull();
            //RuleFor(x => x.Delta).NotNull();

        }
    }
}
