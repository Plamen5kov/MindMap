using GalaSoft.MvvmLight;
using MindMap.Common;
using Parse;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace MindMap.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private const string LoginFailedMessage = "login failed";
        private const string RegisterSuccessfulMessage = "registration successful";
        private double DefaultMessageDelayTime = 3.0;

        private ICommand commandLogin;
        private ICommand commandRegister;
        private string notifyMessage;

        public LoginPageViewModel()
        {
            this.User = new UserViewModel()
            {
                Username = "Plamen",
                Password = "asdasd"
            };
        }

        public event EventHandler LoginSuccessfullEvent;

        public string NotifyMessage
        {
            get
            {
                return this.notifyMessage;
            }
            set
            {
                this.notifyMessage = value;
                this.RaisePropertyChanged(() => this.NotifyMessage);
            }
        }

        public UserViewModel User { get; set; }

        public ICommand Login
        {
            get
            {
                if (this.commandLogin == null)
                {
                    this.commandLogin = new RelayCommand(this.PerformLogin);
                }

                return commandLogin;
            }
        }

        public ICommand Register
        {
            get
            {
                if (this.commandRegister == null)
                {
                    this.commandRegister = new RelayCommand(this.PerformRegister);
                }
                return this.commandRegister;
            }
        }

        private void PerformRegister() //relay command requires signature void in stead of task
        {
            RegisterUser();
        }

        private async Task RegisterUser()
        {
            var user = new ParseUser();

            user.Username = this.User.Username;
            user.Password = this.User.Password;

            await user.SignUpAsync();

            this.NotifyMessage = RegisterSuccessfulMessage;
            HideMessageAfter(DefaultMessageDelayTime);
        }

        private async void PerformLogin()
        {
            try
            {
                await ParseUser.LogInAsync(this.User.Username, this.User.Password);

                if (this.LoginSuccessfullEvent != null)
                {
                    this.LoginSuccessfullEvent(this, null); // fire the event from the view thats attached to the event
                }
            }
            catch
            {
                this.NotifyMessage = LoginFailedMessage;
                HideMessageAfter(DefaultMessageDelayTime);
            }
        }

        private void HideMessageAfter(double delayTime)
        {
            var timer = new DispatcherTimer();
            timer.Tick += (snd, args) =>
            {
                this.NotifyMessage = "";
                timer.Stop();
            };
            timer.Interval = TimeSpan.FromSeconds(delayTime);
            timer.Start();
        }
    }
}
