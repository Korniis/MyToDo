using MyToDo.Common.Models;
using MyToDo.Shared.Dtos;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class IndexViewModel : BindableBase
    {
        public IndexViewModel()
        {
            TaskBars = new ObservableCollection<TaskBar> ();
            CreatTaskBars();
            CreateTestDate();
        }


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

        void CreatTaskBars() 
        {

            TaskBars.Add( new TaskBar() { Icon = "CalendarClock", Color="#ff0000ff ", Content="9" , Target="", Title=" 汇总"   });
            TaskBars.Add( new TaskBar() { Icon = "AlarmCheck", Color="#ff1eca3a", Content="9" , Target="", Title="已完成"   });
            TaskBars.Add( new TaskBar() { Icon = "ChartLineVariant", Color="#ff02c6dc", Content="100%" , Target="", Title="完成率"   });
            TaskBars.Add( new TaskBar() { Icon = "PlaylistStar", Color= "#ffff0000", Content="9" , Target="", Title="备忘录"   });


        }
        void CreateTestDate() 
        {
            ToDoDtos= new ObservableCollection<ToDoDto>();
            MemoDtos = new ObservableCollection<MemoDto>();
            for (int i = 0; i < 10; i++)
            {
                ToDoDtos.Add(new ToDoDto() { Title="待办"+i , Content="正在处理中。。。。。" });
                MemoDtos.Add(new MemoDto() { Title = "备忘"+i , Content = "我的密码" });

            }
        
        }

    }
}
