using GalaSoft.MvvmLight;
using MindMap.Common;
using MindMap.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.Media.Capture;
using MindMap.Models;

namespace MindMap.ViewModels
{
    public class NodeDetailsViewModel : ViewModelBase
    {
        private const string DbName = "Nodes.db";
        private ICommand commandSave;
        private Windows.Media.Capture.MediaCapture takePhotoManager;
        private Node node;

        public NodeDetailsViewModel()
        {
            this.Node = new Node();
        }

        public event EventHandler SaveDetailsForNodeEvent;

        public Node Node { get; set; }

        public CaptureElement PhotoPreview { get; set; }

        public string SelectedPicturePath { get; set; }

        public int CurrentNodeId { get; set; }

        public ICommand Save
        {
            get
            {
                if (this.commandSave == null)
                {
                    this.commandSave = new RelayCommand(this.SaveNode);
                }
                return this.commandSave;
            }
        }

        public async void InitializePicture(CaptureElement photoPreview)
        {
#if WINDOWS_APP

            try
            {
                //rootPage.NotifyUser("", NotifyType.StatusMessage);

                // Using Windows.Media.Capture.CameraCaptureUI API to capture a photo
                CameraCaptureUI dialog = new CameraCaptureUI();
                Size aspectRatio = new Size(8, 3);
                dialog.PhotoSettings.CroppedAspectRatio = aspectRatio;

                StorageFile file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);
                if (file != null)
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        bitmapImage.SetSource(fileStream);
                    }

                    //if you reached this line you have the picture in bitmapImage 
                }
                else
                {
                    //picture was not captured
                }
            }
            catch (Exception ex)
            {
                //something went horribly wrong
            }
#elif WINDOWS_PHONE_APP
            // capture pic
            this.takePhotoManager = new Windows.Media.Capture.MediaCapture();
            await takePhotoManager.InitializeAsync();

            photoPreview.Source = takePhotoManager;
            await takePhotoManager.StartPreviewAsync();
            // to stop it
            //await takePhotoManager.StopPreviewAsync();

#endif
        }

#if WINDOWS_PHONE_APP
        public async void TakePicture()
        {
            ImageEncodingProperties imgFormat = ImageEncodingProperties.CreateJpeg();

            // a file to save a photo
            StorageFile picFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(
                    "Photo.jpg", CreationCollisionOption.ReplaceExisting);

            await takePhotoManager.CapturePhotoToStorageFileAsync(imgFormat, picFile);
        }
#endif

        private async void SaveNode()
        {
            // update passed element
            await SQLiteCrud.Instance.UpdateArticleTitleAsync(DbName, this.CurrentNodeId, this.Node.Title, this.Node.Content);

            if (this.SaveDetailsForNodeEvent != null)
            {
                this.SaveDetailsForNodeEvent(this, null);
            }
        }
    }
}
