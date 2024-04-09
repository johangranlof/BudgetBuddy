using BusinessLayer.Controllers;
using Entities;
using PresentationsLayerWPF.Commands;
using PresentationsLayerWPF.Models;
using PresentationsLayerWPF.Views;
using System;
using System.Windows;
using System.Windows.Input;

namespace PresentationsLayerWPF.ViewModels
{
    public class CreateAccountViewModel : ObservableObject
    {
        private UserController _userController { get; set; }

        public CreateAccountViewModel()
        {
            _userController = new UserController();
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        private decimal _balance;
        public decimal Balance
        {
            get => _balance;
            set
            {
                if (_balance != value)
                {
                    _balance = value;
                    OnPropertyChanged(nameof(Balance));
                }
            }
        }

        private decimal _savings;
        public decimal Savings
        {
            get => _savings;
            set
            {
                if (_savings != value)
                {
                    _savings = value;
                    OnPropertyChanged(nameof(Savings));
                }
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private ICommand _createAccountCommand;
        public ICommand CreateAccountCommand => _createAccountCommand ??= new RelayCommand(() =>
        {
            if (string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Name) ||
                Balance == 0 ||
                Savings == 0)
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var user = new User
            {
                Name = Name,
                Email = Email,
                Balance = Balance,
                Savings = Savings,
                Password = Password,
                Username = Username,
            };

            try
            {
                _userController.AddUser(user);
                MessageBox.Show("User created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        });

        private ICommand _closeWindowCommand;
        public ICommand CloseWindowCommand => _closeWindowCommand ??= new RelayCommand(() =>
        {
            Close();
        });

        public void Close()
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this);

            if (currentWindow != null)
            {
                LogInView logInView = new LogInView();
                Application.Current.MainWindow = logInView;
                logInView.Show();
                currentWindow.Close();
            }
        }
    }
}
