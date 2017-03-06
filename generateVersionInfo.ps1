$debugVersion = ""
$version = ""

$hasChanges = $false
$diff = git diff-index HEAD
if ($diff) {
    $hasChanges = $true
}

$versionString = Get-Content ".\version.txt"
$versionSegments = $versionString.Split("{.}")
if ($versionSegments.Length -ne 3)
{
    Throw "Файл version.txt должен содержать номер версии в формате X.Y.Z"
}

$currentBranch = git rev-parse --abbrev-ref HEAD
$commitsCount = git rev-list --count HEAD
if (!$hasChanges -and ($currentBranch -eq "master")) {
    $version = "$($versionSegments[0]).$($versionSegments[1]).$($versionSegments[2])"
    $debugVersion = "$version.$commitsCount"
}
else {
    $version = "$($versionSegments[0]).$($versionSegments[1]).$($versionSegments[2]).$($commitsCount + 1)"
    $debugVersion = $version
}

$versionInfo = @"
/* 
    Этот файл генерируется автоматически при сборке проекта.
    Номер версии задаётся в "version.txt" в формате X.Y.Z
   
    "Корректная" версия из 3х цифр (SEMVER) прописывается на dll'ки, только если выполнены условия:
    * в рабочей копии отсутствуют незафиксированные изменения
    * сборка производится из ветки master
    * сборка производится в конфигурации Release
*/

using System.Reflection;

#if DEBUG
[assembly: AssemblyVersion("$debugVersion")]
[assembly: AssemblyInformationalVersion("$debugVersion")]
#else
[assembly: AssemblyVersion("$version")]
[assembly: AssemblyInformationalVersion("$version")]
#endif
"@

Set-Content -Encoding UTF8 -Path "AssemblyVersion.cs" -Value $versionInfo