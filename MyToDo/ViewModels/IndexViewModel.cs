using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
namespace MyToDo.ViewModels
{
    public class IndexViewModel : BindableBase
    {
        public readonly IDialogHostService dialog;
        public IndexViewModel(IDialogHostService dialog)
        {
            TaskBars = new ObservableCollection<TaskBar>();
            InitTimer();
            CreatTaskBars();
            ExcuteCommand = new DelegateCommand<string>(excute);
            this.dialog = dialog;
            // CreateTestDate();
        }
        private void excute(string obj)
        {
            switch (obj)
            {
                case "新增待办": AddToDo(); break;
                case "新增备忘": AddMemo(); break;
            }
        }
        private async void AddMemo()
        {
           await dialog.ShowDialog("AddMemoView",null,"Root");
        }
        private async void AddToDo()
        {
           await dialog.ShowDialog("AddToDoView",null,"Root");
        }
        #region 属性
        private ObservableCollection<TaskBar> taskbars;
        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskbars; }
            set { taskbars = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<ToDoDto> toDoDtos;
        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<MemoDto> memoDtos;
        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }
        #endregion
        public DelegateCommand<string> ExcuteCommand { get; set; }
        private void InitTimer()
        {
            DispatcherTimer _timer = new DispatcherTimer();
            _timer.Interval = System.TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_tick;
            _timer.Start();
        }
        private void Timer_tick(object sender, EventArgs e)
        {
            DateNow = DateTime.Now.ToString("yyyy/MM/dd");
            TimeNow = DateTime.Now.ToString("HH:mm:ss");
            DayNow = DateTime.Now.ToString("dddd");
        }
        private string timeNow;
        public string TimeNow
        {
            get { return timeNow; }
            set { timeNow = value; RaisePropertyChanged(); }
        }
        private string dayNow;
        public string DayNow
        {
            get { return dayNow; }
            set { dayNow = value; RaisePropertyChanged(); }
        }
        private string dateNow;
      

        public string DateNow
        {
            get { return dateNow; }
            set { dateNow = value; RaisePropertyChanged(); }
        }
        void CreatTaskBars()
        {
            TaskBars.Add(new TaskBar() { Icon = "CalendarClock", Color = "#ff0000ff ", Content = "9", Target = "", Title = " 汇总" });
            TaskBars.Add(new TaskBar() { Icon = "AlarmCheck", Color = "#ff1eca3a", Content = "9", Target = "", Title = "已完成" });
            TaskBars.Add(new TaskBar() { Icon = "ChartLineVariant", Color = "#ff02c6dc", Content = "100%", Target = "", Title = "完成率" });
            TaskBars.Add(new TaskBar() { Icon = "PlaylistStar", Color = "#ffff0000", Content = "9", Target = "", Title = "备忘录" });
        }
        /*  void CreateTestDate()
          {
              ToDoDtos = new ObservableCollection<ToDoDto>();
              MemoDtos = new ObservableCollection<MemoDto>();
              for (int i = 0; i < 10; i++)
              {
                  ToDoDtos.Add(new ToDoDto() { Title = "待办" + i, Content = "正在处理中。。。。。" });
                  MemoDtos.Add(new MemoDto() { Title = "备忘" + i, Content = "我的密码" });
              }
          }*/
    }
}
