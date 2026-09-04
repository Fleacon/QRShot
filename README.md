# QRShot
QRShot is a lightweight Windows desktop app that automatically scans screenshots for QR codes and instantly pops up when it detects one. Decode links and text without leaving your workflow.

## Features
- Take a screenshot with a QR code and QRShot detects it automatically
- Window appears on detection, disappearing when you're done
- Open URLs or copy the content of the QR code
- No polling - Uses win32 API clipboard listeners for instant detection
- Runs in the background
- Click the system tray icon to drag & drop images or browse files

## How it works
1. Run QRShot in your system tray
2. Take a screenshot containing a QR code
3. QRShot detects it and pops up automatically
4. Click to open the link or copy the decoded text
5. Window closes when you're done

Alternatively, click the system tray icon anytime to manually scan an image via drag & drop or file browser.

## Installation
Download the latest release and extract to your preferred location

**Requirements**
- Windows 10 or later
- .NET 8 Runtime

## Dependencies
- Avalonia UI - Cross-platform UI framework
- ZXing.Net - QR code detection and decoding
- Win32 API - Clipboard monitoring (Windows-only)

## Remarks
QRShot uses the ClipboardFormatListener Win32 API for instant clipboard change notifications. This enables the auto-detect feature without constant polling. Linux and macOS versions would require platform-specific alternatives and are not currently planned.

## Contributing
Contributions welcome! Feel free to open issues or PRs.