using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AnketHazirlamaPortali.Models;
using BusinessLayer.Services.Abstractions;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using AnketHazirlamaPortali.Views.Home;
using Microsoft.AspNetCore.Authorization;

namespace AnketHazirlamaPortali.Controllers;

public class HomeController : Controller
{
    private readonly ISurveyService surveyService;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IUserIdentityService _userIdentityService;

    public HomeController(ISurveyService surveyService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IUserIdentityService userIdentityService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        this.surveyService = surveyService;
        _userIdentityService = userIdentityService;
    }

    public IActionResult Index()
    {
        return View();
    }
    [Authorize]
    public async Task<IActionResult> Surveys()
    {
        var surveys = await surveyService.GetAllSurveysAsync();
        return View(surveys);
    }
    public async Task<IActionResult> SurveyDetails(int surveyId)
    {
        var survey = await surveyService.GetSurveyByIdAsync(surveyId);
        return View(survey);
    }
    [HttpPost]
    public async Task<IActionResult> SubmitAnswers(List<string> cevaplar, List<int> soruIds, int surveyId)
    {
        var userId = _userIdentityService.GetCurrentUserId();

        var success = await surveyService.SubmitAnswersAsync(cevaplar, soruIds, surveyId, userId);

        if (success)
        {
            return Json(new { redirectUrl = Url.Action("ThankYou", "Home", new { area = "" }) });
        }
        else
        {
            return Json(new { error = "Cevaplar kaydedilirken bir hata oluştu." });
        }
    }
    public IActionResult ThankYou()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.Sifre != model.ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Şifreler eşleşmiyor.";
                return View(model);
            }

            var user = new AppUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.ConfirmPassword);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(Models.LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.KullaniciAdi, model.Sifre, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.KullaniciAdi);

                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("SurveyList", "Home", new { area = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            TempData["ErrorMessage"] = "Geçersiz giriş denemesi.";

            return View(model);
        }

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home", new { area = "" });
    }
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}

