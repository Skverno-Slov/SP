using Microsoft.AspNetCore.SignalR.Client;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LabWork27
{
    public partial class MainWindow : Window
    {
        private HubConnection connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5035/chat")
            .WithAutomaticReconnect()
            .Build();

        private string user = string.Empty;
        private string room = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
            => await SendMessageAsync();

        private async void EnterButton_Click(object sender, RoutedEventArgs e) 
            => await LoginUser();

        private async Task LoginUser()
        {
            var userName = LoginTextBox.Text.Trim();
            var roomName = RoomTextBox.Text.Trim();

            if (String.IsNullOrEmpty(userName))
                MessageBox.Show("Ошибка", "Введите логин");

            if (String.IsNullOrEmpty(roomName))
                MessageBox.Show("Ошибка", "Введите название комнаты");

            user = userName;
            room = roomName;
            RoomNameTextBox.Text = $"{room} {user}";
            ChatStackPanel.Visibility = Visibility.Visible;
            LoginStackPanel.Visibility = Visibility.Collapsed;

            await InitializeConnection();
        }

        private async Task InitializeConnection()
        {
            connection.On<string>("ReseiveMessage", (message) =>
            {
                Dispatcher.InvokeAsync(() =>
                {
                    MessagesListBox.Items.Add(message);
                });
            });

            try
            {
                await connection.StartAsync();

                await connection.InvokeAsync("JoinRoom", room, user);
                MessagesListBox.Items.Add("Подключение выполнено");
            }
            catch (Exception ex)
            {
                MessagesListBox.Items.Add($"ОНО УМЕРЛО ИТД:\n {ex.Message}");
            }
        }

        private async Task SendMessageAsync()
        {
            if (connection is null)
                MessagesListBox.Items.Add("Соединение разорвано");

            string message = MessageTextBox.Text.Trim();
            if (String.IsNullOrEmpty(message))
                return;

            try
            {
                await connection.InvokeAsync("SendMessage", $"{user}: {message}");

                MessageTextBox.Clear();
            }
            catch(Exception ex)
            {
                MessagesListBox.Items.Add($"ОНО УМЕРЛО ИТД:\n {ex.Message}");
            }
        }

        private async void Window_Closed(object sender, EventArgs e)
        {
            if (connection is not null)
            {
                await connection.StopAsync();
                await connection.DisposeAsync();
            }
        }
    }
}