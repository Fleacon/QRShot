[Setup]
AppName=QRShot
AppVersion=1.0
DefaultDirName={pf}\QRShot
DefaultGroupName=QRShot

[Files]
Source: "QRShot\bin\Release\net10.0\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\QRShot"; Filename: "{app}\QRShot.exe"
Name: "{userdesktop}\QRShot"; Filename: "{app}\QRShot.exe"

[Registry]
Root: HKCU; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "QRShot"; ValueData: """{app}\QRShot.exe"""; Flags: uninsdeletevalue
