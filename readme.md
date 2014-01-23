Check out Carbon (http://get-carbon.org/) for a much more capable powershell automation libary.

Install via nuget (http://nuget.org/List/Packages/PSHostsFile).  Or run .\build.ps1 and use the PSHostsFile.dll.

Powershell:

    import-module .\PSHostsFile.dll
	        
    get-HostsFileEntry
    remove-HostsFileEntry <hostName>
    set-HostsFileEntry <hostName> <Address>
    
C#:

    using PSHostsFile;

    namespace ConsoleApplication1
    {
        class Program
        {
            static void Main(string[] args)
            {
                foreach (var hostsEntry in HostsFile.Get())
                {
                    System.Console.WriteLine("Have host {0} - {1}.", hostsEntry.Hostname, hostsEntry.Address);
                }

                HostsFile.Set("foo.example.com", "127.0.0.1");  //  Add or update the hosts file entry for foo.example.com
                HostsFile.Set("another.example.com", "127.0.0.1");
                HostsFile.Remove("another.example.com");  //  Remove a hosts file entry
            }
        }
    }
