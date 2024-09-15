
using MyToDo.Common.Models;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class MemoViewModel : BindableBase
    {
        private readonly IMemoService memoService;
        public MemoViewModel(IMemoService memoService)
        {
            this.memoService = memoService;

            MemoDtos = new ObservableCollection<MemoDto>();
            CreatMemoList();
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
            set { isRightDrawerOpen = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<MemoDto> memoDtos;


        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }
        async void CreatMemoList()
        {
            var memoResult= await memoService.GetAllAsync(new Shared.Parameters.QueryParameter()
            {
                PageIndex = 0,
                PageSize = 100
            });
            if(memoResult.Status)
            {
                MemoDtos.Clear();
                foreach(var item in memoResult.Result.Items )
                {
                    MemoDtos.Add(item);

                }

            }
        }
        public DelegateCommand AddCommand { get; private set; }

    }
}
