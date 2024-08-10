using Microsoft.AspNetCore.Identity;
using Store.Core.Entities.Identity;

namespace Store.Core.Services.Contract;

public interface IAuthService
{
    Task<string> CreateTokenAsync(AppUser user,UserManager<AppUser> userManager);
}
