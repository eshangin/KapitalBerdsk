﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Services
{
    public interface IPayEmployeePayrollService
    {
        Task PayToAllEmployees();
    }
}
