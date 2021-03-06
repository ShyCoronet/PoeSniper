using Newtonsoft.Json;
using PoeSniperUI;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace PoeSniper
{
    public partial class MainWindow : Window
    {
        private ApplicationViewModel ApplicationView;
        private string sessionFilePath;

        public MainWindow()
        {
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
            sessionFilePath = Path.Combine(GetStoragePath(), "session.json");
            InitializeComponent();
            GetLastSessionState();
            this.Closed += SaveLastSessionState;
            if (ApplicationView == null) ApplicationView = new ApplicationViewModel();
            DataContext = ApplicationView;
        }

        private void GetLastSessionState()
        {
            try
            {
                using (StreamReader streamReader = File.OpenText(sessionFilePath))
                {
                    using (JsonTextReader reader = new JsonTextReader(streamReader))
                    {
                        JsonSerializer jsonSerializer = new JsonSerializer();
                        ApplicationView = (ApplicationViewModel)jsonSerializer.Deserialize(reader,
                            typeof(ApplicationViewModel));
                    }
                }
            }
            catch { }
        }

        private void SaveLastSessionState(object sender, EventArgs e)
        {
            using (StreamWriter streamWriter = File.CreateText(sessionFilePath))
            {
                JsonSerializer jsonSerializer = new JsonSerializer();
                jsonSerializer.Serialize(streamWriter, ApplicationView);
            }
        }

        private void CloseWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) this.Close();
        }

        private void ChangeWindowSize(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = this.WindowState != WindowState.Normal ? 
                WindowState.Normal : WindowState.Maximized; 
        }

        private void MinimizeWindow(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) this.DragMove();
        }

        private string GetStoragePath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appStoragePath = Path.Combine(appDataPath, "PoeSniper");

            if (Directory.Exists(appDataPath)) Directory.CreateDirectory(appStoragePath);

            return appStoragePath;
        }
    }
}