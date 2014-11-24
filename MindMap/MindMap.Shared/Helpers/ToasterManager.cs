using System;
using System.Collections.Generic;
using System.Text;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace MindMap.Helpers
{
    public static class ToasterManager
    {
        public static void CreateToastWithText(string text)
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastText01;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(text));

            ToastNotification toast = new ToastNotification(toastXml);

            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
