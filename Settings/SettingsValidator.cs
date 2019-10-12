using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blazor.CssBundler.Settings
{
    class SettingsValidator
    {
        public static List<ValidationResult> Validate(object settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            var results = new List<ValidationResult>();
            var context = new ValidationContext(settings);

            Validator.TryValidateObject(settings, context, results, true);
            return results;
        }
    }
}
