using System;
using BusinessLayer.Services.Abstractions;
using DataAccessLayer.Concrete;
using DataAccessLayer.UnitOfWorks;
using EntityLayer.Concrete;

namespace BusinessLayer.Services.Concrete
{
    public class SurveyService : ISurveyService
    {
        private readonly IUnitOfWork unitOfWork;

        public SurveyService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<Anket>> GetAllSurveysAsync()
        {
            var queryableAnkets = await unitOfWork.GetRepository<Anket>().GetAllAsync(includeProperties: a => a.Sorular);
            return queryableAnkets.ToList();
        }
        public async Task<int> AddSurveyAsync(Anket anket)
        {
            await unitOfWork.GetRepository<Anket>().AddAsync(anket);
            await unitOfWork.SaveAsync();
            return anket.Id;
        }

        public async Task<int> AddQuestionAsync(Soru soru)
        {
            await unitOfWork.GetRepository<Soru>().AddAsync(soru);
            await unitOfWork.SaveAsync();
            return soru.Id;
        }
        public async Task<bool> DeleteSurveyAsync(int surveyId)
        {
            var sorular = await unitOfWork.GetRepository<Soru>().GetAllAsync(s => s.AnketId == surveyId);
            foreach (var soru in sorular)
            {
                await unitOfWork.GetRepository<Soru>().DeleteAsync(soru);
            }

            var survey = await unitOfWork.GetRepository<Anket>().GetByIdAsync(surveyId);
            if (survey == null)
            {
                return false;
            }

            await unitOfWork.GetRepository<Anket>().DeleteAsync(survey);
            await unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateSurveyAsync(Anket anket)
        {
            var existingSurvey = await unitOfWork.GetRepository<Anket>().GetByIdAsync(anket.Id);

            if (existingSurvey == null)
            {
                return false;
            }

            existingSurvey.Baslik = anket.Baslik;
            existingSurvey.Sorular = anket.Sorular;

            await unitOfWork.GetRepository<Anket>().UpdateAsync(existingSurvey);
            await unitOfWork.SaveAsync();
            return true;
        }

        public async Task<Anket> GetSurveyByIdAsync(int surveyId)
        {
            return await unitOfWork.GetRepository<Anket>().GetAsync(x => x.Id == surveyId, includeProperties:y=>y.Sorular);
        }
        public async Task<bool> SubmitAnswersAsync(List<string> cevaplar, List<int> soruIds, int surveyId, string userId)
        {
            try
            {
                for (int i = 0; i < cevaplar.Count; i++)
                {
                    var answer = new Cevap
                    {
                        CevapMetni = cevaplar[i],
                        UserId = userId,
                        AnketId = surveyId,
                        SoruId = soruIds[i]
                    };

                    await unitOfWork.GetRepository<Cevap>().AddAsync(answer);
                }

                await unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<List<Cevap>> GetSurveyResponsesAsync()
        {
            var answers = await unitOfWork.GetRepository<Cevap>().GetAllAsync(includeProperties: c => c.Soru);
            var answerAll = await unitOfWork.GetRepository<Cevap>().GetAllAsync(includeProperties: c => c.User);
            var answerAllIncludes = await unitOfWork.GetRepository<Cevap>().GetAllAsync(includeProperties: c => c.Anket);

            return answerAllIncludes;
        }

    }
}

