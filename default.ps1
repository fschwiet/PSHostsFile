
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

	exec { & "$baseDirectory\packages\NUnit.2.5.10.11092\tools\nunit-console.exe" $buildDirectory\PSHostsFileTest.dll /xml "$buildDirectory\TestResults.xml" }
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

        add-xmlnamespace "ns" "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"

        set-xml -exactlyOnce "//ns:version" "3.0"
        set-xml -exactlyOnce "//ns:owners" "fschwiet"

        set-xml -exactlyOnce "//ns:licenseUrl" "https://github.com/fschwiet/PSHostsFile/blob/master/LICENSE.txt"
        set-xml -exactlyOnce "//ns:projectUrl" "https://github.com/fschwiet/PSHostsFile/"
        remove-xml -exactlyOnce "//ns:iconUrl"
        set-xml -exactlyOnce "//ns:tags" "BDD, NUnit"

        remove-xml -exactlyOnce "//ns:dependencies"
    }

    ..\..\tools\nuget pack "PSHostsFile.nuspec"

    cd $old
}


