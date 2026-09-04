using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using QRShot.Services;
using QRShot.ViewModels;

namespace QRShot.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Closing += OnClosed;
        Loaded += OnLoaded;
        
        AddHandler(DragDrop.DropEvent, OnDrop);
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        var screenSize = Screens.Primary.WorkingArea.Size;
        var windowSize = PixelSize.FromSize(ClientSize, Screens.Primary.Scaling);

        Position = new PixelPoint(
            screenSize.Width - windowSize.Width,
            0);
        Hide();
    }

    private void OnClosed(object? sender, CancelEventArgs e)
    {
        Hide();
        e.Cancel = true;
    }

    private async Task OnDrop(object? sender, DragEventArgs e)
    {
        if (e.DataTransfer.TryGetFile() is IStorageFile file && DataContext is MainWindowViewModel viewmodel)
        {
            await viewmodel.ProcessImageFile(file);
        }
    }
}