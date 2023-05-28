using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace invokertrainer
{

    public partial class ChoiseLvlPage : Page, INotifyPropertyChanged
    {
        private Stopwatch stopwatch;
        private List<string> combinations;
        private List<CombinationElement> icons;
        private ObservableCollection<CombinationElement> slots;
        private string currentCombination; // Изменено: использовать строку для текущей комбинации
        private int errorCount;
        private int errors;
        public int Errors
        {
            get { return errors; }
            set
            {
                errors = value;
                OnPropertyChanged(nameof(Errors));
            }
        }

        private int score;
        public int Score
        {
            get { return score; }
            set
            {
                score = value;
                OnPropertyChanged(nameof(Score));
            }
        }

        private string _targetCombination;
        public string TargetCombination
        {
            get { return _targetCombination; }
            set
            {
                _targetCombination = value;
                OnPropertyChanged(nameof(TargetCombination));
            }
        }

        private TimeSpan elapsedTime;
        public TimeSpan ElapsedTime
        {
            get { return elapsedTime; }
            set
            {
                elapsedTime = value;
                OnPropertyChanged(nameof(ElapsedTime));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class CombinationElement : INotifyPropertyChanged
        {
            private string icon;
            public string Icon
            {
                get { return icon; }
                set
                {
                    icon = value;
                    OnPropertyChanged(nameof(Icon));
                }
            }

            private string iconImageSource;
            public string IconImageSource
            {
                get { return iconImageSource; }
                set
                {
                    iconImageSource = value;
                    OnPropertyChanged(nameof(IconImageSource));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ChoiseLvlPage()
        {
            InitializeComponent();

            stopwatch = new Stopwatch();
            combinations = new List<string>
            {
                "qwe", "qqw", "qqq", "qqe", "www", "wwe", "wwq", "eee", "eew", "eeq"
            };




            DataContext = this;
            KeyDown += ChoiseLvlPage_KeyDown;
            PreviewKeyDown += ChoiseLvlPage_PreviewKeyDown;
            errorCount = 0;
            SetRandomTargetCombination();
        }

        private void SetRandomTargetCombination()
        {
            Random random = new Random();
            int index = random.Next(combinations.Count);
            TargetCombination = combinations[index];
        }

        private void StartTimer()
        {
            stopwatch.Start();
            CompositionTarget.Rendering += UpdateTimerText;
        }

        private void StopTimer()
        {
            stopwatch.Stop();
            CompositionTarget.Rendering -= UpdateTimerText;
        }

        private string GetCombination()
        {
            
            if (Keyboard.IsKeyDown(Key.Q))
                currentCombination += "q";
            if (Keyboard.IsKeyDown(Key.W))
                currentCombination += "w";
            if (Keyboard.IsKeyDown(Key.E))
                currentCombination += "e";
            return currentCombination;
        }


        private string _currentCombination;
        public string CurrentCombination
        {
            get { return _currentCombination; }
            set
            {
                _currentCombination = value;
                OnPropertyChanged(nameof(CurrentCombination));
            }
        }

        private void ChoiseLvlPage_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            {
                CurrentCombination = GetCombination(); // Обновить значение текущей комбинации
            }
        }

        private void ChoiseLvlPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R)
            {
                if (CurrentCombination == TargetCombination)
                {
                    Score++;
                    if (Score >= 10)
                    {
                        StopTimer();
                        ShowWinMessage();
                    }
                    else
                    {
                        SetRandomTargetCombination();
                        currentCombination = "";
                    }
                }
                else
                {
                    errorCount++;
                    MessageBox.Show($"Правильный ответ: {TargetCombination}\nВаш ответ: {CurrentCombination}");
                    if (errorCount >= 10)
                    {
                        StopTimer();
                        ShowLossMessage();
                    }
                    else
                    {
                        Errors = errorCount;
                        SetRandomTargetCombination();
                        currentCombination = "";
                    }
                }
            }
        }


        private void ShowWinMessage()
        {
            MessageBox.Show($"Congratulations! You won!\nScore: {Score}\nElapsed Time: {ElapsedTime.ToString(@"hh\:mm\:ss")}");
        }

        private void ShowLossMessage()
        {
            MessageBox.Show($"Game Over! You lost!\nScore: {Score}\nElapsed Time: {ElapsedTime.ToString(@"hh\:mm\:ss")}");
        }

        private void UpdateTimerText(object sender, EventArgs e)
        {
            ElapsedTime = stopwatch.Elapsed;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

            errorCount = 0;
            Score = 0;
            StartTimer();

        }
    }
}