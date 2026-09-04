using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using QRShot.Services;
using QRShot.ViewModels;
using QRShot.Views;

namespace QRShot;

public partial class App : Application
{
    public new static App? Current => Application.Current as App;
    
    public IServiceProvider? Services { get; private set; }
    
    private Window? _mainWindow;
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _mainWindow = new MainWindow();
            _mainWindow.ShowInTaskbar = false;

            var services = new ServiceCollection();

            var fileService = new FileService(_mainWindow);
            services.AddSingleton(fileService);
            services.AddSingleton(new ClipboardListener(_mainWindow));

            Services = services.BuildServiceProvider();

            var clipboardListener = Services.GetRequiredService<ClipboardListener>();

            _mainWindow.DataContext = new MainWindowViewModel(
                _mainWindow,
                clipboardListener,
                fileService
            );
            
            desktop.MainWindow = _mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void ShowWindow_OnClick(object? sender, EventArgs e)
    {
        if (_mainWindow is null) 
            return;
        
        _mainWindow.ShowInTaskbar = true;
        _mainWindow.Show();
        _mainWindow.Activate();
    }

    private void Exist_OnClick(object? sender, EventArgs e)
    {
        if (Current.ApplicationLifetime is IControlledApplicationLifetime controlledApplicationLifetime)
        {
            controlledApplicationLifetime.Shutdown(0);
        }
    }
}