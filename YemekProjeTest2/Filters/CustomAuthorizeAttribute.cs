using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;

    public CustomAuthorizeAttribute(string roles)
    {
        _roles = roles.Split(','); // Virgülle ayrılmış rolleri bir diziye dönüştürün
    }

    public CustomAuthorizeAttribute()
    {
        _roles = null; // Yalnızca giriş yapmış kullanıcıları kontrol etmek isterseniz
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userId = context.HttpContext.Session.GetInt32("UserId"); // Kullanıcının ID'sini alın

        // Giriş yapmış bir kullanıcı yoksa, hata sayfasına yönlendir
        if (userId == null)
        {
            context.Result = new RedirectToActionResult("Error", "Home", null);
            return;
        }

        // Roller belirtilmişse, kullanıcının rolünü kontrol et
        if (_roles != null)
        {
            var userRole = context.HttpContext.Session.GetString("Role");
            if (!_roles.Contains(userRole))
            {
                context.Result = new RedirectToActionResult("Error", "Home", null);
            }
        }
    }
}
