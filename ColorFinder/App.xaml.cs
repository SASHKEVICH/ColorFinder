using System.Windows;
using Prism.Ioc;
using Prism.Unity;

namespace ColorFinder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<Services.IUploadService, Services.ImageUploadService>();
        }

        protected override Window CreateShell()
        {
            var window = Container.Resolve<Views.MainWindow>();
            return window;
        }
    }
}