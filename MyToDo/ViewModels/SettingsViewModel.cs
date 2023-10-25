using MyToDo.Common.Models;
using MyToDo.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace MyToDo.ViewModels
{
    public class SettingsViewModel:BindableBase
    {
        public SettingsViewModel( IRegionManager regionManager)
        {
            MenuBars = new ObservableCollection<MenuBar>();
            CreateMenuBars();
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            this.regionManager = regionManager;

        }
        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;
            regionManager.Regions[PrismManager.SettingsViewRegionName].RequestNavigate(obj.NameSpace);


        }

        private IRegionNavigationJournal journal;

    public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
    public DelegateCommand GoBackCommand { get; private set; }
    public DelegateCommand GoForwardCommand { get; private set; }
    private readonly IRegionManager regionManager;

    private ObservableCollection<MenuBar> menuBars;

      
        public ObservableCollection<MenuBar> MenuBars
    {
        get { return menuBars; }
        set { menuBars = value; RaisePropertyChanged(); }
    }
    void CreateMenuBars()
    {
        MenuBars.Add(new MenuBar() { Icon = "Home", NameSpace = "SkinView", Title = "个性化" });
        MenuBars.Add(new MenuBar() { Icon = "CodeNotEqual", NameSpace = "NetView", Title = "系统设置" });
        MenuBars.Add(new MenuBar() { Icon = "NotebookPlus", NameSpace = "AboutView", Title = "关于更多" });
       
    }
    }
    
}
