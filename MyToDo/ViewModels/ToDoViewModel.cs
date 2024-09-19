using MyToDo.Common.Models;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using Prism.Commands;
using Prism.Common;
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
    public class ToDoViewModel : NavigationViewModel
    {
        public ToDoViewModel(IToDoService toDoService, IContainerProvider containerProvider) : base(containerProvider)
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();
            ExcuteCommand = new DelegateCommand<string>(Excute);
            SelectedCommand = new DelegateCommand<ToDoDto>(Select);
            this.toDoService = toDoService;
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
                    var UpdateResult = await toDoService.UpdateAsync(CurrentDto);
                    if (UpdateResult.Status)
                    {
                        var todo = ToDoDtos.FirstOrDefault(t => t.Id == currentDto.Id);
                        if (todo != null)
                        {
                            todo.Title = CurrentDto.Title;
                            todo.Content = CurrentDto.Content;
                            todo.Status = CurrentDto.Status;
                            IsRightDrawerOpen = false;

                        }
                    }

                }
                else
                {
                    var addResult = await toDoService.AddAsync(CurrentDto);
                    if (addResult.Status)
                    {
                        ToDoDtos.Add(addResult.Result);
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

        private ToDoDto currentDto;

        public ToDoDto CurrentDto
        {
            get { return currentDto; }
            set { currentDto = value; RaisePropertyChanged(); }
        }

        private async void Select(ToDoDto dto)
        {
            try
            {
                UpdateLoading(true);
                var todoResult = await toDoService.GetFirstOfDefaultAsync(dto.Id);
                if (todoResult.Status)
                {
                    CurrentDto = todoResult.Result;

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
            CurrentDto = new ToDoDto();

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
        /// <summary>
        /// 获取数据
        /// </summary>
        async Task GetDataAsync()
        {
            UpdateLoading(true);
            var todoResult = await toDoService.GetAllAsync(new Shared.Parameters.QueryParameter
            {
                PageIndex = 0,
                PageSize = 100,
                Search = Search,
            });
            if (todoResult.Status)
            {
                ToDoDtos.Clear();
                foreach (var item in todoResult.Result.Items)
                {
                    ToDoDtos.Add(item);
                }
            }
            UpdateLoading(false);
        }
        public DelegateCommand<string> ExcuteCommand { get; private set; }
        public DelegateCommand<ToDoDto> SelectedCommand { get; private set; }
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


    }
}
