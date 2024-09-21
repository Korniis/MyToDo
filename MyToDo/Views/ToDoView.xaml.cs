using MyToDo.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MyToDo.Views
{
    /// <summary>
    /// ToDoView.xaml 的交互逻辑
    /// </summary>
    public partial class ToDoView : UserControl
    {
        private readonly ToDoViewModel toDoViewModel;

        public ToDoView(ToDoViewModel toDoViewModel)
        {
            InitializeComponent();
            this.toDoViewModel = toDoViewModel;
            DataContext = toDoViewModel;  // 确保 ViewModel 绑定到 DataContext
        }

        // ComboBox 的 SelectionChanged 事件处理
        private void ComboBox_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count == 1)
                if (toDoViewModel.FilterCommand.CanExecute())
                {
                    // 将 SelectedItem 或 SelectedIndex 作为参数传递

                    toDoViewModel.FilterCommand.Execute(); // 或 comboBox.SelectedItem
                }
        }

    }
}

