using Lab_05.Models;
using Lab_05.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab_05.Controllers
{
    public class UsersController : Controller
    {
        private readonly NotePlannerDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager, NotePlannerDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        [Authorize(Roles = "admin")]
        public IActionResult Index(string userName, int page = 1)
        {
            int pageSize = 10;
            IQueryable<ApplicationUser> query = _userManager.Users;

            if (!string.IsNullOrEmpty(userName))
            {
                query = query.Where(u => u.UserName.Contains(userName));
            }

            List<ApplicationUser> applicationUsers = query.ToList();

            PageViewModel pageViewModel = new PageViewModel(applicationUsers.Count, page, pageSize);

            List<ApplicationUser> usersOnPage = applicationUsers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            FilterUsersViewModel filterNotesViewModel = new FilterUsersViewModel(userName);

            UsersViewModel usersViewModel = new UsersViewModel(usersOnPage, pageViewModel, filterNotesViewModel);

            return View(usersViewModel);
        }

        [Authorize(Roles = "admin")]
        public IActionResult CreateUser() => View();

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser newUser = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.Name,
                    Surname = model.Surname,
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditUser(string id)
        {
            ApplicationUser editUser = await _userManager.FindByIdAsync(id);

            if (editUser == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel
            {
                Id = editUser.Id,
                UserName = editUser.UserName,
                Email = editUser.Email,
                Name = editUser.FirstName,
                Surname = editUser.Surname,

            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.FirstName = model.Name;
                    user.Surname = model.Surname;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }

                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            ApplicationUser deleteUser = await _userManager.FindByIdAsync(id);

            if (deleteUser != null)
            {
                await _userManager.DeleteAsync(deleteUser);
            }
            return RedirectToAction("Index");
        }
    }
}