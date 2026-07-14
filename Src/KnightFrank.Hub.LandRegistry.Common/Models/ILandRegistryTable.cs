using System.Collections.Generic;

namespace KnightFrank.Hub.LandRegistry.Common.Models
{
    public interface ILandRegistryTable
    {
        public List<LandRegistryDto> GetById(string drawNumber);

        public List<LandRegistryDto> Select(string filter = null);

        public int Insert(LandRegistryDto dto);

        public int Update(LandRegistryDto dto);

        public int Upsert(LandRegistryDto dto);

        public int Delete(LandRegistryDto dto);
    }
}
