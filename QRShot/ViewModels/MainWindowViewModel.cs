using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using QRShot.Models;
using QRShot.Services;

namespace QRShot.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial QrImage? QrCode { get; private set; }

    private readonly Window _mainWindow;
    private readonly FileService _fileService;

    [ObservableProperty]
    public partial bool IsClear { get; private set; }

    public MainWindowViewModel(Window mainWindow, ClipboardListener clipboardListener, FileService fileService)
    {
        _mainWindow = mainWindow;
        _fileService = fileService;
        clipboardListener.ClipboardChanged += OnClipboardChanged;

        Clear();
    }

    private async Task OnClipboardChanged(object? sender, EventArgs e)
    {
        var data = await _mainWindow.Clipboard!.TryGetBitmapAsync();
        if (data is not null)
        {
            using var stream = new MemoryStream();
            data.Save(stream, new PngBitmapEncoderOptions());
            var result = await ScannerService.ReadQrCode(stream);
            if (result is not null)
            {
                QrCode = new QrImage(data, result.Text, CheckText(result.Text));
                IsClear = false;
                _mainWindow.Show();
            }
        }
    }

    private bool CheckText(string text)
    {
        return Uri.TryCreate(text, UriKind.Absolute, out _);
    }

    [RelayCommand]
    private async Task OpenLink()
    {
        if (QrCode!.IsLink)
        {
            await _mainWindow.Launcher.LaunchUriAsync(new Uri(QrCode.Content));
            _mainWindow.Close();
        }
    }

    [RelayCommand]
    private void Close()
    {
        _mainWindow.Close();
        Clear();
    }

    [RelayCommand]
    private async Task AddToClipboard()
    {
        var data = new DataTransfer();
        data.Add(DataTransferItem.CreateText(QrCode.Content));
        await _mainWindow.Clipboard.SetDataAsync(data);
    }

    private void Clear()
    {
        QrCode = new QrImage(null, "", false);
        IsClear = true;
    }

    [RelayCommand]
    private async Task SelectFile()
    {
        var file = await _fileService.OpenFileAsync();
        if (file is not null)
        {
            await ProcessImageFile(file);
        }
    }
    
    public async Task ProcessImageFile(IStorageFile file)
    {
        try
        {
            await using var stream = await file.OpenReadAsync();
            var bitmap = new Bitmap(stream);
            
            stream.Position = 0;
            var result = await ScannerService.ReadQrCode(stream);
            if (result is not null)
            {
                QrCode = new QrImage(bitmap, result.Text, CheckText(result.Text));
                IsClear = false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file: {ex.Message}");
        }
    }

    [RelayCommand]
    public void ShowWindow()
    {
        _mainWindow.ShowInTaskbar = true;
        _mainWindow.Show();
        _mainWindow.Activate();
    }
}