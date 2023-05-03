using ClosedXML.Excel;
using MarketPlace.Interfaces;
using MarketPlace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static MarketPlace.Helpers.Roles;

namespace MarketPlace.Controllers;
public class AdminController : Controller
{
    private readonly IAdminRepository _adminRepository;
    private readonly UserManager<ApplicationUsers> _userManager;

    public AdminController(IAdminRepository adminRepository, UserManager<ApplicationUsers> userManager)
    {
        _adminRepository = adminRepository;
        _userManager = userManager;
    }
    public async Task<IActionResult> ViewAllUsers()
    {
        var users = await _adminRepository.GetAllUsersAsync();

        var userRoles = new List<(ApplicationUsers users, string roles)>();

        foreach (var user in users)
        {
            var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            userRoles.Add((user, role));
        }
        return View(userRoles);
    }
    public IActionResult AddCategory()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AddCategory(Categories category)
    {
        await _adminRepository.AddCategoryAsync(category);
        return RedirectToAction("ViewAllCategories");
    }
    public async Task<IActionResult> ViewAllCategories()
    {
        var categories = await _adminRepository.GetAllCategoriesAsync();
        return View(categories);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteCategory(Categories category)
    {
        await _adminRepository.DeleteCategoryAsync(category);
        return RedirectToAction("ViewAllCategories");
    }
    [HttpPost]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _adminRepository.GetUserByIdAsync(id);
        await _adminRepository.DeleteUserAsync(user);
        return RedirectToAction("ViewAllUsers", "Admin");
    }
    public async Task<IActionResult> ViewAllAdminUsers()
    {
        var admins = await _adminRepository.GetAllAdminsAsync();
        return View(admins);
    }
    [HttpPost]
    public async Task<IActionResult> FindUser(string username)
    {
        var model = new List<(ApplicationUsers user, string role)>();
        if (username == null)
        {
            TempData["EmptyInput"] = "Please enter username in input field!";
            return RedirectToAction("ViewAllUsers");
        }

        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            TempData["NoUser"] = "User with such username wasn't found!";
            return RedirectToAction("ViewAllUsers");
        }

        var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
        model.Add((user, role));
        return View("ViewAllUsers", model);
    }
    [HttpPost]
    public async Task<IActionResult> MakeAnAdmin(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            return RedirectToAction("ViewAllUsers");
        }
        await _userManager.AddToRoleAsync(user, Role.Admin);
        await _userManager.RemoveFromRoleAsync(user, Role.User);
        await _adminRepository.SaveAsync();
        return RedirectToAction("ViewAllAdminUsers");
    }
    [HttpPost]
    public async Task<IActionResult> ExportUsers()
    {
        using (var workBook = new XLWorkbook(XLEventTracking.Disabled))
        {
            var users = await _adminRepository.GetAllUsersAsync();
            var worksheet = workBook.Worksheets.Add("All users");
            worksheet.Cell("A1").Value = "Id";
            worksheet.Cell("B1").Value = "Username";
            worksheet.Cell("C1").Value = "Email";
            worksheet.Cell("D1").Value = "First name";
            worksheet.Cell("E1").Value = "Last name";
            worksheet.Cell("F1").Value = "Phone number";

            for (int i = 1; i <= users.Count; i++)
            {
                worksheet.Cell(i + 1, 1).Value = users[i - 1].Id;
                worksheet.Cell(i + 1, 2).Value = users[i - 1].UserName;
                worksheet.Cell(i + 1, 3).Value = users[i - 1].Email;
                worksheet.Cell(i + 1, 4).Value = users[i - 1].FirstName;
                worksheet.Cell(i + 1, 5).Value = users[i - 1].LastName;
                worksheet.Cell(i + 1, 6).Value = users[i - 1].PhoneNumber;
            }
            using (var stream = new MemoryStream())
            {
                workBook.SaveAs(stream);
                await stream.FlushAsync();
                return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = $"Users_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                };
            }
        }
    }

    public IActionResult AddPayment()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AddPayment(PaymentType type)
    {
        if (!ModelState.IsValid)
        {
            return View(type);
        }
        await _adminRepository.AddPaymentTypeAsync(type);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult AddShipping()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AddShipping(ShippingType type)
    {
        if (!ModelState.IsValid)
        {
            return View(type);
        }
        await _adminRepository.AddShippingTypeAsync(type);
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> ViewAllPaymentTypes()
    {
        var paymentTypes = await _adminRepository.GetAllPaymentTypesAsync();
        return View(paymentTypes);
    }
    [HttpPost]
    public async Task<IActionResult> DeletePaymentType(PaymentType type)
    {
        await _adminRepository.DeletePaymentTypeAsync(type);
        return RedirectToAction("ViewAllPaymentTypes");
    }

    public async Task<IActionResult> ViewAllShippingTypes()
    {
        var shippingTypes = await _adminRepository.GetAllShippingTypesAsync();
        return View(shippingTypes);
    }
    [HttpPost]
    public async Task<IActionResult> DeleteShippingType(ShippingType type)
    {
        await _adminRepository.DeleteShippingTypeAsync(type);
        return RedirectToAction("ViewAllShippingTypes");
    }
}
