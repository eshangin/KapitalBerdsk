﻿using System.ComponentModel.DataAnnotations;

namespace KapitalBerdsk.Web.Classes.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = Resources.ResourceKeys.Required)]
        [StringLength(15, ErrorMessage = "{0} должен содержать от {2} до {1} символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повторите новый пароль")]
        [Compare("NewPassword", ErrorMessage = "Новый пароль и его подтверждение не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
