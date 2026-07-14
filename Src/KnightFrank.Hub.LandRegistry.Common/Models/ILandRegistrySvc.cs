using System.Threading.Tasks;

namespace KnightFrank.Hub.LandRegistry.Common.Models
{
    public interface ILandRegistrySvc
    {
        public Task<LandRegistryDto> FindProperty(LandRegistryDto property);
    }
}
