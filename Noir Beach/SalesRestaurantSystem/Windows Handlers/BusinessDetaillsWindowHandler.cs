using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Text.Json;
using RestaurantDataManager;
using Microsoft.Win32;
using RestaurantData;

namespace SalesRestaurantSystem.WindowsHandlers
{
    public class BusinessDetaillsWindowHandler : IUIPanel
    {
        private Window _window;
        private Grid _grid;
        private BackButtonController _backButtonController;
        private Action onBackButtonPressed;


        private TextBox _businessNameField;
        private TextBox _tinField;
        private TextBox _addressField;
        private Image _imageField;
        private Button _saveChanges_BTN;
        private Button _upload_BTN;

        private BitmapImage defaultImage;

        private string appDataPath;
        private string appDirectory;
        private string dataPath;
        private string defaultLogo;


        public BusinessDetaillsWindowHandler(Window window)
        {
            _window = window;
            appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            appDirectory = Path.Combine(appDataPath, "CRIAMALDITA");
            dataPath = Path.Combine(appDirectory, "BusinessData.json");
            defaultLogo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Business.png");
        }

        public void SetField(TextBox nameField, TextBox tinField, TextBox addressField, Image imageField)
        {
            _window.Dispatcher.Invoke(() =>
            {
                _businessNameField = nameField;
                _tinField = tinField;
                _addressField = addressField;
                _imageField = imageField;

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(defaultLogo, UriKind.Absolute);
                bitmap.EndInit();
                defaultImage = bitmap;
            });
        }
        public void SetButtons(Button saveChanges, Button uploadButton)
        {
            _window.Dispatcher.Invoke(() =>
            {
                _saveChanges_BTN = saveChanges;
                _upload_BTN = uploadButton;
                _saveChanges_BTN.Click += (o, e) => SetParameters();
                _upload_BTN.Click += (o, e) => UploadImage();
            });
        }
        public void SetParameters()
        {
            _window.Dispatcher.Invoke(() =>
            {
                BusinessData data = new BusinessData()
                {
                    BusinessName = _businessNameField.Text,
                    Tin = _tinField.Text,
                    Address = _addressField.Text,
                };
                BitmapImage bitmap = _imageField.Source as BitmapImage;
                string imgURL = bitmap.UriSource.ToString().Replace("file:///", "");
                data.ImageURL = imgURL;
                DataManager.Instance.Bussiness.SaveData(data);
            });
        }
      
        private BitmapImage UploadImage()
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            _window.Dispatcher.Invoke(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Title = "Select an Image",
                    Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    string newURL = defaultLogo.Replace("Business", "NewLogo");
                    File.Copy(openFileDialog.FileName, newURL, true);
                    image.UriSource = new Uri(newURL, UriKind.Absolute);
                    image.EndInit();
                    _imageField.Source = image;
                }
            });
            return image;
        }
        private void SaveImage(string sourceImagePath)
        {
            string appDirectory = Path.Combine(appDataPath, "CRIAMALDITA", "Images");

            if (!Directory.Exists(appDirectory))
            {
                Directory.CreateDirectory(appDirectory);
            }

            string imagePath = Path.Combine(appDirectory, "userImage.png");
            File.Copy(sourceImagePath, imagePath, true);
        }
        public void SetPanel(Grid grid, BackButtonController backButton)
        {
            _window.Dispatcher.Invoke(() =>
            {
                _grid = grid;
                _backButtonController = backButton;
            });
        }
        public void ShowUI()
        {
            _backButtonController.ShowButton(true);
            _backButtonController.AddListener(GoBack);
            _window.Dispatcher.Invoke(() =>
            {
                BusinessData businessData = DataManager.Instance.Bussiness.LoadData();

                _businessNameField.Text = businessData.BusinessName;
                _tinField.Text = businessData.Tin;
                _addressField.Text = businessData.Address;

                BitmapImage loadedImg = new BitmapImage();
                loadedImg.BeginInit();
                loadedImg.UriSource = new Uri(businessData.ImageURL);
                loadedImg.EndInit();
                _imageField.Source = loadedImg;

                _grid.Visibility = Visibility.Visible;
            });
        }
        public void HideUI()
        {
            _grid.Visibility = Visibility.Hidden;
        }
        public void GoBack()
        {
            HideUI();   
            onBackButtonPressed?.Invoke();
        }
        public void OnBackButtonPressed(Action action)
        {
            onBackButtonPressed += action;
        }

    }
}
