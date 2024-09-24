using MyToDo.Extensions;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    public class NavigationViewModel : BindableBase,INavigationAware
    {
        private readonly IContainerProvider containerProvider;
        protected readonly IEventAggregator eventAggregator;
        public NavigationViewModel(IContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;
            eventAggregator = containerProvider.Resolve<IEventAggregator>();
        }
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {

            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
          
        }
        public void UpdateLoading(bool IsOpen)
        {

            eventAggregator.UpdateLoading(new Common.Event.UpdateModel()
            {

                IsOpen = IsOpen
            }
            );



        }
    }
}
