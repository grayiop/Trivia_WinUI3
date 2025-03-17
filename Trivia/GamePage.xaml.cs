using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Trivia
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private int userSelectedAnswer = -1;
        private List<Question> questions = QuestionSetService.Instance.Questions;
        private Score scores = ScoreBoardService.Instance.Scores;
        private int question_index = 0;
        private bool isSubmited = false;
        public GamePage()
        {
            this.InitializeComponent();
            LoadQuestion();

        }
        private void Answer1_Click(object sender, RoutedEventArgs e)
        {
            SelectButtons(Answers1);
            userSelectedAnswer = 1;
        }
        private void Answer2_Click(object sender, RoutedEventArgs e)
        {
            SelectButtons(Answers2);
            userSelectedAnswer = 2;
        }
        private void Answer3_Click(object sender, RoutedEventArgs e)
        {
            SelectButtons(Answers3);
            userSelectedAnswer = 3;
        }
        private void Answer4_Click(object sender, RoutedEventArgs e)
        {
            SelectButtons(Answers4);
            userSelectedAnswer = 4;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (isSubmited)
            {
                NextQuestion();
                Description.Text = "";
            } else
            {
                SubmitQuestion();
            }
            isSubmited = !isSubmited;
        }
        private void SubmitQuestion()
        {
            ResetButtonsColor();
            switch (questions[question_index].CorrectAnswer)
            {
                case 1:
                    Answers1.Background = new SolidColorBrush(Microsoft.UI.Colors.Green);
                    break;
                case 2:
                    Answers2.Background = new SolidColorBrush(Microsoft.UI.Colors.Green);
                    break;
                case 3:
                    Answers3.Background = new SolidColorBrush(Microsoft.UI.Colors.Green);
                    break;
                case 4:
                    Answers4.Background = new SolidColorBrush(Microsoft.UI.Colors.Green);
                    break;
            }
            if (userSelectedAnswer != questions[question_index].CorrectAnswer)
            {
                ScoreBoardService.Instance.AddIncorrectAnswer();
                switch (userSelectedAnswer)
                {
                    case 1:
                        Answers1.Background = new SolidColorBrush(Microsoft.UI.Colors.Red);
                        break;
                    case 2:
                        Answers2.Background = new SolidColorBrush(Microsoft.UI.Colors.Red);
                        break;
                    case 3:
                        Answers3.Background = new SolidColorBrush(Microsoft.UI.Colors.Red);
                        break;
                    case 4:
                        Answers4.Background = new SolidColorBrush(Microsoft.UI.Colors.Red);
                        break;
                }
            } 
            else
            {
                ScoreBoardService.Instance.AddCorrectAnswer();
            }
            Score.Text = $"{ScoreBoardService.Instance.GetScore():F0}";
            Description.Text = questions[question_index].Description;
            Submit.Content = "Next";
        }
        private void NextQuestion()
        {
            question_index++;
            LoadQuestion();
            Submit.IsEnabled = false;
            Submit.Content = "Submit";
        }

        private void LoadQuestion()
        {
            ResetButtonsColor();
            Question.Text = questions[question_index].QuestionText;
            Answers1.Content = questions[question_index].Answers[0];
            Answers2.Content = questions[question_index].Answers[1];
            Answers3.Content = questions[question_index].Answers[2];
            Answers4.Content = questions[question_index].Answers[3];
            Index.Text = $"{question_index} / {questions.Count}";
        }
        private void SelectButtons(Button selectedButton)
        {
            if (isSubmited)
            {
                return;
            }
            ResetButtonsColor();
            selectedButton.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Gray);
            selectedButton.Background = new SolidColorBrush(Microsoft.UI.Colors.White);
            Submit.IsEnabled = true;
        }

        private void ResetButtonsColor()
        {
            Answers1.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
            Answers1.Background = new SolidColorBrush(Microsoft.UI.Colors.Gray);
            Answers2.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
            Answers2.Background = new SolidColorBrush(Microsoft.UI.Colors.Gray);
            Answers3.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
            Answers3.Background = new SolidColorBrush(Microsoft.UI.Colors.Gray);
            Answers4.Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
            Answers4.Background = new SolidColorBrush(Microsoft.UI.Colors.Gray);
        }
    }
}
