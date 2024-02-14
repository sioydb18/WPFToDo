using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFToDo.Models;

namespace WPFToDo.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<ToDoItem> Items { get; } = new ObservableCollection<ToDoItem>();

        public ICommand AddItemCommand { get; }
        public ICommand DeleteItemCommand { get; }

        private string _newItemTitle;
        public string NewItemTitle
        {
            get => _newItemTitle;
            set => _newItemTitle = value; // NotifyPropertyChanged omitted for brevity
        }

        public MainViewModel()
        {
            AddItemCommand = new RelayCommand(_ => AddItem());
            DeleteItemCommand = new RelayCommand(DeleteItem);
        }

        private void AddItem()
        {
            if (!string.IsNullOrEmpty(NewItemTitle))
            {
                Items.Add(new ToDoItem { Title = NewItemTitle, IsCompleted = false });
                NewItemTitle = string.Empty; // Clear the input after adding
            }
        }

        private void DeleteItem(object parameter)
        {
            if (parameter is ToDoItem item)
            {
                Items.Remove(item);
            }
        }
    }

    public class RelayCommand : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;
        public void Execute(object parameter) => _execute(parameter);
    }
}
