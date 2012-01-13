using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DCT.Monitor.Entities;

namespace DCT.Monitor.Client.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public object SelectedPath
        {
            get { return (object)GetValue(SelectedPathProperty); }
            set { SetValue(SelectedPathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedPath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedPathProperty =
            DependencyProperty.Register("SelectedPath", typeof(object), typeof(MainWindow), new UIPropertyMetadata(null));


        public MainWindow()
        {
            InitializeComponent();
            var binding = new Binding("SelectedDomain");
            binding.Mode = BindingMode.TwoWay;
            SetBinding(SelectedPathProperty, binding);
        }

        private void lstDomains_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = sender as ListBox;
            SelectedPath = data.SelectedItem;
        }

        private void tvDomainStats_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void SetSelected(object data)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!(data is DomainStatistics)) return;
                SelectedPath = data;
            }));
        }
    }
}
