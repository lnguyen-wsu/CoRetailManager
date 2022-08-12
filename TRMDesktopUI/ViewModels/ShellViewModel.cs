using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    /// <summary>
    /// Lesson 8C: Back end of Login View
    /// </summary>
    public class ShellViewModel: Conductor<object> , IHandle<LogOnEvent>
    {
        
        private IEventAggregator _events;   // Lesson 13c :Adding IHandle<LogOnEvent> and IEventAggregator      
        private ILoggedInUserModel _user;
        private IAPIHelper _apiHelper;
        public ShellViewModel(IEventAggregator events,                           
                              ILoggedInUserModel user,
                              IAPIHelper aPIHelper)
        {
            // section 13c : modified      
            _events = events;                   
            _user = user;
            _apiHelper = aPIHelper;
            _events.SubscribeOnPublishedThread(this);
           
            // end section13c
           
            ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());    // Solution for LoginViewModel : Create new Instance LoginViewModel after successfully logged in so that user credential is not saved
        }

        public bool IsLoggedIn
        {
            get {
                return !string.IsNullOrEmpty(_user.Token);
            }

        }

        public void ExitApplication()
        {
            TryCloseAsync();         
        }

        public async Task UserManagement()
        {
            await ActivateItemAsync(IoC.Get<UserDisplayViewModel>() , new CancellationToken());         
        }
        public async Task LogOut()
        {
            _user.ResetUserModel();
            _apiHelper.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());          
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
        

        public async Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<SalesViewModel>(), cancellationToken);           
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
