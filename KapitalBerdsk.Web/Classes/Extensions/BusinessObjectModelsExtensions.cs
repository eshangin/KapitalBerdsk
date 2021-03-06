﻿using KapitalBerdsk.Web.Classes.Data;
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

        public static IQueryable<BuildingObject> ApplyOrder(this IQueryable<BuildingObject> items)
        {
            return items.OrderBy(item => item.Status)
                .ThenBy(item => item.OrderNumber)
                .ThenByDescending(item => item.Id);
        }

        public static IEnumerable<BuildingObject> ApplyOrder(this IEnumerable<BuildingObject> items)
        {
            return ApplyOrder(items.AsQueryable());
        }

        public static IQueryable<PdSection> ApplyOrder(this IQueryable<PdSection> items)
        {
            return items
                .OrderBy(item => item.OrderNumber)
                .ThenByDescending(item => item.Id);
        }

        public static IEnumerable<PdSection> ApplyOrder(this IEnumerable<PdSection> items)
        {
            return ApplyOrder(items.AsQueryable());
        }

        public static IQueryable<TEntity> OnlyActive<TEntity>(this IQueryable<TEntity> items)
            where TEntity : IInactivatable
        {
            return items.Where(item => !item.IsInactive);
        }
    }
}
