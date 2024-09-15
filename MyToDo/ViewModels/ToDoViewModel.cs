using MyToDo.Common.Models;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Common;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class ToDoViewModel : BindableBase
    {
        public ToDoViewModel(IToDoService toDoService)
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();

            AddCommand = new DelegateCommand(Add);

            this.toDoService = toDoService;
            CreatToDoList();
        }

        private void Add()
        {
            IsRightDrawerOpen = true;
        }
        /// <summary>
        /// 右侧窗口展开
        /// </summary>
        private bool isRightDrawerOpen;

        public bool IsRightDrawerOpen
        {
            get { return isRightDrawerOpen; }
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<ToDoDto> toDoDtos;
        private readonly IToDoService toDoService;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }
        async void CreatToDoList()
        {
            var todoResult = await toDoService.GetAllAsync(new Shared.Parameters.QueryParameter
            {
                PageIndex = 0,
                PageSize = 100,
            });
            if (todoResult.Status)
            {
                ToDoDtos.Clear();
                foreach (var item in todoResult.Result.Items)
                {
                    ToDoDtos.Add(item);
                }

            }
        }
        public DelegateCommand AddCommand { get; private set; }

    }

}
