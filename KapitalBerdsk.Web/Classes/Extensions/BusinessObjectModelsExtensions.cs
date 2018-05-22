using KapitalBerdsk.Web.Classes.Data.Interfaces;
using KapitalBerdsk.Web.Classes.Models.BusinessObjectModels.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Extensions
{
    public static class BusinessObjectModelsExtensions
    {
        public static void VerifyEmployeeSelection(this IModelWithOneTimeEmployeeSelection model, 
            ModelStateDictionary modelState)
        {
            if (model.UseOneTimeEmployee)
            {
                if (string.IsNullOrWhiteSpace(model.OneTimeEmployeeName))
                {
                    modelState.AddModelError(nameof(model.EmployeeId), "Поле не может быть пустым");
                }
            }
            else
            {
                if (!model.EmployeeId.HasValue)
                {
                    modelState.AddModelError(nameof(model.EmployeeId), "Поле не может быть пустым");
                }
            }
        }

        public static void UpdateOrder(this IEnumerable<IOrderable> items, IEnumerable<int> idsWithNewOrder)
        {
            for (int i = 0; i < idsWithNewOrder.Count(); i++)
            {
                int id = idsWithNewOrder.ElementAt(i);
                IOrderable orderable = items.FirstOrDefault(item => item.Id == id);
                if (orderable != null)
                {
                    orderable.OrderNumber = i;
                }
            }
        }
    }
}
