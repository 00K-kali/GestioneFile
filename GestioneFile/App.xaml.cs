namespace GestioneFile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                MainPage = new NavigationPage(new MainAndroid());
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                MainPage = new NavigationPage(new MainWinUI());
            }

            
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            const int Width = 1020;
            const int Height = 580;

            window.Width = Width;
            window.Height = Height;
            window.MinimumWidth = Width;
            window.MinimumHeight = Height;
            

            return window;
        }
    }
}
