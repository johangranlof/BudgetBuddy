using BusinessLayer.Controllers;
using Entities;
using PresentationsLayerWPF.Commands;
using PresentationsLayerWPF.Models;
using PresentationsLayerWPF.Utilites;
using PresentationsLayerWPF.Views;
using System.Windows;
using System.Windows.Input;

namespace PresentationsLayerWPF.ViewModels
{
    public class LogInViewModel : ObservableObject
    {
        public LogInController LogInController;

        public LogInViewModel()
        {
            LogInController = new LogInController();
        }

        public string Username { get; set; }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        private ICommand loginCommand = null!;
        public ICommand LoginCommand => loginCommand ??= new RelayCommand(async () =>
        {
            IsLoading = true;

            User user = await Task.Run(() => LogInController.LogIn(Username, Password));
            Global.LoggedInUser = user;

            IsLoading = false;


            if (user != null)
            {
                MainView mv = new MainView();
                mv.Show();
                Application.Current.MainWindow.Close();
            }
            else
            {
                MessageBox.Show("Login failed.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

        private ICommand _createNewAccountCommand;
        public ICommand CreateAccountCommand => _createNewAccountCommand ??= new RelayCommand(() =>
        {
            CreateAccountView createAccountView = new CreateAccountView();
            createAccountView.Show();
            Application.Current.MainWindow.Close();
        });

        private ICommand closeWindowCommand = null!;
        public ICommand CloseWindowCommand => closeWindowCommand ??= new RelayCommand(() =>
        {
            Application.Current.Shutdown();
        });
    }
}
