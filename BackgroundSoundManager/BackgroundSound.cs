using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BackgroundSoundManager
{
    public sealed class BackgroundSound : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastText01;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode("Hello from background"));

            ToastNotification toast = new ToastNotification(toastXml);

            ToastNotificationManager.CreateToastNotifier().Show(toast);

            var deferral = taskInstance.GetDeferral();

            //taskInstance.Canceled += (s, e) => { };

            //taskInstance.Progress = 0;

            deferral.Complete();
        }
    }
}
