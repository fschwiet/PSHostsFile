
properties {
	$baseDirectory = (resolve-path .).ToString();
	$buildDirectory = "$baseDirectory\build"
}

import-module .\tools\PSUpdateXml.psm1

task Default -depends BuildNuget

task Cleanup {
	rm $buildDirectory -recurse -force
}

task Build -depends Cleanup {
    $v4_net_version = (ls "$env:windir\Microsoft.NET\Framework\v4.0*").Name
    $solution = "$baseDirectory\PSHostsFiles.sln"

	exec { &"C:\Windows\Microsoft.NET\Framework\$v4_net_version\MSBuild.exe" $solution /T:"Clean,Build" /property:OutDir="$buildDirectory\" }
}

task Test -depends Build {

	exec { & "$baseDirectory\lib\nunit\nunit-console" $buildDirectory\PSHostsFilesTest.dll /xml "$buildDirectory\TestResults.xml" }
}

task BuildNuget -depends Build,Test {

    $nugetTarget = "$buildDirectory\nuget"

    $null = mkdir "$nugetTarget\lib\"
    $null = mkdir "$nugetTarget\tools\"

    cp "$buildDirectory\PSHostsFiles.dll" "$nugetTarget\lib\"
    cp "$buildDirectory\PSHostsFiles.pdb" "$nugetTarget\lib\"
    cp "$buildDirectory\PSHostsFiles.dll" "$nugetTarget\tools\"
    cp "$buildDirectory\PSHostsFiles.pdb" "$nugetTarget\tools\"

	$old = pwd
    cd $nugetTarget

    ..\..\tools\nuget.exe spec -a ".\lib\PSHostsFiles.dll"

    update-xml "PSHostsFiles.nuspec" {

        add-xmlnamespace "ns" "http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"

        set-xml -exactlyOnce "//ns:version" "3.0"
        set-xml -exactlyOnce "//ns:owners" "fschwiet"

        set-xml -exactlyOnce "//ns:licenseUrl" "https://github.com/fschwiet/PSHostsFiles/blob/master/LICENSE.txt"
        set-xml -exactlyOnce "//ns:projectUrl" "https://github.com/fschwiet/PSHostsFiles/"
        remove-xml -exactlyOnce "//ns:iconUrl"
        set-xml -exactlyOnce "//ns:tags" "BDD, NUnit"

        remove-xml -exactlyOnce "//ns:dependencies"
    }

    ..\..\tools\nuget pack "PSHostsFiles.nuspec"

    cd $old
}


