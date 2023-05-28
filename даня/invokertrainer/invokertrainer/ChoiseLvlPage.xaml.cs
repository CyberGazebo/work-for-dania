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
    
    public partial class ChoiseLvlPage : Page
    {
        private Stopwatch stopwatch;
        private List<string> combinations;
        private List<string> Icons;
        public List<string> Slots { get; set; }
        private int errorCount;
        public class CombinationElement : INotifyPropertyChanged
        {
            private string _icon;
            public string Icon
            {
                get { return _icon; }
                set
                {
                    _icon = value;
                    OnPropertyChanged(nameof(Icon));
                }
            }

            private string _iconImageSource;
            public string IconImageSource
            {
                get { return _iconImageSource; }
                set
                {
                    _iconImageSource = value;
                    OnPropertyChanged(nameof(IconImageSource));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
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

            Icons = new List<string>
            {
                "/deafening_blast.png", "/ghost_walk.png", "/cold_snap.png", "/ice_wall.png", "/emp.png",
                "/alacrity.png", "/tornado.png", "/sun_strike.png", "/chaos_meteor.png", "/forge_spirit.png"
            };
            Slots = new ObservableCollection<CombinationElement>
    {
        new CombinationElement { Icon = "Icon1", IconImageSource = "/icon1.png" },
        new CombinationElement { Icon = "Icon2", IconImageSource = "/icon2.png" }
    };



            DataContext = this;
            KeyDown += MainWindow_KeyDown;
            errorCount = 0;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Q || e.Key == Key.W || e.Key == Key.E)
            {
                string currentCombination = GetCombination();
                if (combinations.Contains(currentCombination))
                {
                    int index = combinations.IndexOf(currentCombination);
                    if (index < 2)
                    {
                        string icon = Icons[index];
                        Slots[index] = icon;
                        errorCount = 0; // Сбрасываем счетчик ошибок при правильной комбинации
                    }
                }
                else
                {
                    errorCount++;
                    if (errorCount >= 3)
                    {
                        // Достигнуто максимальное количество ошибок, останавливаем таймер
                        stopwatch.Stop();
                        CompositionTarget.Rendering -= UpdateTimerText;
                    }
                }
            }
        }

        private string GetCombination()
        {
            string combination = "";
            if (Keyboard.IsKeyDown(Key.Q))
                combination += "q";
            if (Keyboard.IsKeyDown(Key.W))
                combination += "w";
            if (Keyboard.IsKeyDown(Key.E))
                combination += "e";
            return combination;
        }


        private void UpdateTimerText(object sender, EventArgs e)
        {
            TimeSpan elapsed = stopwatch.Elapsed;
            timerText.Text = string.Format("{0:00}:{1:00}:{2:00}", elapsed.Hours, elapsed.Minutes, elapsed.Seconds);
        }
    }
}

