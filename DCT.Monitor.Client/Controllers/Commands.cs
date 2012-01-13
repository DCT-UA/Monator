using DCT.WPF.MVC;
using DCT.WPF.MVC.Actions;
using DCT.Monitor.Client.Models;

namespace DCT.Monitor.Client.Controllers
{
    public class Commands
    {
        public static StatisticsController StatisticsController = new StatisticsController();
        public static UserController UserController = new UserController();

        public static readonly ControllerCommand StatisticsCommand = ControllerCommand.Create<StatisticsController, StatisticsModel>((c) => c.GetStatistics, NavigationHint.Switch);
        public static readonly ControllerCommand RefreshStatisticsCommand = ControllerCommand.Create<StatisticsController, StatisticsModel>((c) => c.GetStatistics, NavigationHint.None);
        public static readonly ControllerCommand SelectCommand = ControllerCommand.Create<StatisticsController, StatisticsModel>((c) => c.SelectDomain, NavigationHint.None);
        public static readonly ControllerCommand LogonCommand = ControllerCommand.Create<UserController, LogonModel>((c) => c.Logon, NavigationHint.Switch);
        public static readonly ControllerCommand SettingsCommand = ControllerCommand.Create<UserController>((c) => c.Settings, NavigationHint.Next);

        public static readonly ControllerCommand ExitCommand = ShutdownAction.Command;
        public static readonly ControllerCommand NavigationCommand = OpenFileAction.Command;
    }
}
