using BusinessLayer.Controllers;
using EntityLayer;
using PresentationsLayerWPF.Commands;
using PresentationsLayerWPF.Models;
using PresentationsLayerWPF.Utilites;
using PresentationsLayerWPF.Views;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace PresentationsLayerWPF.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public UserController UserController { get; set; }
        public ObservableCollection<Income> Incomes = new ObservableCollection<Income>();

        public MainViewModel()
        {
            UserController = new UserController();
            Name = UserController.GetUser(Global.LoggedInUser.UserID).Name;
        }

        private UserControl currentView = new HomeUC();

        public UserControl? CurrentView
        {
            get { return currentView; }
            set
            {
                currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private ICommand homeCommand = null!;
        public ICommand HomeCommand => homeCommand ??= new RelayCommand(() =>
        {
            CurrentView = new HomeUC();
        });

        private ICommand createBudgetCommand = null!;
        public ICommand CreateBudgetCommand => createBudgetCommand ??= new RelayCommand(() =>
        {
            CurrentView = new CreateBudgetUC();
        });

        private ICommand _seeIncomesCommand = null!;
        public ICommand SeeIncomesCommand => _seeIncomesCommand ??= new RelayCommand(() =>
        {
            CurrentView = new AddIncomeUC();
        });

        private ICommand _seeExpensesCommand = null!;
        public ICommand SeeExpensesCommand => _seeExpensesCommand ??= new RelayCommand(() =>
        {
            CurrentView = new AddExpenseUC();
        });

        private ICommand _logOutCommand = null!;
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(() =>
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this);

            if (currentWindow != null)
            {
                LogInView logInView = new LogInView();
                Application.Current.MainWindow = logInView;
                logInView.Show();
                currentWindow.Close();
            }
        });

        private ICommand exitCommand = null!;
        public ICommand ExitCommand => exitCommand ??= new RelayCommand(() => App.Current.Shutdown());
    }
}
