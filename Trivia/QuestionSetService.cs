using System.Collections.Generic;

namespace Trivia
{
    public class QuestionSetService
    {
        private static QuestionSetService _instance;
        public static QuestionSetService Instance => _instance ??= new QuestionSetService();

        public List<Question> Questions { get; private set; }

        private QuestionSetService()
        {
            Questions = new List<Question>();
        }

        public void LoadQuestions(List<Question> questions)
        {
            Questions = questions;
        }
    }

    public class Question
    {
        public int Index { get; set; }
        public string QuestionText { get; set; }
        public int NumberOfAnswers { get; set; }
        public List<string> Answers { get; set; }
        public int CorrectAnswer { get; set; }
        public string Description { get; set; }
    }
}