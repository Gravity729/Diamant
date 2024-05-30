using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Diamant.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string LName { get; set; } = null!;

    public string FName { get; set; } = null!;

    public string? PName { get; set; }

    public DateOnly BDate { get; set; }

    public string Phone { get; set; } = null!;

    public string SPassport { get; set; } = null!;

    public string NPassport { get; set; } = null!;

    public string LoginE { get; set; } = null!;

    public string PasswordE { get; set; } = null!;

    public string FullNameEmployee => $"{LName} {FName} {PName}";
    public string ShortNameEmployee => $"{LName} {FName[0]}. {PName?[0]}.";
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    //0 - авторизация неуспешна, ошибка в MessageBox'е
    //1 - авторизация от имени сотрудника
    //2 - авторизация от имени администратора
    //3 - авторизация неуспешна, логин некорректен
    //4 - авторизация неуспешна, пароль некорректен
    public static byte CheckAuthorization(string login, string password, ref Employee employee, out string errorMessage)
    {
        string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^])[A-Za-z\d!@#$%^]{6,}$";
        if (login.Length < 5)
        {
            errorMessage = "Поле логина введено не корректно.";
            return 3;
        }

        else if (string.IsNullOrWhiteSpace(password) || !Regex.IsMatch(password, passwordPattern))
        {
            errorMessage = "Поле пароля введено не корректно.";
            return 4;
        }

        else
        {
            errorMessage = "";

            if (login == "*admin734" && password == "Q!werty2")
            {
                return 2;
            }

            else if (login != null)
            {
                using (PawnshopContext context = new PawnshopContext())
                {
                    employee = context.Employees.Where(t => t.LoginE == login && t.PasswordE == password).FirstOrDefault();
                }
                if (employee != null)
                {
                    return 1;
                }
                else
                {
                    errorMessage = "Логин или пароль введен неверно!";
                    return 0;
                }
            }
            else
            {
                errorMessage = "Логин или пароль введен неверно!";
                return 0;
            }
        }
    }
}
