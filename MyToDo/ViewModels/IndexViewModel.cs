using MyToDo.Common.Models;
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
        }


        private ObservableCollection<TaskBar> taskbars;

        public ObservableCollection<TaskBar> TaskBars
        {
            get { return taskbars; }
            set { taskbars = value; RaisePropertyChanged(); }
        }
        void CreatTaskBars() 
        {

            TaskBars.Add( new TaskBar() { Icon = "CalendarClock", Color="#ff0000ff ", Content="9" , Target="", Title=" 汇总"   });
            TaskBars.Add( new TaskBar() { Icon = "AlarmCheck", Color="#ff1eca3a", Content="9" , Target="", Title="已完成"   });
            TaskBars.Add( new TaskBar() { Icon = "ChartLineVariant", Color="#ff02c6dc", Content="100%" , Target="", Title="完成率"   });
            TaskBars.Add( new TaskBar() { Icon = "PlaylistStar", Color= "#ffff0000", Content="9" , Target="", Title="备忘录"   });


        }

    }
}
