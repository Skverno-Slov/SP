using System.IO;
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
using System.Windows.Threading;

namespace LabWork24
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> _imageFiles = new List<string>();
        int _currentIndex = 0;
        DispatcherTimer _ImageTimer;
        DispatcherTimer _timer;

        double _width; 
        double _height;

        double _stepX = 2;
        double _stepY = 2;

        Point? _lastPosition;

        public MainWindow()
        {
            InitializeComponent();
            LoadImages();
        }

        void LoadImages()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

            _imageFiles = Directory.GetFiles(path).ToList();
        }

        async Task Start()
        {
            ShowImage();
            _height = App.Current.MainWindow.Height;
            _width = App.Current.MainWindow.Width;

            Canvas.SetLeft(TimeTextBloxk, (_width - TimeTextBloxk.ActualWidth) / 2);
            Canvas.SetTop(TimeTextBloxk, (_height - TimeTextBloxk.ActualHeight) / 2);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(16);
            _timer.Tick += (s, e) => UpdateClock();
            _timer.Start();

            _ImageTimer = new DispatcherTimer();
            _ImageTimer.Interval = TimeSpan.FromSeconds(3);
            _ImageTimer.Tick += (s, e) => ShowImage();
            _ImageTimer.Start();
        }

        void UpdateClock()
        {
            TimeTextBloxk.Text = DateTime.Now.ToString("HH:mm:ss");

            double nextStepX = Canvas.GetLeft(TimeTextBloxk) + _stepX;
            double nextStepY = Canvas.GetTop(TimeTextBloxk) + _stepY;

            if (nextStepX <= 0 || TimeTextBloxk.ActualWidth + nextStepX >= _width)
                _stepX = -_stepX;

            if (nextStepY <= 0 || TimeTextBloxk.ActualHeight + nextStepY >= _height)
                _stepY = -_stepY;

            Canvas.SetTop(TimeTextBloxk, nextStepY);
            Canvas.SetLeft(TimeTextBloxk, nextStepX);
        }

        private void ShowImage()
        {
            if (_currentIndex >= _imageFiles.Count)
                _currentIndex = 0;

            var uri = new Uri(_imageFiles[_currentIndex]);
            BackGroundImage.Source = new BitmapImage(uri);

            _currentIndex++;
        }

        void CloseWindowByMove(MouseEventArgs e)
        {
            var position = e.GetPosition(this);

            if (_lastPosition is not null)
            {
                double deltaX = position.X - _lastPosition.Value.X;
                double deltaY = position.Y - _lastPosition.Value.Y;

                if (deltaX + deltaY > 5)
                {
                    Close();
                }
            }

            _lastPosition = position;
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            CloseWindowByMove(e);
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Start();
        }
    }
}