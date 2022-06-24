using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    /// <summary>
    /// Lesson 8C: Back end of Login View
    /// </summary>
    public class ShellViewModel: Conductor<object> , IHandle<LogOnEvent>
    {
        
        private IEventAggregator _events;   // Lesson 13c :Adding IHandle<LogOnEvent> and IEventAggregator
        private SalesViewModel _salesVM;    // Lesson 13c : after the listening correct event, open sale windows
        private ILoggedInUserModel _user;                               
        public ShellViewModel( IEventAggregator events, SalesViewModel salesVM ,  ILoggedInUserModel user)
        {
            // section 13c : modified      
            _events = events;        
            _salesVM = salesVM;
            _user = user;
            _events.Subscribe(this);
           
            // end section13c
           
            ActivateItem(IoC.Get<LoginViewModel>());    // Solution for LoginViewModel : Create new Instance LoginViewModel after successfully logged in so that user credential is not saved
        }

        public bool IsLoggedIn
        {
            get {
                return !string.IsNullOrEmpty(_user.Token);
            }

        }

        public void ExitApplication()
        {
            TryClose();         
        }
        public void LogOut()
        {
            _user.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
        // Lesson 13 c: Listen event from the event LoginViewModel
        public void Handle(LogOnEvent message)
        {
            ActivateItem(_salesVM);
            // When it activate saleVM, the sales windows will show up but problem is that 
            // the login windows will be behind but still have user's credential there 
            // we need to fix it ==> The fix has been mentioned as the contructor
            // For the saleView we want to store anything in it, since user can come back and edit them any time
            NotifyOfPropertyChange(() => IsLoggedIn);
        }
    }
}
