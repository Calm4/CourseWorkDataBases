using Lab_05;
using Lab_05.Models;
using Microsoft.AspNetCore.Identity;

namespace Lab_05
{
    public class DbUserInitializer
    {
        // Users
        private static string[] loginsVocabulary = { "kirieshka", "chelik", "mishutka", "pusya", "kingboss", "trisinaBOX" };
        private static string[] passwordsVocabulary = { "Pass_word", "pass_W_ord", "secreXD_", "pusya_usya_Rusya", "P_orolchik", "pASSssw_" };
        private static string[] emailVocabulary = { "boss.roto", "kesik.sos", "tuzik_puzo", "pupchanskiy", "trybochist", "kosvos", "alt_f4", "ctrl_atl_del" };
        private static string[] firstNameVocabulary = { "Данила_", "Кирилл_", "Сергей_", "Никита_", "Антон_", "Дмитрий_", "Иван_", "Максим_", "Богдан_", "Артем_" };
        private static string[] secondNameVocabulary = { "Буякевич_", "Громыко_", "Игнатенков_", "Гаращук_", "Ешман_", "Воянец_", "Кононович_", "Щербенко_", "Яковлев_", "Ленин_", "Сталин_", "Хрущев_" };

        private const int USERS_COUNT = 500;
        private const string USER_ROLE = "admin";

        public static async Task Initialize(IServiceProvider serviceProvider)
        {

            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));

                ApplicationUser admin = new ApplicationUser
                {
                    Email = "admin@gmail.com",
                    UserName = "admin",
                    FirstName = "Admin",
                    Surname = "admin",
                };

                var result = await userManager.CreateAsync(admin, "Pass_1");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, USER_ROLE);
                }
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            if (userManager.Users.ToList().Count > 1)
            {
                return;
            }


            Random randomObj = new Random();

            int userId;
            string userName;
            string password;
            string email;
            string name;
            string surname;





            for (userId = 1; userId <= USERS_COUNT; userId++)
            {
                userName = loginsVocabulary[randomObj.Next(loginsVocabulary.Length)] + userId;
                password = passwordsVocabulary[randomObj.Next(passwordsVocabulary.Length)] + userId;
                email = emailVocabulary[randomObj.Next(emailVocabulary.Length)] + userId + "@mail.ru";
                name = firstNameVocabulary[randomObj.Next(firstNameVocabulary.Length)] + userId;
                surname = secondNameVocabulary[randomObj.Next(secondNameVocabulary.Length)] + userId;

                ApplicationUser newUser = new ApplicationUser { UserName = userName, Email = email, FirstName = name, Surname = surname };
                IdentityResult result = await userManager.CreateAsync(newUser, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, "user");
                }
            }
        }
    }
}
