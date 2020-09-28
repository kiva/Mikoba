using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using mikoba.ViewModels;
using Xamarin.Forms;

namespace mikoba.Services
{
    public interface INavigationService
    {
        void RegisterContainerPage();
        Task PopModalAsync();

        Task NavigateToAsync(Page page, NavigationType type = NavigationType.Normal);

        Task NavigateToAsync<TViewModel>(object parameter = null, NavigationType type = NavigationType.Normal) where TViewModel : IBaseViewModel;
        
        Task NavigateToAsync<TViewModel>(TViewModel viewModel, object parameter = null, NavigationType type = NavigationType.Normal) where TViewModel : IBaseViewModel;

        Task AddTabChildToMainView<TViewModel>(TViewModel viewModel, object parameter, int atIndex = -1) where TViewModel : IBaseViewModel;
        
        Task NavigateBackAsync();

        Task RemoveLastFromBackStackAsync();

        Task NavigateToPopupAsync<TViewModel>(bool animate, TViewModel viewModel) where TViewModel : IBaseViewModel;

        Task NavigateToPopupAsync<TViewModel>(object parameter, bool animate, TViewModel viewModel) where TViewModel : IBaseViewModel;

        Task CloseAllPopupsAsync();

        IList<IBaseViewModel> GetMainViewTabChildren();

        bool RemoveTabChildFromMainView(IBaseViewModel childViewModel);

        void SetCurrentTabOnMainView<TViewModel>();

        Type GetCurrentPageViewModel();

        bool SetCurrentPageTitle(string title);

        void AddPageViewModelBinding<TVm, TP>();

        void AddPopupViewModelBinding<TVm, TV>();
    }
}
