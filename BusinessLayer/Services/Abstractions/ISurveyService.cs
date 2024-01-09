using System;
using EntityLayer.Concrete;

namespace BusinessLayer.Services.Abstractions
{
    public interface ISurveyService
    {
        Task<List<Anket>> GetAllSurveysAsync();
        Task<Anket> GetSurveyByIdAsync(int surveyId);
        Task<int> AddSurveyAsync(Anket anket);
        Task<int> AddQuestionAsync(Soru soru);
        Task<bool> DeleteSurveyAsync(int surveyId);
        Task<bool> UpdateSurveyAsync(Anket anket);
        Task<bool> SubmitAnswersAsync(List<string> cevaplar, List<int> soruIds, int surveyId, string userId);
        Task<List<Cevap>> GetSurveyResponsesAsync();

    }
}

