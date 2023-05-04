param
    (
    [String]$Culture = $null
    )

Function Internal-Add-Type([String]$Path)
    {
    Try
        {
        Add-Type -Path $Path
        }
    Catch [Exception]
        {
        $_.Exception.Data["Path"] = $Path;
        Throw
        }
    }

Try
    {
    Internal-Add-Type "BinaryStudio.PlatformComponents.dll"
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

[System.Globalization.CultureInfo] $CultureInfo = $null
If (![String]::IsNullOrWhiteSpace($Culture)) {
    $CultureInfo = [System.Globalization.CultureInfo]::GetCultureInfo($Culture)
    }

[Int32] $MaxEnumName = 0
$EnumNames = [Enum]::GetNames([BinaryStudio.PlatformComponents.Win32.HResult])
[Array]::Sort($EnumNames)

ForEach ($EnumName in $EnumNames) {
   $EnumValue = [Enum]::Parse([BinaryStudio.PlatformComponents.Win32.HResult],$EnumName);
   $MaxEnumName = [Math]::Max($MaxEnumName,$EnumName.Length);
   }

#ForEach ($EnumName in $EnumNames) {
#   $EnumValue = [Enum]::Parse([BinaryStudio.PlatformComponents.Win32.HResult],$EnumName);
#   [Int32] $EnumInt = $EnumValue
#   $Output = [String]::Format("        {0,-" + $MaxEnumName + "} = unchecked((Int32)0x{1:x8}),",$EnumName,$EnumInt)
#   Write-Output "$Output"
#   }

ForEach ($EnumName in $EnumNames) {
   $EnumValue = [Enum]::Parse([BinaryStudio.PlatformComponents.Win32.HResult],$EnumName);
   [Int32] $EnumInt = $EnumValue
   $EnumText  = ([BinaryStudio.PlatformComponents.Win32.HResultException]::GetExceptionForHR($EnumValue,$CultureInfo)).Message;
   #$EnumId = [String]::Format("{0:X8}",$EnumInt)
   $Output = [String]::Format("        {0,-" + $MaxEnumName + "} = unchecked((Int32)0x{1:x8}),",$EnumName,$EnumInt)
   Write-Output "$Output"

   }
