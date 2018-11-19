using System.Collections.Generic;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Services;
using Microsoft.AspNetCore.Mvc;

namespace KapitalBerdsk.Web.Classes.ViewComponents
{
    public class NotificationArea : ViewComponent
    {
        private readonly IBuildingObjectClosingContractsChecker _buildingObjectClosingContractsChecker;

        public NotificationArea(
            IBuildingObjectClosingContractsChecker buildingObjectClosingContractsChecker)
        {
            _buildingObjectClosingContractsChecker = buildingObjectClosingContractsChecker;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<BuildingObject> model = await _buildingObjectClosingContractsChecker.GetBuildingObjectWithClosingContracts();
            return View(model);
        }
    }
}
