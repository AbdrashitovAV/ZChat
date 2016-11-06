using System;
using System.Windows;
using Microsoft.Practices.Unity;
using ZChat.ViewModel;

namespace ZChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();

            _mainWindowViewModel = new MainWindowViewModel();
            DataContext = _mainWindowViewModel;
        }
    }
}
