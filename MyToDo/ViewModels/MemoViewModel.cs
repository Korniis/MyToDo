
using MyToDo.Common.Models;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class MemoViewModel : NavigationViewModel
    {
        public MemoViewModel(IMemoService memoService, IContainerProvider containerProvider) : base(containerProvider)
        {
            MemoDtos = new ObservableCollection<MemoDto>();
            ExcuteCommand = new DelegateCommand<string>(Excute);
            SelectedCommand = new DelegateCommand<MemoDto>(Select);
            DeleteCommand = new DelegateCommand<MemoDto>(Delete);
            this.memoService = memoService;
        }
        private void Excute(string obj)
        {
            switch (obj)
            {
                case "新增": Add(); break;
                case "查询": QuerySearch(); break;
                case "保存": Save(); break;
            }
        }
        private async void Delete(MemoDto obj)
        {
            try
            {
                var deleteResult = await memoService.DeleteAsync(obj.Id);
                if (deleteResult.Status)
                {
                    MemoDtos.Remove(obj);
                }
            }
            catch (Exception ex)
            {
            }
        }
        private async void Save()
        {
            if (string.IsNullOrWhiteSpace(CurrentDto.Content) || string.IsNullOrWhiteSpace(CurrentDto.Title))
            {
                return;
            }
            UpdateLoading(true);
            try
            {
                if (CurrentDto.Id > 0)
                {
                    var UpdateResult = await memoService.UpdateAsync(CurrentDto);
                    if (UpdateResult.Status)
                    {
                        var memo = MemoDtos.FirstOrDefault(t => t.Id == currentDto.Id);
                        if (memo != null)
                        {
                            memo.Title = CurrentDto.Title;
                            memo.Content = CurrentDto.Content;
                            IsRightDrawerOpen = false;
                        }
                    }
                }
                else
                {
                    var addResult = await memoService.AddAsync(CurrentDto);
                    if (addResult.Status)
                    {
                        MemoDtos.Add(addResult.Result);
                        IsRightDrawerOpen = false;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                UpdateLoading(false);
            }
        }
        private async void QuerySearch()
        {
            await GetDataAsync();
        }
        private async void FilterSearch()
        {
            await GetDataAsync();
        }

        private MemoDto currentDto;
        public MemoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value; RaisePropertyChanged(); }
        }
        private async void Select(MemoDto dto)
        {
            try
            {
                UpdateLoading(true);
                var memoResult = await memoService.GetFirstOfDefaultAsync(dto.Id);
                if (memoResult.Status)
                {
                    CurrentDto = memoResult.Result;
                    IsRightDrawerOpen = true;
                }
                UpdateLoading(false);
            }
            catch (Exception ex)
            {
            }
        }
        private void Add()
        {
            CurrentDto = new MemoDto();
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
        private readonly IMemoService memoService;
        public ObservableCollection<MemoDto> MemoDtos
        {
            get { return memoDtos; }
            set { memoDtos = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        async Task GetDataAsync()
        {
            UpdateLoading(true);
            int? Status = SelectedIndex == 0 ? null : SelectedIndex == 2 ? 1 : 0;
            var memoResult = await memoService.GetAllAsync(new QueryParameter
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search,
            });
            if (memoResult.Status)
            {
                MemoDtos.Clear();
                foreach (var item in memoResult.Result.Items)
                {
                    MemoDtos.Add(item);
                }
            }
            UpdateLoading(false);
        }

        public DelegateCommand FilterCommand { get; set; }
        public DelegateCommand<string> ExcuteCommand { get; private set; }
        public DelegateCommand<MemoDto> SelectedCommand { get; private set; }
        public DelegateCommand<MemoDto> DeleteCommand { get; private set; }
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            GetDataAsync();
        }
        private String search;
        public String Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }
        private int selectedIndex;


        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; RaisePropertyChanged(); }
        }

    }
}
