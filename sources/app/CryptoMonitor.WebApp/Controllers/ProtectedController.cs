using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoMonitor.WebApp.Controllers
{
    [Authorize]
    public abstract class ProtectedController : Controller
    {
        internal string UserId
        {
            get
            {
                if (User.Identity is { IsAuthenticated: true })
                {
                    var claim = User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier);
                    return claim.Value;
                }

                return null;
            }
        }
    }
}