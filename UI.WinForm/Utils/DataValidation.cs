using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UI.WinForm
{
    public class DataValidation
    {
        private ValidationContext context;
        private List<ValidationResult> results;
        private bool valid;
        private string message;

        public DataValidation(object instance)
        {
            context = new ValidationContext(instance);
            results = new List<ValidationResult>();
            valid = Validator.TryValidateObject(instance, context, results, true);
        }
        public bool Validate()
        {
            if (valid == false)
            {
                foreach (ValidationResult item in results)
                {
                    message += item.ErrorMessage + "\n";
                }
                System.Windows.Forms.MessageBox.Show(message);
            }
            return valid;
        }
    }
}
