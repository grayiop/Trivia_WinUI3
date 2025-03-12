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
using ClosedXML.Excel;
using System.Diagnostics;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Trivia
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoadingPage : Page
    {
        public LoadingPage()
        {
            this.InitializeComponent();
        }

        private async void LoadXmlButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.Desktop;
            picker.FileTypeFilter.Add(".xlsx");
            picker.FileTypeFilter.Add(".xls");

            //// Get the current window's HWND
            var hwnd = WindowNative.GetWindowHandle(App.MainWindow);
            InitializeWithWindow.Initialize(picker, hwnd);

            StorageFile file = await picker.PickSingleFileAsync();
            InfoTextBlock.Text = "Question Set Loading";
            if (file != null)
            {
                using (var stream = await file.OpenStreamForReadAsync())
                {
                    using (var workbook = new XLWorkbook(stream))
                    {
                        var worksheet = workbook.Worksheets.First();
                        if (CheckQuestionSetFormatIntegrity(worksheet))
                        {
                            Debug.WriteLine("Question set format is correct");
                            LoadXmlButton.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green);
                            LoadQuestionSet(worksheet);
                            var questions = QuestionSetService.Instance.Questions;
                            InfoTextBlock.Text = $"{questions.Count} Questions Loaded";
                            StartButton.IsEnabled = true;
                        }
                        else
                        {
                            Debug.WriteLine("Question set format is incorrect");
                            InfoTextBlock.Text = "Question set format is incorrect";
                            LoadXmlButton.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red);
                            StartButton.IsEnabled = false;
                        }
                    }
                }
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GamePage));
        }

        private bool CheckQuestionSetFormatIntegrity(IXLWorksheet worksheet)
        {
            var indexTitle = worksheet.Cell("A1").Value.ToString();
            var questionTitle = worksheet.Cell("B1").Value.ToString();
            var numberOfAnswers = worksheet.Cell("C1").Value.ToString(); // Not being used for right now
            var correctAnswer = worksheet.Cell("D1").Value.ToString();
            var description = worksheet.Cell("E1").Value.ToString();
            var answerTitle1 = worksheet.Cell("F1").Value.ToString();
            var answerTitle2 = worksheet.Cell("G1").Value.ToString();
            var answerTitle3 = worksheet.Cell("H1").Value.ToString();
            var answerTitle4 = worksheet.Cell("I1").Value.ToString();

            if (indexTitle != "Index" || questionTitle != "Question" || numberOfAnswers != "# Answers" || description != "Description" || correctAnswer != "Correct Answer" || answerTitle1 != "Answer 1" || answerTitle2 != "Answer 2" || answerTitle3 != "Answer 3" || answerTitle4 != "Answer 4")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void LoadQuestionSet(IXLWorksheet worksheet)
        {
            var questions = new List<Question>();
            var rows = worksheet.RowsUsed().Skip(1); // Skip header row

            foreach (var row in rows)
            {
                var question = new Question
                {
                    Index = int.Parse(row.Cell(1).Value.ToString()),
                    QuestionText = row.Cell(2).Value.ToString(),
                    NumberOfAnswers = int.Parse(row.Cell(3).Value.ToString()),
                    CorrectAnswer = int.Parse(row.Cell(4).Value.ToString()),
                    Answers = new List<string>
                    {
                        row.Cell(5).Value.ToString(),
                        row.Cell(6).Value.ToString(),
                        row.Cell(7).Value.ToString(),
                        row.Cell(8).Value.ToString()
                    }
                };
                questions.Add(question);
            }
            QuestionSetService.Instance.LoadQuestions(questions);
        }
    }
}
