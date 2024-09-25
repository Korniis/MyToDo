using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Views;
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
    public class MainViewModel : BindableBase, IConfigureService
    {
        public MainViewModel(IContainerProvider containerProvider, IRegionManager regionManager)
        {
            MenuBars = new ObservableCollection<MenuBar>();
   
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            this.regionManager = regionManager;
            GoBackCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoBack) { journal.GoBack();   }
            });
            GoForwardCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoForward) { journal.GoForward(); }
            });
            LoginOutCommand = new DelegateCommand(() =>
            {
                //注销当前用户
                App.LoginOut(containerProvider);
            });
            this.containerProvider = containerProvider;
        }
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }
        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, back =>
            {
                journal = back.Context.NavigationService.Journal;
            });
        }
        private MenuBar selectedMenuBar;
        public MenuBar SelectedMenuBar
        {
            get { return selectedMenuBar; }
            set { selectedMenuBar = value; RaisePropertyChanged(); }
        }
        private IRegionNavigationJournal journal;
        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }
        public DelegateCommand LoginOutCommand { get; private set; }

        private readonly IContainerProvider containerProvider;
        private readonly IRegionManager regionManager;
        private ObservableCollection<MenuBar> menuBars;
        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }
        void CreateMenuBars()
        {
            MenuBars.Add(new MenuBar() { Icon = "Home", NameSpace = "IndexView", Title = "首页" });
            MenuBars.Add(new MenuBar() { Icon = "CodeNotEqual", NameSpace = "ToDoView", Title = "待办事项" });
            MenuBars.Add(new MenuBar() { Icon = "Note", NameSpace = "MemoView", Title = "备忘录" });
            MenuBars.Add(new MenuBar() { Icon = "Net", NameSpace = "NetView", Title = "上网" });
            MenuBars.Add(new MenuBar() { Icon = "Cog", NameSpace = "SettingsView", Title = "设置" });
        }
        /// <summary>
        /// 配置首页初始化参数
        /// </summary>
        /// <param name="configureService"></param>
        public void Configure( )
        {
            UserName = AppSession.UserName;
            CreateMenuBars();
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("MainView");
        }
    }
}
