using System.Collections.Generic;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Data;

namespace KapitalBerdsk.Web.Classes.Services
{
    public interface IBuildingObjectClosingContractsChecker
    {
        Task Check();
        Task<List<BuildingObject>> GetBuildingObjectWithClosingContracts();
    }
}
