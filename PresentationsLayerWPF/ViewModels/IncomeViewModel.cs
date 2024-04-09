using BusinessLayer.Controllers;
using EntityLayer;
using EntityLayer.Enums;
using PresentationsLayerWPF.Commands;
using PresentationsLayerWPF.Models;
using PresentationsLayerWPF.Utilites;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PresentationsLayerWPF.ViewModels
{
    public class IncomeViewModel : ObservableObject
    {
        public UserController UserController { get; set; }
        public IncomeController IncomeController { get; set; }
        public BudgetController BudgetController { get; set; }
        public ObservableCollection<TypeOfIncome> Types { get; set; }
        public ObservableCollection<Income> Incomes { get; set; }


        public IncomeViewModel()
        {
            IncomeController = new IncomeController();
            UserController = new UserController();
            Incomes = new ObservableCollection<Income>();
            Types = new ObservableCollection<TypeOfIncome>((TypeOfIncome[])Enum.GetValues(typeof(TypeOfIncome)));
            LoadIncomes();
        }

        public void LoadIncomes()
        {
            var incomes = IncomeController.GetAllIncomesForUser(Global.LoggedInUser);
            foreach (var income in incomes)
            {
                Incomes.Add(income);
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

        private TypeOfIncome _selectedType;
        public TypeOfIncome SelectedType
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

        private ICommand _submitIncome;
        public ICommand SubmitIncome => _submitIncome ??= new RelayCommand(() =>
        {
            var income = new Income
            {
                Amount = Amount,
                Date = Date,
                Note = Note,
                UserID = Global.LoggedInUser.UserID,
                Type = SelectedType,
            };
            Global.LoggedInUser.Balance += Amount;
            UserController.UpdateUser(Global.LoggedInUser);
            IncomeController.AddIncome(income);
            Incomes.Add(income);
        });

        private Income _selectedIncome;
        public Income SelectedIncome
        {
            get { return _selectedIncome; }
            set
            {
                if (_selectedIncome != value)
                {
                    _selectedIncome = value;
                    OnPropertyChanged(nameof(SelectedIncome));
                }
            }
        }

        private ICommand _deleteSelected;
        public ICommand DeleteSelected => _deleteSelected ??= new RelayCommand(() =>
        {
            if (SelectedIncome != null)
            {
                Global.LoggedInUser.Balance -= SelectedIncome.Amount;
                IncomeController.DeleteIncome(SelectedIncome);
                Incomes.Remove(SelectedIncome);
            }
        });

        private ICommand _deleteAllCommand;
        public ICommand DeleteAllCommand => _deleteAllCommand ??= new RelayCommand(() =>
        {
            if (Incomes.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete all incomes?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    foreach (var income in Incomes.ToList())
                    {
                        Global.LoggedInUser.Balance -= income.Amount;
                        IncomeController.DeleteIncome(income);
                        Incomes.Remove(income);
                    }
                }
            }
        });

        private ICommand _saveAllChangesCommand;
        public ICommand SaveAllChangesCommand => _saveAllChangesCommand ??= new RelayCommand(() =>
        {
            foreach (var income in Incomes)
            {
                IncomeController.UpdateIncome(income);
            }
        });
    }
}
