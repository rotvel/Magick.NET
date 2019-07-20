# Copyright 2013-2019 Dirk Lemstra <https://github.com/dlemstra/Magick.NET/>
#
# Licensed under the ImageMagick License (the "License"); you may not use this file except in
# compliance with the License. You may obtain a copy of the License at
#
#   https://www.imagemagick.org/script/license.php
#
# Unless required by applicable law or agreed to in writing, software distributed under the
# License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
# either express or implied. See the License for the specific language governing permissions
# and limitations under the License.

param (
    [string]$quantumName = $env:QuantumName,
    [string]$platformName = $env:PlatformName,
    [string]$pfxPassword = '',
    [parameter(mandatory=$true)][string]$version,
    [parameter(mandatory=$true)][string]$destination
)

. $PSScriptRoot\..\tools\windows\utils.ps1

function addFile($xml, $source, $target) {
    $files = $xml.package.files

    $file = $xml.CreateElement("file", $xml.DocumentElement.NamespaceURI)

    $srcAtt = $xml.CreateAttribute("src")
    $srcAtt.Value = $source
    [void]$file.Attributes.Append($srcAtt)

    $targetAtt = $xml.CreateAttribute("target")
    $targetAtt.Value = $target
    [void]$file.Attributes.Append($targetAtt)

    [void]$files.AppendChild($file)
}

function addLibrary($xml, $library, $quantumName, $platform, $targetFramework) {
    $source = fullPath "src\$library\bin\Release$quantumName\$platform\$targetFramework\$library-$quantumName-$platform.dll"
    $target = "lib\$targetFramework\$library-$quantumName-$platform.dll"
    addFile $xml $source $target

    $source = fullPath "src\$library\bin\Release$quantumName\$platform\$targetFramework\$library-$quantumName-$platform.xml"
    $target = "lib\$targetFramework\$library-$quantumName-$platform.xml"
    addFile $xml $source $target
}

function addMagickNetLibraries($xml, $quantumName, $platform) {
    addLibrary $xml Magick.NET $quantumName $platform "net20"
    addLibrary $xml Magick.NET $quantumName $platform "net40"
    addLibrary $xml Magick.NET $quantumName $platform "netstandard13"
    addLibrary $xml Magick.NET $quantumName $platform "netstandard20"
}

function addNativeLibrary($quantumName, $platform, $runtime, $extension, $destination) {
    $source = fullPath "src\Magick.Native\libraries\Magick.Native-$quantumName-$platform$extension"
    $target = "runtimes\$runtime-$platform\native\Magick.Native-$quantumName-$platform$extension"
    addFile $xml $source $target
}

function addNativeLibraries($xml, $quantumName, $platform) {
    if ($platform -eq "AnyCPU")
    {
        addNativeLibraries $xml $quantumName "x86"
        addNativeLibraries $xml $quantumName "x64"
        return
    }

    addNativeLibrary $quantumName $platform "win" ".dll" $destination

    if (($platform -eq "x64") -and (!$quantumName.EndsWith("-OpenMP"))) {
        addNativeLibrary $quantumName $platform "linux" ".dll.so" $destination
        addNativeLibrary $quantumName $platform "osx" ".dll.dylib" $destination
    }
}

function createAndSignNuGetPackage($name, $version, $pfxPassword) {
    $nuspecFile = fullPath "publish\$name.nuspec"

    $nuget = fullPath "tools\windows\nuget.exe"
    & $nuget pack $nuspecFile -NoPackageAnalysis
    checkExitCode "Failed to create NuGet package"

    if ($pfxPassword.Length -gt 0) {
        $nupkgFile = fullPath "$name*.nupkg"
        $certificate = fullPath "build\windows\ImageMagick.pfx"
        & $nuget sign $nupkgFile -CertificatePath "$certificate" -CertificatePassword "$pfxPassword" -Timestamper http://sha256timestamp.ws.symantec.com/sha256/timestamp
        checkExitCode "Failed to sign NuGet package"
    }
}

function createMagickNetNuGetPackage($quantumName, $platform, $version, $pfxPassword) {
    $name = "Magick.NET-$quantumName-$platform"
    $path = fullPath "publish\Magick.NET.nuspec"
    $xml = [xml](Get-Content $path)
    $xml.package.metadata.id = $name
    $xml.package.metadata.title = $name
    $xml.package.metadata.version = $version
    $xml.package.metadata.releaseNotes = "https://github.com/dlemstra/Magick.NET/releases/tag/$version"

    $platform = $platformName

    if ($platform -eq "Any CPU") {
        $platform = "AnyCPU"
    }

    addMagickNetLibraries $xml $quantumName $platform
    addNativeLibraries $xml $quantumName $platform

    if ($platform -ne "AnyCPU") {
        addFile $xml "Magick.NET.targets" "build\net20\$name.targets"
        addFile $xml "Magick.NET.targets" "build\net40\$name.targets"
    }

    $nuspecFile = FullPath "publish\$name.nuspec"
    $xml.Save($nuspecFile)

    createAndSignNuGetPackage $name $version $pfxPassword
}

function createMagickNetWebNuGetPackage($quantumName, $platform, $version, $pfxPassword) {
    $name = "Magick.NET.Web-$quantumName-$platform"
    $path = fullPath "publish\Magick.NET.Web.nuspec"
    $xml = [xml](Get-Content $path)
    $xml.package.metadata.id = $name
    $xml.package.metadata.title = $name
    $xml.package.metadata.version = $version

    addLibrary $xml Magick.NET.Web $quantumName $platform "net40"

    $nuspecFile = FullPath "publish\$name.nuspec"
    $xml.Save($nuspecFile)

    createAndSignNuGetPackage $name $version $pfxPassword
}

$platform = $platformName

if ($platform -eq "Any CPU") {
    $platform = "AnyCPU"
}

createMagickNetNuGetPackage $quantumName $platform $version $pfxPassword

if (!$quantumName.EndsWith("-OpenMP")) {
    createMagickNetWebNuGetPackage $quantumName $platform $version $pfxPassword
}

Copy-Item "*.nupkg" $destination