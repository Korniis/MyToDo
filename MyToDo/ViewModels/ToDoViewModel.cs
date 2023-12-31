﻿using MyToDo.Common.Models;
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
        public ToDoViewModel()
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();
            CreatToDoList();
            AddCommand = new DelegateCommand(Add);
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
            set { isRightDrawerOpen = value;  RaisePropertyChanged(); }
        }

        private ObservableCollection<ToDoDto> toDoDtos;

        public ObservableCollection<ToDoDto> ToDoDtos
        {
            get { return toDoDtos; }
            set { toDoDtos = value; RaisePropertyChanged(); }
        }
        void CreatToDoList()
        {
            for (int i = 0; i < 20; i++)
            {
                ToDoDtos.Add(new ToDoDto()
                {
                    Title = "标题" + i,
                    Content = "测试数据"
                });


            }
        }
        public DelegateCommand AddCommand { get; private set; }

    }

}
