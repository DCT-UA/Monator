using DCT.Unity;
using DCT.WPF.MVC;
using DCT.Monitor.Client.Models;
using DCT.Monitor.Entities;
using DCT.Monitor.Modules;

namespace DCT.Monitor.Client.Controllers
{
    [Async]
    public class UserController: Controller
    {
        IUserModule module = ServiceLocator.Current.Resolve<IUserModule>();

        public object Logon(LogonModel model)
        {
            if (model == null) return new LogonModel();
            if (string.IsNullOrEmpty(model.UserName)) model.Error = "User Name is empty.";
            else if (string.IsNullOrEmpty(model.Password)) model.Error = "Password is empty.";
            else
            {
                var user = new User();
                user.UserName = model.UserName;
                user.Password = model.Password;

                if (module.Authenticate(user))
                {
                    App.CurrentApp.LogonUser(user);
                    return new StatisticsModel(Context);
                }

                model.Error = "Login failed. Check User Name or Password.";
            }

            return model;
        }

        public object Settings()
        {
            return new SettingsModel();
        }
    }
}
