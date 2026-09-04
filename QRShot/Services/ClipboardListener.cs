using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform;

namespace QRShot.Services;

public class ClipboardListener
{
    private readonly Window _window;
    private const int WM_CLIPBOARDUPDATE = 0x031D;
    private IPlatformHandle? _handle;
    public delegate Task ClipboardChangedEventHandler(object? sender, EventArgs e);
    public event ClipboardChangedEventHandler? ClipboardChanged;

    public ClipboardListener(Window window)
    {
        _window = window;
        _window.Opened += OnOpened;
        _window.Closed += OnClosed;
    }
    
    private void OnOpened(object? sender, EventArgs e)
    {
        var toplevel = TopLevel.GetTopLevel(_window);
        if (toplevel is null)
            return;
        
        Win32Properties.AddWndProcHookCallback(toplevel, WndProcHook);

        if (toplevel.TryGetPlatformHandle() is { HandleDescriptor: "HWND" } handle)
        {
            _handle = handle;
            ExternalFunctions.AddClipboardFormatListener(handle.Handle);
        }
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        var toplevel = TopLevel.GetTopLevel(_window);
        if (toplevel is null)
            return;
        
        Win32Properties.RemoveWndProcHookCallback(toplevel, WndProcHook);
        
        if(_handle is not null)
            ExternalFunctions.RemoveClipboardFormatListener(_handle.Handle);
    }

    private IntPtr WndProcHook(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == WM_CLIPBOARDUPDATE)
        {
            ClipboardChanged?.Invoke(this, EventArgs.Empty);
            handled = true;
        }
        
        return IntPtr.Zero;
    }
}

internal static partial class ExternalFunctions
{
    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool AddClipboardFormatListener(IntPtr hwnd);
    
    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool RemoveClipboardFormatListener(IntPtr hwnd);
}