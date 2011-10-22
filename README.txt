Install via nuget (http://nuget.org/List/Packages/PSHostsFile).  Or run .\build.ps1 and use the PSHostsFile.dll.

Powershell:

    import-module .\PSHostsFile.dll

        
    get-HostsFileEntry
    remove-HostsFileEntry <hostName>
    set-HostsFileEntry <hostName> <Address>
    
C#:

    Similar methods on PSHostsFile.HostsFile.
