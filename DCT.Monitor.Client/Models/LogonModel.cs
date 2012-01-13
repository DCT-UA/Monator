using DCT.WPF.MVC;

namespace DCT.Monitor.Client.Models
{
    public class LogonModel: ViewModel
    {
        private string _error;

        public string Error
        {
            get { return _error; }
            set { _error = value; SendPropertyChanged("Error"); }
        }

        private string _password;

        public string UserName
        {
            get { return _password; }
            set { _password = value; SendPropertyChanged("UserName"); }
        }

        private string _userName;

        public string Password
        {
            get { return _userName; }
            set { _userName = value; SendPropertyChanged("Password"); }
        }
    }
}
