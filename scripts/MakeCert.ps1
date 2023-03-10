using namespace System.IO
using namespace BinaryStudio.PlatformComponents.Win32
using namespace BinaryStudio.Security.Cryptography
using namespace BinaryStudio.Security.Cryptography.Specific.CryptoProCSP;

param
    (
    [String]$ProviderType="PROV_GOST_2001_DH",
    [String]$TargetFile="my.cer",
    [DateTime]$NotBefore="2022-01-01",
    [DateTime]$NotAfter="2024-01-01"
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
    Internal-Add-Type "BinaryStudio.Security.Cryptography.dll"
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

Function Dispose([IDisposable]$Object)
    {
    If ($Object -ne $null) {
        $Object.Dispose();
        $Object = $null
        }
    }

[CRYPT_PROVIDER_TYPE]$CryptProviderType = [Enum]::Parse([CRYPT_PROVIDER_TYPE],$ProviderType)

If (($CryptProviderType -eq [CRYPT_PROVIDER_TYPE]::PROV_GOST_2001_DH ) -or
    ($CryptProviderType -eq [CRYPT_PROVIDER_TYPE]::PROV_GOST_2012_256) -or
    ($CryptProviderType -eq [CRYPT_PROVIDER_TYPE]::PROV_GOST_2012_512))
    {
    [CryptKey]$Key=$null
    $Container = [String]::Format("\\.\REGISTRY\{0}",[Guid]::NewGuid().ToString("D").ToLowerInvariant())
    $ContextS  = [CryptographicContext]::AcquireContext($CryptProviderType,$Container,[CryptographicContextFlags]::CRYPT_NEWKEYSET)
    $ContextT  = New-Object CryptoProCSPCryptographicContext -ArgumentList @(,$ContextS)
    Try
        {
        $ContextS.SecureCode = [CryptographicContext]::GetSecureString("SomePassword")
        $Key = [CryptKey]::GenKey($ContextS, [ALG_ID]::AT_SIGNATURE, [CryptGenKeyFlags]::CRYPT_EXPORTABLE)
        $Certificate = $ContextT.CreateSelfSignCertificate("CN=R-CA, C=ru",$NotBefore,$NotAfter)
        [File]::WriteAllBytes($TargetFile,$Certificate.Bytes)
        }
    Finally
        {
        Dispose($Key)
        Dispose($ContextS)
        }
    }
Else
    {
    Write-Host "error: invalid provider type." -ForegroundColor Red
    }