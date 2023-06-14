using DLC_3.Core;
using DLC_3.MVVM.ViewModel;
using DLC_3.NET;
using DLC_3.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows;

namespace DLC_3
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    
    public partial class App : Application
    {

        private readonly IServiceProvider _serviceProvider;
        public App()
        {
            IServiceCollection services = new ServiceCollection();            
            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });
            
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<CViewModel>();
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<Func<Type, ViewModel>>(serviceProvider =>
                                  viewModelType =>(ViewModel)serviceProvider.GetRequiredService(viewModelType));

            _serviceProvider = services.BuildServiceProvider();
        }

        

        protected override void OnStartup(StartupEventArgs e)
        {
            
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();            
            base.OnStartup(e);
        }

    }
}
