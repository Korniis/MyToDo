using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Service;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using Prism.Commands;
using Prism.Common;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
namespace MyToDo.ViewModels
{
    public class ToDoViewModel : NavigationViewModel
    {
        private readonly IDialogHostService dialogHost;
        private readonly IToDoService toDoService;

        public ToDoViewModel(IToDoService service, IContainerProvider provider)
            : base(provider)
        {
            ToDoDtos = new ObservableCollection<ToDoDto>();
            ExcuteCommand = new DelegateCommand<string>(Excute);
            SelectedCommand = new DelegateCommand<ToDoDto>(Select);
            DeleteCommand = new DelegateCommand<ToDoDto>(Delete);
            dialogHost = provider.Resolve<IDialogHostService>();
            this.toDoService = service;
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
        private async void Delete(ToDoDto obj)
        {
            try
            {
                var dialogResult = await dialogHost.Question("温馨提示", $"确认删除待办事项:{obj.Title} ?");
                if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
                var deleteResult = await toDoService.DeleteAsync(obj.Id);
                if (deleteResult.Status)
                {
                    ToDoDtos.Remove(obj);
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
        private async void FilterSearch()
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
            int? Status = SelectedIndex == 0 ? null : SelectedIndex == 2 ? 1 : 0;
            var todoResult = await toDoService.GetAllFilterAsync(new ToDoParameter
            {
                PageIndex = 0,
                PageSize = 100,
                Status = Status,
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
       
        public DelegateCommand FilterCommand { get; set; }
        public DelegateCommand<string> ExcuteCommand { get; private set; }
        public DelegateCommand<ToDoDto> SelectedCommand { get; private set; }
        public DelegateCommand<ToDoDto> DeleteCommand { get; private set; }
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
