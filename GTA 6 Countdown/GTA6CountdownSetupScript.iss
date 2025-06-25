[Setup]
AppName=GTA 6 Countdown
AppVersion=1.1.0
DefaultDirName={autopf}\GTA 6 Countdown
DefaultGroupName=GTA 6 Countdown
PrivilegesRequired=lowest
OutputDir=Release
OutputBaseFilename=GTA6CountdownSetup
SetupIconFile=Images\GTA6Logo.ico
Compression=lzma
SolidCompression=yes

[Files]
Source: "Images\GTA6Logo.ico"; DestDir: "{app}\Images"; Flags: ignoreversion
Source: "bin\Release\net8.0-windows\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\GTA 6 Countdown"; Filename: "{app}\GTA 6 Countdown.exe"; IconFilename: "{app}\Images\GTA6Logo.ico"
Name: "{commondesktop}\GTA 6 Countdown"; Filename: "{app}\GTA 6 Countdown.exe"; IconFilename: "{app}\Images\GTA6Logo.ico"; Tasks: desktopicon
Name: "{userstartup}\GTA 6 Countdown"; Filename: "{app}\GTA 6 Countdown.exe"; IconFilename: "{app}\Images\GTA6Logo.ico"

[Tasks]
Name: "desktopicon"; Description: "Create a &desktop shortcut"; GroupDescription: "Additional icons:"; Flags: checkedonce

[Run]
Filename: "{app}\GTA 6 Countdown.exe"; Description: "Launch GTA 6 Countdown"; Flags: nowait postinstall skipifsilent