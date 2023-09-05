using MattRamageTrivia.Models.Data;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace MattRamageTrivia.Models.QuestionExportImport
{
    public class Import
    {
        public Import(IJSRuntime jSRuntime, TriviaRepository repo)
        {
            JSRuntime = jSRuntime;
            this.Repo = repo;

        }
        private IJSRuntime JSRuntime { get; set; }
        private TriviaRepository Repo { get; set; }
        public async Task ImportFile(InputFileChangeEventArgs e)
        {
            //Load XML File
            MemoryStream ms = new MemoryStream();
            try
            {
                await e.File.OpenReadStream(long.MaxValue).CopyToAsync(ms);
            }

            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", ex.Message); // Alert
            }

            ms.Position = 0;
            StreamReader reader = new StreamReader(ms);
            string text = await reader.ReadToEndAsync();

            //Deserialize XML
            List<Question>? questions = DeserializeXml(text);

            if (questions == null || questions.Count == 0)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Failed to load questions!"); // Alert
                return;
            }

            bool anyNewQuestionsPresent = await FindNewQuestions(questions);

            if (!anyNewQuestionsPresent)
                await JSRuntime.InvokeVoidAsync("alert", "Didn't Find any new questions to load!"); // Alert

            //UpdateSearchAndList();
        }

        private List<Question>? DeserializeXml(string text)
        {
            Type tyListQuestion = typeof(List<Question>);
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(tyListQuestion);
            List<Question>? questions = new List<Question>();

            using (StringReader stringReader = new StringReader(text))
            {
                questions = xmlSerializer.Deserialize(stringReader) as List<Question>;
            }

            return questions;
        }

        private async Task<bool> FindNewQuestions(List<Question> questions)
        {
            int newItems = 0;
            foreach (var question in questions)
            {
                try
                {
                    Question? existQuestion = GetExistingQuestion(question);

                    if (existQuestion == null)
                    {
                        newItems++;
                        SaveNewQuestion(question);
                    }
                }
                catch (Exception ex)
                {
                    await JSRuntime.InvokeVoidAsync("alert", ex.Message); // Alert
                }
            }

            if (newItems > 0)
            {
                await JSRuntime.InvokeVoidAsync("alert", $"Loaded {newItems} new questions!"); // Alert
            }

            return newItems != 0 ;
        }

        private Question? GetExistingQuestion(Question question)
        {
            var existQuestions = Repo.Questions
                .Where(q => (!string.IsNullOrEmpty(q.Text) && !string.IsNullOrEmpty(question.Text) && q.Text.ToLower() == question.Text.ToLower())
                    || (q.Image != null && question.Image != null & q.Image == question.Image))
                .ToList();

            Question? existQuestion = null;

            if (existQuestions.Count > 1)
            {
                existQuestion = GetMatchingQuestion(question, existQuestions);
            }
            else
            {
                existQuestion = existQuestions.FirstOrDefault();
            }

            return existQuestion;
        }

        private Question? GetMatchingQuestion(Question question, List<Question> existQuestions)
        {
            Question? matchingQuestion = null;

            foreach (var eQ in existQuestions)
            {
                var matchOptionCount = GetMatchingOptionCount(question, eQ);

                if (matchOptionCount == eQ.ListOptions.Count)
                {
                    matchingQuestion = eQ;
                    break;
                }
            }

            return matchingQuestion;
        }

        private int GetMatchingOptionCount(Question question, Question eQ)
        {
            int matchOptionCount = 0;

            //Validate Options
            var existOptions = Repo.QuestionOptions
                .Where(o => (o.QuestionId == eQ.Id))
                .ToList();

            if (existOptions.Count == question.ListOptions.Count)
            {
                for (int i = 0; i < existOptions.Count; i++)
                {
                    if (existOptions[i].IsAnswer == question.ListOptions[i].IsAnswer
                        && (!string.IsNullOrEmpty(existOptions[i].Text) && !string.IsNullOrEmpty(question.ListOptions[i].Text) && existOptions[i].Text == question.ListOptions[i].Text)
                        || (existOptions[i].Image != null && question.ListOptions[i].Image != null && existOptions[i].Image == question.ListOptions[i].Image))
                    {
                        matchOptionCount++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return matchOptionCount;
        }

        private void SaveNewQuestion(Question question)
        {
            question.Id = 0;
            question.Used = false;

            Repo.Questions.Add(question);
            Repo.SaveChanges();

            var optionsList = question.ListOptions;
            foreach (var option in optionsList)
            {
                option.QuestionId = question.Id;
            }
            Repo.QuestionOptions.AddRange(optionsList);
            Repo.SaveChanges();
        }

    }
}
