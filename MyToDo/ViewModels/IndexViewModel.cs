using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
namespace MyToDo.ViewModels
{
    public class IndexViewModel : NavigationViewModel
    {
        private readonly IToDoService toDoService;
        private readonly IMemoService memoService;
        private readonly IDialogHostService dialog;
        private readonly IRegionManager regionManager;

        public IndexViewModel(IContainerProvider provider,
            IDialogHostService dialog) : base(provider)
        {
            CreateTaskBars();
            Title = $"你好，{AppSession.UserName} {DateTime.Now.GetDateTimeFormats('D')[1].ToString()}";
       
            ExecuteCommand = new DelegateCommand<string>(Execute);
            this.toDoService = provider.Resolve<IToDoService>();
            this.memoService = provider.Resolve<IMemoService>();
            this.regionManager = provider.Resolve<IRegionManager>();
            this.dialog = dialog;
            EditMemoCommand = new DelegateCommand<MemoDto>(AddMemo);
            EditToDoCommand = new DelegateCommand<ToDoDto>(AddToDo);
            ToDoCompltedCommand = new DelegateCommand<ToDoDto>(Complted);
            NavigateCommand = new DelegateCommand<TaskBar>(Navigate);
            InitTimer();


        }
        private void Navigate(TaskBar obj)
        {
            if (string.IsNullOrWhiteSpace(obj.Target)) return;

            NavigationParameters param = new NavigationParameters();

            if (obj.Title == "已完成")
            {
                param.Add("Value", 2);
            }
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.Target, param);
        }

        private async void Complted(ToDoDto obj)
        {
            try
            {
                UpdateLoading(true);
                var updateResult = await toDoService.UpdateAsync(obj);
                if (updateResult.Status)
                {
                    var todo = summary.ToDoList.FirstOrDefault(t => t.Id.Equals(obj.Id));
                    if (todo != null)
                    {
                        summary.ToDoList.Remove(todo);
                        summary.CompletedCount += 1;
                        summary.CompletedRatio = (summary.CompletedCount / (double)summary.Sum).ToString("0%");
                        this.Refresh();
                    }
                    eventAggregator.SendMessage("已完成!");
                }
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        public DelegateCommand<ToDoDto> ToDoCompltedCommand { get; private set; }
        public DelegateCommand<ToDoDto> EditToDoCommand { get; private set; }
        public DelegateCommand<MemoDto> EditMemoCommand { get; private set; }
        public DelegateCommand<string> ExecuteCommand { get; private set; }

        public DelegateCommand<TaskBar> NavigateCommand { get; private set; }

        #region 属性

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<TaskBar> taskBars;

        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskBars; }
            set { taskBars = value; RaisePropertyChanged(); }
        }

        private SummaryDto summary;

        /// <summary>
        /// 首页统计
        /// </summary>
        public SummaryDto Summary
        {
            get { return summary; }
            set { summary = value;  RaisePropertyChanged(); }
        }

        #endregion

        private void Execute(string obj)
        {
            switch (obj)
            {
                case "新增待办": AddToDo(null); break;
                case "新增备忘录": AddMemo(null); break;
            }
        }

        /// <summary>
        /// 添加待办事项
        /// </summary>
        async void AddToDo(ToDoDto model)
        {
            try
            {


                DialogParameters param = new DialogParameters();
                if (model != null)
                    param.Add("Value", model);

                var dialogResult = await dialog.ShowDialog("AddToDoView", param);
                if (dialogResult.Result == ButtonResult.OK)
                {
                    try
                    {
                        UpdateLoading(true);
                        var todo = dialogResult.Parameters.GetValue<ToDoDto>("Value");
                        if (todo.Id > 0)
                        {
                            var updateResult = await toDoService.UpdateAsync(todo);
                            if (updateResult.Status)
                            {
                                var todoModel = summary.ToDoList.FirstOrDefault(t => t.Id.Equals(todo.Id));
                                if (todoModel != null)
                                {
                                    todoModel.Title = todo.Title;
                                    todoModel.Content = todo.Content;
                                }
                            }
                        }
                        else
                        {
                            var addResult = await toDoService.AddAsync(todo);
                            if (addResult.Status)
                            {
                                summary.Sum += 1;
                                summary.ToDoList.Add(addResult.Result);
                               // summary.ToDoList.OrderByD(m => m.CreateTime);
                                summary.CompletedRatio = (summary.CompletedCount / (double)summary.Sum).ToString("0%");
                                this.Refresh();
                            }
                        }
                    }
                    finally
                    {
                        UpdateLoading(false);
                    }
                }
            }
            catch (Exception)
            {


            }
        }

        /// <summary>
        /// 添加备忘录
        /// </summary>
        async void AddMemo(MemoDto model)
        {
            DialogParameters param = new DialogParameters();
            if (model != null)
                param.Add("Value", model);

            var dialogResult = await dialog.ShowDialog("AddMemoView", param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                try
                {
                    UpdateLoading(true);
                    var memo = dialogResult.Parameters.GetValue<MemoDto>("Value");

                    if (memo.Id > 0)
                    {
                        var updateResult = await memoService.UpdateAsync(memo);
                        if (updateResult.Status)
                        {
                            var todoModel = summary.MemoList.FirstOrDefault(t => t.Id.Equals(memo.Id));
                            if (todoModel != null)
                            {
                                todoModel.Title = memo.Title;
                                todoModel.Content = memo.Content;
                            }
                        }
                    }
                    else
                    {
                        var addResult = await memoService.AddAsync(memo);
                        if (addResult.Status)
                        {
                            summary.MemoList.Add(addResult.Result);
                          //  summary.MemoList.OrderByDescending(m => m.CreateTime);
                        }
                    }
                }
                finally
                {
                    UpdateLoading(false);
                }
            }
        }

        void CreateTaskBars()
        {
            TaskBars = new ObservableCollection<TaskBar>();
            TaskBars.Add(new TaskBar() { Icon = "ClockFast", Title = "汇总", Color = "#FF0CA0FF", Target = "ToDoView" ,Content="0" });
            TaskBars.Add(new TaskBar() { Icon = "ClockCheckOutline", Title = "已完成", Color = "#FF1ECA3A", Target = "ToDoView", Content = "0" });
            TaskBars.Add(new TaskBar() { Icon = "ChartLineVariant", Title = "完成比例", Color = "#FF02C6DC", Target = "", Content = "0" });
            TaskBars.Add(new TaskBar() { Icon = "PlaylistStar", Title = "备忘录", Color = "#FFFFA000", Target = "MemoView",Content = "0" });
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var summaryResult = await toDoService.SummaryAsync();
            if (summaryResult.Status)
            {
                Summary = summaryResult.Result;
              


                Refresh();
            }
            base.OnNavigatedTo(navigationContext);
        }
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

        void Refresh()
        {
            TaskBars[0].Content = summary.Sum.ToString();
            TaskBars[1].Content = summary.CompletedCount.ToString();
            TaskBars[2].Content = summary.CompletedRatio;
            TaskBars[3].Content = summary.MemoeCount.ToString();
        }
    }


    /*        private void excute(string obj)
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
            }*/
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
