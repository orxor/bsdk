using namespace System
using namespace System.IO
using namespace System.Text
using namespace System.Xml
using namespace System.Xml.Linq
using namespace System.Collections.Generic
using namespace System.Collections
using namespace System.Globalization

param
    (
    #[String]$Source="C:\TFS\bsdk\src\dotnet\libraries\BinaryStudio.PlatformComponents\Properties\Messages.xml",
    #[String]$Target="C:\TFS\bsdk\src\dotnet\libraries\BinaryStudio.PlatformComponents\Properties\Messages.mc"
    [String]$Source,
    [String]$Target
    )

Add-Type -TypeDefinition @"
 using System;
 using System.Diagnostics;
 using System.Runtime.InteropServices;
 using System.IO;

 public static class Utils
    {
    public static Int32 OR (Int32 x,Int32 y) { return x | y;  }
    public static Int32 AND(Int32 x,Int32 y) { return x & y;  }
    public static Int32 SHL(Int32 x,Int32 y) { return x << y; }
    public static Int32 SHR(Int32 x,Int32 y) { return x >> y; }
    }
"@

$StoredCurrentFolder = (Get-Location).Path
$ExecutingFolder = Split-Path (Get-Variable MyInvocation).Value.MyCommand.Path
$TargetFolder = [Path]::GetDirectoryName($Target)
$TargetResourceFile = [Path]::ChangeExtension($Target,".rc")

Set-Location -Path $ExecutingFolder

Function Dispose([IDisposable]$Object)
    {
    If ($Object -ne $null) {
        $Object.Dispose()
        $Object = $null
        }
    }

Function Int32-Parse([String]$Value)
    {
    If ($Value.StartsWith("0x") -eq $true)
        {
        Return [Int32]::Parse($Value.Substring(2),[System.Globalization.NumberStyles]::HexNumber)
        }
    Else
        {
        Return [Int32]::Parse($Value)
        }
    }

Function Internal-Add-Assembly([String]$Path)
    {
    Try
        {
        Add-Type -Assembly $Path
        }
    Catch [Exception]
        {
        $_.Exception.Data["Assembly"] = $Path;
        Throw
        }
    }

Try
    {
    Internal-Add-Assembly "System.Xml.Linq"
    }
Catch [Exception]
    {
    Write-Host $_.Exception.ToString() -ForegroundColor Red
    $I = 0
    ForEach ($Key In $_.Exception.Data.Keys) {
        If ($I -eq 0) {
            Write-Host "Exception Data:" -ForegroundColor Red
            }
        $Value = $_.Exception.Data[$Key];
        Write-Host "    {$Key}:{$Value}" -ForegroundColor Red
        }
    Exit
    }

[Stream]$SourceStream = $null
[Stream]$TargetStream = $null
[TextWriter]$Writer = $null

$Facilities = @{
    "NULL"=0x0000
    "RPC"=0x0001
    "DISPATCH"=0x0002
    "STORAGE"=0x0003
    "ITF"=0x0004
    "WIN32"=0x0007
    "WINDOWS"=0x0008
    "SECURITY"=0x0009
    "SSPI"=0x0009
    "CONTROL"=0x000a
    "CERT"=0x000b
    "INTERNET"=0x000c
    "MEDIASERVER"=0x000d
    "MSMQ"=0x000e
    "SETUPAPI"=0x000f
    "SCARD"=0x0010
    "COMPLUS"=0x0011
    "AAF"=0x0012
    "URT"=0x0013
    "ACS"=0x0014
    "DPLAY"=0x0015
    "UMI"=0x0016
    "SXS"=0x0017
    "WINDOWS_CE"=0x0018
    "HTTP"=0x0019
    "USERMODE_COMMONLOG"=0x001a
    "WER"=0x001b
    "USERMODE_FILTER_MANAGER"=0x001f
    "BACKGROUNDCOPY"=0x0020
    "CONFIGURATION"=0x0021
    "WIA"=0x0021
    "STATE_MANAGEMENT"=0x0022
    "METADIRECTORY"=0x0023
    "WINDOWSUPDATE"=0x0024
    "DIRECTORYSERVICE"=0x0025
    "GRAPHICS"=0x0026
    "SHELL"=0x0027
    "NAP"=0x0027
    "TPM_SERVICES"=0x0028
    "TPM_SOFTWARE"=0x0029
    "UI"=0x002a
    "XAML"=0x002b
    "ACTION_QUEUE"=0x002c
    "PLA"=0x0030
    "WINDOWS_SETUP"=0x0030
    "FVE"=0x0031
    "FWP"=0x0032
    "WINRM"=0x0033
    "NDIS"=0x0034
    "USERMODE_HYPERVISOR"=0x0035
    "CMI"=0x0036
    "USERMODE_VIRTUALIZATION"=0x0037
    "USERMODE_VOLMGR"=0x0038
    "BCD"=0x0039
    "USERMODE_VHD"=0x003a
    "USERMODE_HNS"=0x003b
    "SDIAG"=0x003c
    "WEBSERVICES"=0x003d
    "WINPE"=0x003d
    "WPN"=0x003e
    "WINDOWS_STORE"=0x003f
    "INPUT"=0x0040
    "QUIC"=0x0041
    "EAP"=0x0042
    "WINDOWS_DEFENDER"=0x0050
    "OPC"=0x0051
    "XPS"=0x0052
    "RAS"=0x0053
    "MBN"=0x0054
    "POWERSHELL"=0x0054
    "EAS"=0x0055
    "P2P_INT"=0x0062
    "P2P"=0x0063
    "DAF"=0x0064
    "BLUETOOTH_ATT"=0x0065
    "AUDIO"=0x0066
    "STATEREPOSITORY"=0x0067
    "VISUALCPP"=0x006d
    "SCRIPT"=0x0070
    "PARSE"=0x0071
    "BLB"=0x0078
    "BLB_CLI"=0x0079
    "WSBAPP"=0x007a
    "BLBUI"=0x0080
    "USN"=0x0081
    "USERMODE_VOLSNAP"=0x0082
    "TIERING"=0x0083
    "WSB_ONLINE"=0x0085
    "ONLINE_ID"=0x0086
    "DEVICE_UPDATE_AGENT"=0x0087
    "DRVSERVICING"=0x0088
    "DLS"=0x0099
    "SOS"=0x00a0
    "DEBUGGERS"=0x00b0
    "DELIVERY_OPTIMIZATION"=0x00d0
    "USERMODE_SPACES"=0x00e7
    "USER_MODE_SECURITY_CORE"=0x00e8
    "USERMODE_LICENSING"=0x00ea
    "DMSERVER"=0x0100
    "SPP"=0x0100
    "RESTORE"=0x0100
    "DEPLOYMENT_SERVICES_SERVER"=0x0101
    "DEPLOYMENT_SERVICES_IMAGING"=0x0102
    "DEPLOYMENT_SERVICES_MANAGEMENT"=0x0103
    "DEPLOYMENT_SERVICES_UTIL"=0x0104
    "DEPLOYMENT_SERVICES_BINLSVC"=0x0105
    "DEPLOYMENT_SERVICES_PXE"=0x0107
    "DEPLOYMENT_SERVICES_TFTP"=0x0108
    "DEPLOYMENT_SERVICES_TRANSPORT_MANAGEMENT"=0x0110
    "DEPLOYMENT_SERVICES_DRIVER_PROVISIONING"=0x0116
    "DEPLOYMENT_SERVICES_MULTICAST_SERVER"=0x0121
    "DEPLOYMENT_SERVICES_MULTICAST_CLIENT"=0x0122
    "DEPLOYMENT_SERVICES_CONTENT_PROVIDER"=0x0125
    "LINGUISTIC_SERVICES"=0x0131
    "WEB"=0x0375
    "WEB_SOCKET"=0x0376
    "AUDIOSTREAMING"=0x0446
    "TTD"=0x05d2
    "ACCELERATOR"=0x0600
    "MOBILE"=0x0701
    "SQLITE"=0x07af
    "UTC"=0x07c5
    "WMAAECMA"=0x07cc
    "WEP"=0x0801
    "SYNCENGINE"=0x0802
    "DIRECTMUSIC"=0x0878
    "DIRECT3D10"=0x0879
    "DXGI"=0x087a
    "DXGI_DDI"=0x087b
    "DIRECT3D11"=0x087c
    "DIRECT3D11_DEBUG"=0x087d
    "DIRECT3D12"=0x087e
    "DIRECT3D12_DEBUG"=0x087f
    "DXCORE"=0x0880
    "LEAP"=0x0888
    "AUDCLNT"=0x0889
    "WINML"=0x0890
    "WINCODEC_DWRITE_DWM"=0x0898
    "DIRECT2D"=0x0899
    "DEFRAG"=0x0900
    "USERMODE_SDBUS"=0x0901
    "JSCRIPT"=0x0902
    "XBOX"=0x0923
    "GAME"=0x0924
    "PIDGENX"=0x0a01
    "PIX"=0x0abc
    "DLT"=0x0dea
    };

[Dictionary[Int32,String]]$Languages=New-Object Dictionary"[Int32,String]"
$Languages[0x0409]="English"
$Languages[0x0419]="Russian"

$SourceStream = [File]::OpenRead($Source)
$Folder = [Path]::GetDirectoryName($Target)
If ([Directory]::Exists($Folder) -eq $false) {
    [Directory]::CreateDirectory($Folder)
    }
$TargetStream = [File]::Create($Target)
[Encoding]$TargetEncoding=[Encoding]::Unicode

Try
    {
    [Dictionary[Int32,Dictionary[Int32,String]]] $SCodes = New-Object Dictionary"[Int32,Dictionary[Int32,String]]"
    [Dictionary[Int32,String]] $FacilityMapping = New-Object Dictionary"[Int32,String]"
    [XDocument]$Document = [XDocument]::Load($SourceStream)
    
    $Writer = New-Object StreamWriter ($TargetStream,$TargetEncoding)
    $Writer.WriteLine("MessageIdTypedef = DWORD")
    $Writer.WriteLine("LanguageNames=")
    $Writer.WriteLine("    (")
    ForEach ($Element in [XPath.Extensions]::XPathSelectElements($Document,"//Message")) {
        $Language = Int32-Parse -Value $Element.Attribute([XName]::Get("Language")).Value
        $SCode    = Int32-Parse -Value $Element.Attribute([XName]::Get("SCode")).Value
        $Message  = $Element.Value
        [Dictionary[Int32,String]] $Container = $null
        If ($SCodes.TryGetValue($SCode,[ref]$Container) -eq $false) {
            $Container = New-Object Dictionary"[Int32,String]"
            $SCodes.Add($SCode,$Container)
            }
        $Container[$Language] = $Message
        }
    ForEach ($Language in $Languages.GetEnumerator()) {
        $Culture = [CultureInfo]::GetCultureInfo($Language.Key)
        $Writer.WriteLine([String]::Format("    {0}=0x{1:x4}:{2}", $Language.Value,$Language.Key,$Culture.TwoLetterISOLanguageName));
        }
    $Writer.WriteLine("    )")
    $Writer.WriteLine([String]::Empty)
    $Writer.WriteLine("SeverityNames=")
    $Writer.WriteLine("    (")
    $Writer.WriteLine("    Success       = 0x00")
    $Writer.WriteLine("    Informational = 0x01")
    $Writer.WriteLine("    Warning       = 0x02")
    $Writer.WriteLine("    Error         = 0x03")
    $Writer.WriteLine("    )")
    $Writer.WriteLine([String]::Empty)
    $Writer.WriteLine("FacilityNames=")
    $Writer.WriteLine("    (")
    ForEach ($Facility in $Facilities.GetEnumerator()) {
        $Writer.WriteLine([String]::Format("    {0}=0x{1:x4}",$Facility.Key,$Facility.Value));
        $FacilityMapping[$Facility.Value]=$Facility.Key
        }
    $Writer.WriteLine("    )")
    $Writer.WriteLine([String]::Empty)
    ForEach ($SCode in $SCodes.Keys) {
        $Writer.WriteLine("MessageId=0x{0:x4}",[Utils]::AND($SCode,0x0000ffff))
        Switch ([Utils]::AND([Utils]::SHR($SCode,30),0x03)) {
            0 { $Writer.WriteLine("Severity=Success"); }
            1 { $Writer.WriteLine("Severity=Informational"); }
            2 { $Writer.WriteLine("Severity=Warning"); }
            3 { $Writer.WriteLine("Severity=Error"); }
            }
        $Facility = [Utils]::AND([Utils]::SHR($SCode,16),0x0fff)
        $Writer.WriteLine("Facility={0}",$FacilityMapping[$Facility])
        ForEach ($Message in $SCodes[$SCode].GetEnumerator()) {
            $Writer.WriteLine("Language={0}",$Languages[$Message.Key])
            $Writer.WriteLine($Message.Value.Trim())
            $Writer.WriteLine(".")
            }
        $Writer.WriteLine([String]::Empty)
        }
    }
Finally
    {
    Dispose($Writer)
    Dispose($SourceStream)
    Dispose($TargetStream)
    }

Set-Location -Path $TargetFolder

$KitRoot10 = (Get-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows Kits\Installed Roots" -Name KitsRoot10).KitsRoot10
$MessageCompiler  = [Path]::Combine($KitRoot10,"bin\x86\mc.exe")
$ResourceCompiler = [Path]::Combine($KitRoot10,"bin\x86\rc.exe")

& $MessageCompiler  -u -A -b $Target
Write-Host "RC: Compiling $TargetResourceFile" -ForegroundColor Gray
& $ResourceCompiler /nologo $TargetResourceFile

Set-Location -Path $StoredCurrentFolder