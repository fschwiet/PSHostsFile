
properties {
	$baseDirectory = (resolve-path .).ToString();
	$buildDirectory = "$baseDirectory\build"
}


task Default -depends Build

task Cleanup {
	rm $buildDirectory -recurse -force
}

task Build {
    $v4_net_version = (ls "$env:windir\Microsoft.NET\Framework\v4.0*").Name
    $solution = "$baseDirectory\PSHostsFiles.sln"

	exec { &"C:\Windows\Microsoft.NET\Framework\$v4_net_version\MSBuild.exe" $solution /T:"Clean,Build" /property:OutDir="$buildDirectory\" }
}
