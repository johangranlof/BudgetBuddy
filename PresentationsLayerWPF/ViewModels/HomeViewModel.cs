using EntityLayer;
using PresentationsLayerWPF.Models;
using PresentationsLayerWPF.Utilites;
using System.Collections.ObjectModel;
using LiveCharts;
using LiveCharts.Wpf;
using System.Linq;
using System.Windows.Media;
using System;
using System.Windows;
using Entities;
using PresentationsLayerWPF.Commands;
using System.Windows.Input;
using BusinessLayer.Controllers;

namespace PresentationsLayerWPF.ViewModels
{
    public class HomeViewModel : ObservableObject
    {
        private IncomeController IncomeController { get; set; }
        private ExpenseController ExpenseController { get; set; }
        private BudgetController BudgetController { get; set; }
        private UserController UserController { get; set; }
        public ObservableCollection<Budget> Budgets { get; set; }

        public HomeViewModel()
        {
            BudgetController = new BudgetController();
            ExpenseController = new ExpenseController();
            IncomeController = new IncomeController();
            UserController = new UserController();
            Budgets = new ObservableCollection<Budget>();
            ExpenseCategoryValues = new SeriesCollection();

            UpdateGaugeValue();
            LoadBudgets();

            SelectedTimePeriod = "All";
        }

        private double _gaugeValue;
        public double GaugeValue
        {
            get { return _gaugeValue; }
            set
            {
                if (_gaugeValue != value)
                {
                    _gaugeValue = value;
                    OnPropertyChanged(nameof(GaugeValue));
                }
            }
        }

        private decimal _totalIncome;
        public decimal TotalIncome
        {
            get { return _totalIncome; }
            set
            {
                if (_totalIncome != value)
                {
                    _totalIncome = value;
                    OnPropertyChanged(nameof(TotalIncome));
                }
            }
        }

        private decimal _totalExpenses;
        public decimal TotalExpenses
        {
            get { return _totalExpenses; }
            set
            {
                if (_totalExpenses != value)
                {
                    _totalExpenses = value;
                    OnPropertyChanged(nameof(TotalExpenses));
                }
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        public bool IsBudgetSelected => SelectedBudget != null;

        private Budget _selectedBudget;
        public Budget SelectedBudget
        {
            get { return _selectedBudget; }
            set
            {
                if (_selectedBudget != value)
                {
                    _selectedBudget = value;
                    OnPropertyChanged(nameof(SelectedBudget));
                    OnPropertyChanged(nameof(IsBudgetSelected));
                    UpdateGaugeValue();
                }
            }
        }

        private string _selectedTimePeriod;
        public string SelectedTimePeriod
        {
            get { return _selectedTimePeriod; }
            set
            {
                if (_selectedTimePeriod != value)
                {
                    _selectedTimePeriod = value;
                    OnPropertyChanged(nameof(SelectedTimePeriod));
                    UpdateTimePeriod();
                }
            }
        }

        private int _transferAmount;
        public int TransferAmount
        {
            get { return _transferAmount; }
            set
            {
                if (_transferAmount != value)
                {
                    _transferAmount = value;
                    OnPropertyChanged(nameof(TransferAmount));
                }
            }
        }

        private User _user = Global.LoggedInUser;
        public User User
        {
            get { return _user; }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        }

        private decimal _balance = Global.LoggedInUser.Balance;
        public decimal Balance
        {
            get { return _balance; }
            set
            {
                if (_balance != value)
                {
                    _balance = value;
                    OnPropertyChanged(nameof(Balance));
                }
            }
        }

        private decimal _savings = Global.LoggedInUser.Savings;
        public decimal Savings
        {
            get { return _savings; }
            set
            {
                if (_savings != value)
                {
                    _savings = value;
                    OnPropertyChanged(nameof(Savings));
                }
            }
        }

        private SeriesCollection _expenseCategoryValues;
        public SeriesCollection ExpenseCategoryValues
        {
            get { return _expenseCategoryValues; }
            set
            {
                if (_expenseCategoryValues != value)
                {
                    _expenseCategoryValues = value;
                    OnPropertyChanged(nameof(ExpenseCategoryValues));
                }
            }
        }

        private string[] _expenseCategoryNames;
        public string[] ExpenseCategoryNames
        {
            get { return _expenseCategoryNames; }
            set
            {
                if (_expenseCategoryNames != value)
                {
                    _expenseCategoryNames = value;
                    OnPropertyChanged(nameof(ExpenseCategoryNames));
                }
            }
        }

        private ICommand _transferToBalance;
        public ICommand TransferToBalance => _transferToBalance ??= new RelayCommand(() =>
        {
            if(TransferAmount > Savings)
            {
                MessageBox.Show("You have exceeded the amount", "Oops!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                User.Balance += TransferAmount;
                User.Savings -= TransferAmount;
                Balance += TransferAmount;
                Savings -= TransferAmount;
                var updatedUser = UserController.UpdateUser(User);
                User = updatedUser;
            }
        });

        private ICommand _transferToSavings;
        public ICommand TransferToSavings => _transferToSavings ??= new RelayCommand(() =>
        {
            if (TransferAmount > Balance)
            {
                MessageBox.Show("You have exceeded the amount", "Oops!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                User.Balance -= TransferAmount;
                User.Savings += TransferAmount;
                Balance -= TransferAmount;
                Savings += TransferAmount;
                var updatedUser = UserController.UpdateUser(User);
                User = updatedUser;
            }
        });

        private ICommand _deleteBudgetCommand;
        public ICommand DeleteBudgetCommand => _deleteBudgetCommand ??= new RelayCommand(() =>
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete the budget?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                BudgetController.DeleteBudget(SelectedBudget);
                Budgets.Remove(SelectedBudget);

                if (Budgets.Count > 0)
                {
                    SelectedBudget = Budgets.First();
                }
                else
                {
                    SelectedBudget = null;
                    GaugeValue = 0; 
                }
            }
        });

        private void UpdateGaugeValue()
        {
            if (SelectedBudget != null)
            {
                var remaining = SelectedBudget.Amount - SelectedBudget.TotalExpenses;
                GaugeValue = (double)remaining;
            }
        }

        private void LoadBudgets()
        {
            var budgets = BudgetController.GetBudgetsForUser(Global.LoggedInUser);
            Budgets.Clear();
            foreach (Budget budget in budgets)
            {
                Budgets.Add(budget);
            }

            if (Budgets != null && Budgets.Any())
            {
                SelectedBudget = Budgets.First();
            }
            else
            {
                SelectedBudget = null;
            }
        }

        private void UpdateTimePeriod()
        {
            DateTime now = DateTime.Now;

            switch (SelectedTimePeriod)
            {
                case "1Month":
                    StartDate = new DateTime(now.Year, now.Month, 1);
                    EndDate = StartDate.AddMonths(1).AddDays(-1);
                    break;
                case "6Months":
                    StartDate = now.AddMonths(-5);
                    EndDate = now;
                    break;
                case "1Year":
                    StartDate = now.AddYears(-1).AddDays(1);
                    EndDate = now;
                    break;
                case "All":
                    StartDate = DateTime.MinValue;
                    EndDate = DateTime.MaxValue;
                    break;
            }
            LoadIncomes();
            LoadExpenses();
        }

        private void LoadIncomes()
        {
            var allIncomes = IncomeController.GetAllIncomesForUser(Global.LoggedInUser);
            var incomes = allIncomes.Where(i => i.Date >= StartDate && i.Date <= EndDate).ToList();
            TotalIncome = incomes.Sum(i => i.Amount);
        }

        private void LoadExpenses()
        {
            var allExpenses = ExpenseController.GetAllExpensesForUser(Global.LoggedInUser);
            var expenses = allExpenses.Where(e => e.Date >= StartDate && e.Date <= EndDate).ToList();
            TotalExpenses = expenses.Sum(e => e.Amount);

            var expenseGroups = expenses
                .GroupBy(e => e.Type)
                .Select(g => new { Type = g.Key, Amount = g.Sum(e => e.Amount) })
                .ToList();

            var totalAmount = expenseGroups.Sum(g => g.Amount);

            ExpenseCategoryValues.Clear();
            int colorIndex = 0;
            foreach (var group in expenseGroups)
            {
                var percentage = totalAmount > 0 ? (group.Amount / totalAmount) * 100 : 0;
                ExpenseCategoryValues.Add(new PieSeries
                {
                    Title = group.Type.ToString(),
                    Values = new ChartValues<decimal> { group.Amount },
                    DataLabels = true,
                    LabelPoint = chartPoint => string.Format("{0:P}", chartPoint.Participation),
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(GetColor(colorIndex))),
                    Stroke = Brushes.White,
                    StrokeThickness = 2,
                    FontSize = 16,
                    Foreground = Brushes.Black,
                    
                });
                colorIndex++;
            }
            ExpenseCategoryNames = expenseGroups.Select(g => g.Type.ToString()).ToArray();
        }

        private string GetColor(int index)
        {
            var colors = new[] {
                "#33475e",
                "#4CAF50",
                "#FFC107",
                "#9C27B0",
                "#FF5722" 
                }; 
            return colors[index % colors.Length];
        }
    }
}
