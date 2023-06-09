; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "sharpRevit"
#define MyAppVersion "1.0.2"
#define MyAppPublisher "Parallax Team"
#define MyAppURL "http://www.parallaxteam.com/"

#define RevitAddinFolder "{commonappdata}\Autodesk\REVIT\Addins"
#define ProgramFileLocation "{pf64}\sharpRevit"

#define RevitAddin20  RevitAddinFolder+"\2020\"
#define RevitFiles20  ProgramFileLocation+"\2020\"
#define RevitAddin21  RevitAddinFolder+"\2021\"
#define RevitFiles21  ProgramFileLocation+"\2021\"
#define RevitAddin22  RevitAddinFolder+"\2022\"
#define RevitFiles22  ProgramFileLocation+"\2022\"
#define RevitAddin23  RevitAddinFolder+"\2023\"
#define RevitFiles23  ProgramFileLocation+"\2023\"
#define RevitAddin24  RevitAddinFolder+"\2024\"
#define RevitFiles24  ProgramFileLocation+"\2024\"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
SignTool=prlxsign
AppId={{F9C705B6-0608-475E-BF47-DAE81FC557FC}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
UserInfoPage=no
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
LicenseFile=.\LICENSEFREE
CreateAppDir=no
OutputBaseFilename=sharpRevit.v{#MyAppVersion}
SetupIconFile=..\assets\PrlxAppsIcon.ico
UninstallDisplayIcon={uninstallexe}
Compression=lzma
SolidCompression=yes
WizardImageFile=..\assets\banner.bmp
DefaultUserInfoName = {%username|DefaultValue}


[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Components]
Name: "Revit2020"; Description: "sharpRevit 2020";  Types: full custom;
Name: "Revit2021"; Description: "sharpRevit 2021";  Types: full custom;
Name: "Revit2022"; Description: "sharpRevit 2022";  Types: full custom;
Name: "Revit2023"; Description: "sharpRevit 2023";  Types: full custom;
Name: "Revit2024"; Description: "sharpRevit 2024";  Types: full custom;


[Files]

; Revit 2020
Source: "..\_Release\2020\*"; DestDir: "{#RevitFiles20}"; Excludes: "*.pdb,*.xml,*.config,*.addin,*.tmp"; Flags: ignoreversion; Components: Revit2020
Source: "..\_Release\2020\*"; DestDir: "{#RevitAddin20}"; Excludes: "*.pdb,*.xml,*.config,*.dll,*.tmp"; Flags: ignoreversion; Components: Revit2020

; Revit 2021
Source: "..\_Release\2021\*"; DestDir: "{#RevitFiles21}"; Excludes: "*.pdb,*.xml,*.config,*.addin,*.tmp"; Flags: ignoreversion; Components: Revit2021
Source: "..\_Release\2021\*"; DestDir: "{#RevitAddin21}"; Excludes: "*.pdb,*.xml,*.config,*.dll,*.tmp"; Flags: ignoreversion; Components: Revit2021

; Revit 2022
Source: "..\_Release\2022\*"; DestDir: "{#RevitFiles22}"; Excludes: "*.pdb,*.xml,*.config,*.addin,*.tmp"; Flags: ignoreversion; Components: Revit2022
Source: "..\_Release\2022\*"; DestDir: "{#RevitAddin22}"; Excludes: "*.pdb,*.xml,*.config,*.dll,*.tmp"; Flags: ignoreversion; Components: Revit2022

; Revit 2023
Source: "..\_Release\2023\*"; DestDir: "{#RevitFiles23}"; Excludes: "*.pdb,*.xml,*.config,*.addin,*.tmp"; Flags: ignoreversion; Components: Revit2023
Source: "..\_Release\2023\*"; DestDir: "{#RevitAddin23}"; Excludes: "*.pdb,*.xml,*.config,*.dll,*.tmp"; Flags: ignoreversion; Components: Revit2023

; Revit 2024
Source: "..\_Release\2024\*"; DestDir: "{#RevitFiles24}"; Excludes: "*.pdb,*.xml,*.config,*.addin,*.tmp"; Flags: ignoreversion; Components: Revit2024
Source: "..\_Release\2024\*"; DestDir: "{#RevitAddin24}"; Excludes: "*.pdb,*.xml,*.config,*.dll,*.tmp"; Flags: ignoreversion; Components: Revit2024

; NOTE: Don't use "Flags: ignoreversion" on any shared system files

