using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCoreIdentityApp.Web.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddModelErrorList(this ModelStateDictionary ModalState,List<String> errors)
        {
            errors.ForEach(x => ModalState.AddModelError(string.Empty, x));
        }

        public static void AddModelErrorList(this ModelStateDictionary ModalState, IEnumerable<IdentityError> errors)
        {
            errors.ToList().ForEach(x => ModalState.AddModelError(string.Empty, x.Description));
        }
    }
}
