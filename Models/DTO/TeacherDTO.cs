﻿using static ServiceStack.LicenseUtils;
using System.ComponentModel.DataAnnotations;

namespace timely_backend.Models.DTO
{
    public class TeacherDTO
    {
        [Required(ErrorMessage = "Необходимо указать ФИО")]
        [RegularExpression(@"^([А-ЯЁ][а-яё]+[\-\s]?){2,3}$",
        ErrorMessage = "ФИО должно состоять из 2-3 слов, начинаться с заглавной буквы и содержать только кириллические символы, дефисы и пробелы")]
        string Name { get; set; }
    }
}