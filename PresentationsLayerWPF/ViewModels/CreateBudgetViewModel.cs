using BusinessLayer.Controllers;
using EntityLayer;
using EntityLayer.Enums;
using PresentationsLayerWPF.Commands;
using PresentationsLayerWPF.Models;
using PresentationsLayerWPF.Utilites;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PresentationsLayerWPF.ViewModels
{
    public class CreateBudgetViewModel : ObservableObject
    {
        public ObservableCollection<TypeOfExpense> Types { get; set; }
        public BudgetController BudgetController { get; set; }
        public ExpenseController ExpenseController { get; set; }

        public CreateBudgetViewModel()
        {
            BudgetController = new BudgetController();
            ExpenseController = new ExpenseController();
            Types = new ObservableCollection<TypeOfExpense>((TypeOfExpense[])Enum.GetValues(typeof(TypeOfExpense)));

            StartDate = DateTime.Today;
            EndDate = DateTime.Today.AddMonths(1);
        }

        private string _budgetName;
        public string BudgetName
        {
            get => _budgetName;
            set
            {
                if (_budgetName != value)
                {
                    _budgetName = value;
                    OnPropertyChanged(nameof(BudgetName));
                }
            }
        }

        private TypeOfExpense _selectedType;
        public TypeOfExpense SelectedType
        {
            get => _selectedType;
            set
            {
                if (_selectedType != value)
                {
                    _selectedType = value;
                    OnPropertyChanged(nameof(SelectedType));
                }
            }
        }

        private string _selectedTimePeriod;
        public string SelectedTimePeriod
        {
            get => _selectedTimePeriod;
            set
            {
                if (_selectedTimePeriod != value)
                {
                    _selectedTimePeriod = value;
                    OnPropertyChanged(nameof(SelectedTimePeriod));
                }
            }
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged(nameof(Amount));
                }
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
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
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        private ICommand _saveBudgetCommand;
        public ICommand SaveBudgetCommand => _saveBudgetCommand ??= new RelayCommand(() =>
        {
            var budget = new Budget
            {
                Name = BudgetName,
                UserID = Global.LoggedInUser.UserID,
                StartDate = StartDate,
                EndDate = EndDate,
                Amount = Amount,
                UpdatedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
            };
            try
            {
                BudgetController.AddBudget(budget);
                MessageBox.Show("Budget saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the budget: {ex.Message}");
            }
        });
    }
}
