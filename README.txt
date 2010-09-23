Per-machine setup:
	installutil /install .\PSHostsFiles.dll
	
Per-Powershell session setup:
	Add-PSSnapin PSHostsFile
	
Commands:
	get-HostsFileEntry
	remove-HostsFileEntry
	set-HostsFileEntry
