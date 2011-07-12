
Powershell:

    import-module .\PSHostsFile.dll

        
    get-HostsFileEntry
    remove-HostsFileEntry <hostName>
    set-HostsFileEntry <hostName> <Address>
    
C#:

    Similar methods on PSHostsFile.HostsFile.
