using Microsoft.AspNetCore.Identity;
using Store.Core.Entities.Identity;

namespace Store.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if(userManager.Users.Count()==0)
            {
                var user = new AppUser()
                {
                    DisplayName = "Ahmed Mohamed",
                    Email = "ahmedmohamed@gmail.com",
                    UserName = "Elshafey",
                    PhoneNumber = "01028984165",
                };
                await userManager.CreateAsync(user,"Ahmed@12");
            }
        }
    }
}
