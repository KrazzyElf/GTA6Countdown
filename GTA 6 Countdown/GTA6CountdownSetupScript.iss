[Setup]
AppName=GTA 6 Countdown
AppVersion=1.0.0
DefaultDirName={pf}\GTA 6 Countdown
DefaultGroupName=GTA 6 Countdown
OutputDir=Release
OutputBaseFilename=GTA6CountdownSetup
SetupIconFile=Images\GTA6Logo.ico
Compression=lzma
SolidCompression=yes

[Files]
Source: "bin\Release\net8.0-windows\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\GTA 6 Countdown"; Filename: "{app}\GTA 6 Countdown.exe"; IconFilename: "{app}\GTA6Logo.ico"
Name: "{commondesktop}\GTA 6 Countdown"; Filename: "{app}\GTA 6 Countdown.exe"; IconFilename: "{app}\GTA6Logo.ico"; Tasks: desktopicon
Name: "{userstartup}\GTA 6 Countdown"; Filename: "{app}\GTA 6 Countdown.exe"; IconFilename: "{app}\GTA6Logo.ico"

[Tasks]
Name: "desktopicon"; Description: "Create a &desktop shortcut"; GroupDescription: "Additional icons:"; Flags: checkedonce

[Run]
Filename: "{app}\GTA 6 Countdown.exe"; Description: "Launch GTA 6 Countdown"; Flags: nowait postinstall skipifsilent