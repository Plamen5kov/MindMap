using BackgroundSoundManager;
using MindMap.Pages;
using MindMap.ViewModels;
using Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace MindMap
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif
        private const int parentOfRoot = 0;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
            
#if WINDOWS_PHONE_APP
            this.HandleBackgroundTaskRequestAndRegister();
#endif

            ParseClient.Initialize("2063MyRUfXR6BAcyaLg8YcCquRwQyFCupFflSMdf", "DU8OBwFxfYkmLAOF0MNZP79ctLT6COC090LbJ3yp");
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                Type startPageType;
                if(ParseUser.CurrentUser != null)
                {
                    startPageType = typeof(MindMapPage);
                }
                else
                {
                    startPageType = typeof(LoginPage);
                }
                if (!rootFrame.Navigate(startPageType, parentOfRoot))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            deferral.Complete();
        }

#if WINDOWS_PHONE_APP
        private async void HandleBackgroundTaskRequestAndRegister()
        {
            var status = await this.RequestBackgroundAccess();
            if((status == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity) ||
                (status == BackgroundAccessStatus.Unspecified) ||
                (status == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity))
            {
                this.RegisterBackgroundTasks();
            }
        }

        private async Task<BackgroundAccessStatus> RequestBackgroundAccess()
        {
            var result = await BackgroundExecutionManager.RequestAccessAsync();

            if (result == BackgroundAccessStatus.Denied)
            {
                //inform user
            }
            return result;
        }

        private void RegisterBackgroundTasks()
        {

            var taskRegistered = false;
            var exampleTaskName = "ExampleBackgroundTask";

            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == exampleTaskName)
                {
                    taskRegistered = true;
                    break;
                }
            }

            if (taskRegistered == false)
            {
                var builder = new BackgroundTaskBuilder();
                var trigger = new SystemTrigger(SystemTriggerType.InternetAvailable, false);
                //var condition = new SystemCondition(SystemConditionType.InternetNotAvailable);

                builder.Name = exampleTaskName;
                builder.TaskEntryPoint = typeof(BackgroundSound).FullName;
                builder.SetTrigger(trigger);
                //builder.AddCondition(condition);

                var taskRegistration = builder.Register();

                taskRegistration.Completed += TaskRegistrationCompleated;
            }

        }
        private void TaskRegistrationCompleated(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            //
            var a = 5;
        }
#endif
    }
}