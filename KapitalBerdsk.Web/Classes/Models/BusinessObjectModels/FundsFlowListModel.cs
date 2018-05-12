using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KapitalBerdsk.Web.Classes.Models.BusinessObjectModels
{
    public class FundsFlowListModel
    {
        public IEnumerable<FundsFlowListItemModel> Items { get; set; }

        public IEnumerable<SelectListItem> Employees { get; set; }

        public IEnumerable<SelectListItem> BuildingObjects { get; set; }
    }
}
