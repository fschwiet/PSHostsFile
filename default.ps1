
properties {
	$baseDirectory = (resolve-path .).ToString();
	$buildDirectory = "$baseDirectory\build"
}

import-module .\tools\PSUpdateXml.psm1

task Default -depends BuildNuget

task Cleanup {
    if (test-path $buildDirectory) {
	     rm $buildDirectory -recurse -force
    }
}

task Build -depends Cleanup {
    $v4_net_version = (ls "$env:windir\Microsoft.NET\Framework\v4.0*").Name
    $solution = "$baseDirectory\PSHostsFile.sln"

	exec { &"C:\Windows\Microsoft.NET\Framework\$v4_net_version\MSBuild.exe" $solution /T:"Clean,Build" /property:OutDir="$buildDirectory\" }
}

task Test -depends Build {

	exec { & "$baseDirectory\packages\NUnit.Runners.2.6.0.12051\tools\nunit-console.exe" $buildDirectory\PSHostsFileTest.dll /xml "$buildDirectory\TestResults.xml" }
}

task BuildNuget -depends Build,Test {

    $nugetTarget = "$buildDirectory\nuget"

    $null = mkdir "$nugetTarget\lib\"
    $null = mkdir "$nugetTarget\tools\"

    cp "$buildDirectory\PSHostsFile.dll" "$nugetTarget\lib\"
    cp "$buildDirectory\PSHostsFile.pdb" "$nugetTarget\lib\"
    cp "$buildDirectory\PSHostsFile.dll" "$nugetTarget\tools\"
    cp "$buildDirectory\PSHostsFile.pdb" "$nugetTarget\tools\"

	$old = pwd
    cd $nugetTarget

    ..\..\tools\nuget.exe spec -a ".\lib\PSHostsFile.dll"

    update-xml "PSHostsFile.nuspec" {

        set-xml -exactlyOnce "//version" "3.0.5"
        set-xml -exactlyOnce "//owners" "fschwiet"

        set-xml -exactlyOnce "//licenseUrl" "https://github.com/fschwiet/PSHostsFile/blob/master/LICENSE.txt"
        set-xml -exactlyOnce "//projectUrl" "https://github.com/fschwiet/PSHostsFile/"
        remove-xml -exactlyOnce "//iconUrl"
        set-xml -exactlyOnce "//tags" "hosts-file hosts dns"
        set-xml -exactlyOnce "//description" "Change your window's hosts file from C# or Powershell."
        set-xml -exactlyOnce "//releaseNotes" ""

        remove-xml -exactlyOnce "//dependencies"
    }

    ..\..\tools\nuget pack "PSHostsFile.nuspec"

    cd $old
}


