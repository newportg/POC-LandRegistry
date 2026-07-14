using System.ComponentModel.DataAnnotations;

namespace KnightFrank.Hub.LandRegistry.Fv
{
    public class SampleInput
    {
        public string Name { get; set; } = null!;

        public int Value { get; set; }

        public Class1? Class1 { get; set; }
        public Class2? Class2 { get; set; }
        public Class3? Class3 { get; set; }

    }

    public class Class1
    {
        [Required, MaxLength(10)]
        public string? Name { get; set; }
    }

    public class Class2
    {
        [Required]
        public int Count { get; set; }
    }

    public class Class3
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public float bob { get; set; }
    }

}
