using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using PSHostsFile.Core;


namespace PSHostsFile.CmdLets
{
    [Cmdlet("get", "HostsFileEntry")]
    public class GetHostsFileEntry : Cmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = @"Hosts filepath (defaults to SystemRoot\system32\drivers\etc\hosts).")]
        [Alias("f")]
        public string FilePath;
        
        protected override void EndProcessing()
        {
            foreach(var entry in HostsFile.Get(FilePath))
                base.WriteObject(entry);
        }
    }
}
