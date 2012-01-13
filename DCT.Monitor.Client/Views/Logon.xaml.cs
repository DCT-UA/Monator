using System.Windows;
using System.Windows.Controls;

namespace DCT.Monitor.Client.Views
{
    /// <summary>
    /// Interaction logic for Logon.xaml
    /// </summary>
    public partial class Logon : UserControl
    {
        public Logon()
        {
            InitializeComponent();
        }

        private void PasswordControl_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordHidden.Text = PasswordControl.Password;
        }
    }
}
