using Main.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Mail;

namespace Main
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly DataContext _dataContext;
		public MainWindow()
		{
			InitializeComponent();
			//_dataContext = new();
			//_dataContext.Users.Load();
		}
        private String? _mailbox;
		private SmtpClient? GetSmtpClient()
		{
            var config = JsonSerializer.Deserialize<JsonNode>(
                System.IO.File.ReadAllText("appconfig.json"));

            String? host = config?["emails"]?["gmail"]?["host"]?.ToString();
            String? port = config?["emails"]?["gmail"]?["port"]?.ToString();
            String? mailbox = config?["emails"]?["gmail"]?["mailbox"]?.ToString();
            String? key = config?["emails"]?["gmail"]?["key"]?.ToString();

            if (host == null || port == null || mailbox == null || key == null)
            {
                MessageBox.Show("Error reading configuration");
                return null;
            }
            _mailbox = mailbox;
            NetworkCredential credential = new(mailbox, key);
            return new()
            {
                Host = host,
                Port = Convert.ToInt32(port),
                Credentials = credential,
                EnableSsl = true
            };
        }

        private void SendEmailButton_Click(object sender, RoutedEventArgs e)
        {
            using var smtpClient = GetSmtpClient();
            if(smtpClient == null)
            {
                return;
            }
            try
			{
				smtpClient.Send(
					from: _mailbox!,
					recipients: "denniksam@gmail.com",
					subject: "Project Notification",
					body: "Email sent from .NET Project"
				);
				MessageBox.Show("Sent OK");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
        }

        private void SendHtmlButton_Click(object sender, RoutedEventArgs e)
        {
            using var smtpClient = GetSmtpClient();
            if (smtpClient == null)
            {
                return;
            }
            MailMessage message = new()
            {
                From = new MailAddress(_mailbox!),
                Subject = "Project Notification",
                IsBodyHtml = true,
                Body = "<h2>Hello, user!</h2><p>Your register code is: <b>12345</b></p>"
            };
            message.To.Add(new MailAddress("denniksam@gmail.com"));
            try
            {
                smtpClient.Send(message);
                MessageBox.Show("Sent OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
/* SMTP, робота з електронною поштою.
 * Загальна схема:
 * Програма                   Поштовий сервер              Користувач
 * користувач вводить          авторизує Програму           Одержує лист,
 * E-mail, формується   SMTP   знаходить E-mail             переглядає його,
 * лист, надсилається -------> надсилає лист -------------> копіює код,
 * 			<---------------------------------------------- вставляє у Програму
 * Перевіряється введений
 * код (з тим, що надсилався)
 * 
 * На прикладі поштового сервера Gmail
 * - рекомендовано створити окремий обліковий запис для проєкту
 * - у налаштуваннях облікового запису встановити двохетапну автентифікацію
 *    перейти в розділ "двохетапна автентифікація"
 * - з нього - у розділ "Пароли приложений" (внизу сторінки)
 * - створюємо пароль застосунку для Програми, зберігаємо його у 
 *    файлі конфігурації (який не передається на репозиторій)
 *    
 * Використання для задач підтвердження
 * - генеруєте випадковий код, надсилаємо його на пошту
 * - зберігаєте його у БД для даного користувача, наявність
 *    самого коду у БД є свідченням НЕ підтвердженості пошти
 * - при вході користувача (або при доступі до обмежених даних)
 *    перевіряється наявність коду (у БД), якщо він є, то виводиться
 *    вікно підтвердження пошти з вводом цього коду
 * - якщо код вводиться правильно, то з БД код видаляється,
 *    відсутність коду є свідченням підтвердження пошти
 */
