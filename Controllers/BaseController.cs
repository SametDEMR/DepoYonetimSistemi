using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

public class BaseController : Controller
{
    public int? GetLoggedInUserId()
    {
        return HttpContext.Session.GetInt32("UserID");
    }

    public int? GetLoggedInDepoId()
    {
        return HttpContext.Session.GetInt32("DepoId");
    }
}
