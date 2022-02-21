    using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;
using WpfApp2.Classes;
using WpfApp2.Properties;

namespace WpfApp2.Views
{
    /// <summary>
    /// Логика взаимодействия для AutorizationPage.xaml
    /// </summary>
    public partial class AutorizationPage : Page
    {
        int errorcount = 0;
        public DispatcherTimer timer;
        public AutorizationPage()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            BlockPage();
            Settings.Default.Save();
        }
        void BlockPage()
        {
            if (Settings.Default.TimeBlock > DateTime.Now)
            {
                this.IsEnabled = false;
            }
            else
            {
                this.IsEnabled = true;
            }
        }

        private void SignInBtn_Click(object sender, RoutedEventArgs e)
        {
            var CurrentUser = UserPage.db.User.FirstOrDefault(u => u.Login == LogTB.Text && u.Password == PassTB.Password);
            if (CurrentUser != null)
            {
                MessageBox.Show("Вы вошли!");
                NavigationService.Navigate(new WelcomePage());
            }
            else
            {
                MessageBox.Show("Данные неверны");
                errorcount++;
            }
            if (errorcount == 3)
            {
                CaptchaTxb.Visibility = Visibility.Visible;
                CaptchaTB.Visibility = Visibility.Visible;
                CaptchaTxb.Text = Captcha();
                LogTB.IsEnabled = false;
                PassTB.IsEnabled = false;
            }
            else if (CaptchaTB.Text == CaptchaTxb.Text)
            {

                LogTB.IsEnabled = true;
                PassTB.IsEnabled = true;
                SignInBtn.IsEnabled = true;
                CaptchaTxb.Visibility = Visibility.Collapsed;
                CaptchaTB.Visibility = Visibility.Collapsed;
            }
            else if (errorcount > 5)
            {
                MessageBox.Show("Вы заблокированы на 5 сек");
                Settings.Default.TimeBlock = DateTime.Now.AddSeconds(5);
                Settings.Default.Save();
            }
            
        }

         string Captcha()
        {
            Random random = new Random();
            string alphabet = "qwertyuiopasdfghjklmnbvcxzQWERTYUIOPLKJHGFDSAZXCVBNM1234567890";
            string captcha = "";

            for (int i = 0; i < 4; i++)
            {
                captcha += alphabet[random.Next(alphabet.Length)];
            }
            return captcha;
        }
    }
}
