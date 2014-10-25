using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using MediaResource.Web.DataAccess;

namespace MediaResource.Web.Models.CustomValidations
{
    public class UserValidation
    {
        public static ValidationResult ValidateNameNotDuplicate(string name)
        {
            if (!NameExists(name))
            {
                return ValidationResult.Success;
            }

            string errorMessage = String.Format("已经存在用户名为“{0}”的用户。", name);
            return new ValidationResult(errorMessage);
        }

        private static bool NameExists(string name)
        {
            using (var db = new ApplicationDbContext())
            {
                var users = from u in db.Users
                            where u.Name.ToLower() == name.Trim().ToLower()
                            select u;
                return users.Any();
            }
        }
    }
}