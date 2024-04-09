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
    public class ExpenseViewModel : ObservableObject
    {
        public UserController UserController { get; set; }
        public ExpenseController ExpenseController { get; set; }
        public BudgetController BudgetController { get; set; }

        public ObservableCollection<Expense> Expenses { get; set; }
        public ObservableCollection<TypeOfExpense> Types { get; set; }
        public ObservableCollection<Budget> Budgets { get; set; }

        public ExpenseViewModel()
        {
            BudgetController = new BudgetController();
            ExpenseController = new ExpenseController();
            Budgets = new ObservableCollection<Budget>();
            UserController = new UserController();
            Expenses = new ObservableCollection<Expense>();
            Types = new ObservableCollection<TypeOfExpense>((TypeOfExpense[])Enum.GetValues(typeof(TypeOfExpense)));
            LoadExpenses();
            LoadBudgets();
        }

        public void LoadBudgets()
        {
            var budgets = BudgetController.GetBudgetsForUser(Global.LoggedInUser);
            foreach (var budget in budgets)
            {
                Budgets.Add(budget);
            }
        }

        public void LoadExpenses()
        {
            var expenses = ExpenseController.GetAllExpensesForUser(Global.LoggedInUser);
            foreach (var expense in expenses)
            {
                Expenses.Add(expense);
            }
        }

        private int _amount;
        public int Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged(nameof(Amount));
                }
            }
        }

        private DateTime _dateStart;
        public DateTime DateStart
        {
            get { return _dateStart; }
            set
            {
                if (_dateStart != value)
                {
                    _dateStart = value;
                    OnPropertyChanged(nameof(DateStart));
                }
            }
        }

        private DateTime _dateEnd;
        public DateTime DateEnd
        {
            get { return _dateEnd; }
            set
            {
                if (_dateEnd != value)
                {
                    _dateEnd = value;
                    OnPropertyChanged(nameof(DateEnd));
                }
            }
        }

        private DateTime _date = DateTime.Today;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

        private string _note;
        public string Note
        {
            get { return _note; }
            set
            {
                if (_note != value)
                {
                    _note = value;
                    OnPropertyChanged(nameof(Note));
                }
            }
        }

        private TypeOfExpense _selectedType;
        public TypeOfExpense SelectedType
        {
            get { return _selectedType; }
            set
            {
                if (_selectedType != value)
                {
                    _selectedType = value;
                    OnPropertyChanged(nameof(SelectedType));
                }
            }
        }

        private Expense _selectedExpense;
        public Expense SelectedExpense
        {
            get { return _selectedExpense; }
            set
            {
                if (_selectedExpense != value)
                {
                    _selectedExpense = value;
                    OnPropertyChanged(nameof(SelectedExpense));
                }
            }
        }

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

                    if (_selectedBudget != null)
                    {
                        DateStart = _selectedBudget.StartDate;
                        DateEnd = _selectedBudget.EndDate;
                        Date = DateStart;
                    }
                }
            }
        }

        private bool _isBudgetAssigned;
        public bool IsBudgetAssigned
        {
            get { return _isBudgetAssigned; }
            set
            {
                _isBudgetAssigned = value;
                OnPropertyChanged(nameof(IsBudgetAssigned));
                if(!value)
                {
                    SelectedBudget = null;
                }
            }
        }

        private ICommand _submitExpense;
        public ICommand SubmitExpense => _submitExpense ??= new RelayCommand(() =>
        {
            var expense = new Expense
            {
                Amount = Amount,
                Date = Date,
                Note = Note,
                Type = SelectedType,
                UserID = Global.LoggedInUser.UserID,
                BudgetID = IsBudgetAssigned ? SelectedBudget?.BudgetID : null,
            };

            ExpenseController.AddExpense(expense);
            Expenses.Add(expense);

            if (IsBudgetAssigned && SelectedBudget != null)
            {
                BudgetController.AddToTotalExpenses(SelectedBudget.BudgetID, Amount);
            }

            Global.LoggedInUser.Balance -= Amount;
            UserController.UpdateUser(Global.LoggedInUser);
        });

        private ICommand _saveAllChangesCommand;
        public ICommand SaveAllChangesCommand => _saveAllChangesCommand ??= new RelayCommand(() =>
        {
            foreach (var expense in Expenses)
            {
                ExpenseController.UpdateExpense(expense);
            }
        });

        private ICommand _exportToPdfCommand;
        public ICommand ExportToPdfCommand => _exportToPdfCommand ??= new RelayCommand(() =>
        {
            PdfExporter.ExportExpensesToPdf(Expenses);
        });

        private ICommand _chooseExportCommand;
        public ICommand ChooseExportCommand => _chooseExportCommand ??= new RelayCommand(() =>
        {
            PdfExporter.ExportExpensesToPdf(Expenses);
        });

        private ICommand _deleteSelectedCommand;
        public ICommand DeleteSelectedCommand => _deleteSelectedCommand ??= new RelayCommand(() =>
        {
            if (SelectedExpense != null)
            {
                Global.LoggedInUser.Balance += SelectedExpense.Amount;
                ExpenseController.DeleteExpense(SelectedExpense);
                Expenses.Remove(SelectedExpense);
            }
        });

        private ICommand _deleteAllCommand;
        public ICommand DeleteAllCommand => _deleteAllCommand ??= new RelayCommand(() =>
        {
            if (Expenses.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete all expenses?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    foreach (var expense in Expenses.ToList())
                    {
                        Global.LoggedInUser.Balance += expense.Amount;
                        ExpenseController.DeleteExpense(expense);
                        Expenses.Remove(expense);
                    }
                }
            }
        });
    }
}