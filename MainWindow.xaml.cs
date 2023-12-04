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
using System.Windows.Threading;

namespace Quotes_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {
        private DispatcherTimer timer;
        private ClientCommunication clientCommunication;

        public MainWindow()
        {
            InitializeComponent();
            clientCommunication = new ClientCommunication();

            // Создаем таймер и устанавливаем интервал на 1 секунду
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string connectionResult = clientCommunication.ConnectToServer(ipAddress.Text, portNumber.Text);
            OutputWindow.Text += connectionResult + Environment.NewLine;
            SendMessageButton.IsEnabled = clientCommunication.IsConnected();
            ScrollTextBlock.ScrollToEnd();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SendMessageButton.IsEnabled = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (clientCommunication.IsConnected())
            {
                string response = clientCommunication.SendMessage("timeQuiet");
                Status1.Text = "Время Сервера: " + response;
                Status2.Text = "Соединение с сервером: установлено";
            }
            else
            {
                Status1.Text = "Время Сервера: не доступно";
                Status2.Text = "Соединение с сервером: не установлено";
            }
        }


  
        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (clientCommunication.IsConnected())
            {
                if (InputWindow.Text.Length > 0)
                {
                    string response = clientCommunication.SendMessage(InputWindow.Text);
                    OutputWindow.Text += "Ответ сервера: " + response + Environment.NewLine;
                }

            }
            else
            {
                OutputWindow.Text += "Сообщение не отправлено: нет соединения с сервером" + Environment.NewLine;
            }
            ScrollTextBlock.ScrollToEnd();
        }



        private void InputWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (clientCommunication.IsConnected())
                {
                    if (InputWindow.Text.Length > 0)
                    {
                        string response = clientCommunication.SendMessage(InputWindow.Text);
                        OutputWindow.Text += "Ответ сервера: " + response + Environment.NewLine;
                    }

                }
                else
                {
                    OutputWindow.Text += "Сообщение не отправлено: нет соединения с сервером" + Environment.NewLine;
                }
                ScrollTextBlock.ScrollToEnd();
            }



        }
    }
}