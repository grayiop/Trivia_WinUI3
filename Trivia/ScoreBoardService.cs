using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivia
{
    public class ScoreBoardService
    {
        private static ScoreBoardService _instance;
        public static ScoreBoardService Instance => _instance ??= new ScoreBoardService();

        public Score Scores { get; private set; }

        public ScoreBoardService() {
            Scores = new Score();
        }
        public void AddCorrectAnswer()
        {
            Scores.NumberOfCorrectAnswers++;
            Scores.NumberOfQuestionsAnswered++;
        }
        public void AddIncorrectAnswer()
        {
            Scores.NumberOfIncorrectAnswers++;
            Scores.NumberOfQuestionsAnswered++;
        }

        public double GetScore()
        {
            if (Scores.NumberOfQuestionsAnswered <= 0)
            {
                return 0.0;
            } 
            else
            {
                return ((double)Scores.NumberOfCorrectAnswers / Scores.NumberOfQuestionsAnswered) * 100.0;
            }
        }
    }

    public class Score
    {
        public int NumberOfQuestions { get; set; }
        public int NumberOfQuestionsAnswered { get; set; }
        public int NumberOfCorrectAnswers { get; set; }
        public int NumberOfIncorrectAnswers { get; set; }
    }
}
