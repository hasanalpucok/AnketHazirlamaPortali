using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AnketHazirlamaPortali.Areas.Admin.Extensions;
using AnketHazirlamaPortali.Areas.Admin.Models;
using BusinessLayer.Services.Abstractions;
using BusinessLayer.Services.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace AnketHazirlamaPortali.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ISurveyService surveyService;
        private readonly IToastNotification toastNotification;
        private readonly IUserService userService;

        public HomeController(ISurveyService surveyService, IToastNotification toastNotification,IUserService userService)
        {
            this.surveyService = surveyService;
            this.toastNotification = toastNotification;
            this.userService = userService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SurveyList()
        {
            var surveys = await surveyService.GetAllSurveysAsync();
            return View(surveys);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddSurvey()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddSurvey(AnketViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var anket = new Anket { Baslik = viewModel.Baslik };
                var anketId = await surveyService.AddSurveyAsync(anket);

                foreach (var soruMetni in viewModel.Sorular)
                {
                    var soru = new Soru { Metin = soruMetni, AnketId = anketId };
                    await surveyService.AddQuestionAsync(soru);
                }

                toastNotification.AddSuccessToastMessage("Anket ve sorular başarıyla eklenmiştir", new ToastrOptions { Title = "İşlem Başarılı" });

                // JSON nesnesi ile istemciye bilgi gönder
                return Json(new { redirectUrl = Url.Action(nameof(SurveyList), "Home", new { Area = "Admin" }) });
            }

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteSurvey(int id)
        {
            var isDeleted = await surveyService.DeleteSurveyAsync(id);

            if (isDeleted)
            {
                toastNotification.AddSuccessToastMessage("Anket başarıyla silinmiştir", new ToastrOptions { Title = "İşlem Başarılı" });
            }
            else
            {
                toastNotification.AddErrorToastMessage("Anket silinirken bir hata oluştu", new ToastrOptions { Title = "Hata" });
            }

            return RedirectToAction(nameof(SurveyList), "Home", new { Area = "Admin" });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditSurvey(int id)
        {
            var survey = await surveyService.GetSurveyByIdAsync(id);

            if (survey == null)
            {
                return NotFound();
            }

            var viewModel = new AnketEditViewModel
            {
                AnketId = survey.Id,
                AnketBaslik = survey.Baslik,
                Sorular = survey.Sorular != null
                    ? survey.Sorular.Select(s => new SoruViewModel { Id = s.Id, Metin = s.Metin }).ToList()
                    : new List<SoruViewModel>()
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditSurvey(AnketEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var anket = new Anket { Id = viewModel.AnketId, Baslik = viewModel.AnketBaslik, Sorular = viewModel.Sorular.Select(s => SoruViewModelToEntity(s)).ToList() };
                var result = await surveyService.UpdateSurveyAsync(anket);

                if (result)
                {
                    toastNotification.AddSuccessToastMessage("Anket ve sorular başarıyla güncellenmiştir", new ToastrOptions { Title = "İşlem Başarılı" });
                    return RedirectToAction(nameof(SurveyList), "Home", new { Area = "Admin" });
                }
            }

            return View(viewModel);
        }

        private Soru SoruViewModelToEntity(SoruViewModel soruViewModel)
        {
            return new Soru
            {
                Id = soruViewModel.Id,
                Metin = soruViewModel.Metin
            };
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListUsers()
        {
            var users = await userService.GetAllUsersAsync();
            return View(users);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SurveyResponses()
        {
            var responses = await surveyService.GetSurveyResponsesAsync();
            return View(responses);
        }
    }
}