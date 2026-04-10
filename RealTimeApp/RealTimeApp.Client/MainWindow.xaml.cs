using Microsoft.AspNetCore.SignalR.Client;
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

namespace RealTimeApp.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection;

        public MainWindow()
        {
            InitializeComponent();

            connection = new HubConnectionBuilder().WithUrl("http://localhost:5219/chat").Build();

            connection.On<string, string>("Recive", (username, message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    var text = $"{username}: {message}";
                    messageList.Text = text;
                });
            });
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await connection.InvokeAsync("Send", usernameText.Text, messageText.Text);
            }
            catch (Exception ex)
            {
                messageList.Text = ex.Message;
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await connection.StartAsync();
            }
            catch (Exception ex) 
            {
                messageList.Text = $"{ex.Message}";
            }
        }
    }
}