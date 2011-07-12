using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using PSHostsFile.Core;

namespace PSHostsFile.CmdLets
{
    [Cmdlet("set", "HostsFileEntry")]
    public class SetHostsFileEntry : Cmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = @"The hostname to be added to c:\windows\system32\drivers\etc\hosts",
            ValueFromPipeline = true)]
        public string HostName;

        [Parameter(Mandatory = true, HelpMessage = @"The static IP address to be associated with the host",
            ValueFromRemainingArguments = true)]
        public string Address;

        protected override void EndProcessing()
        {
            HostsFile.Set(HostName, Address);
        }
    }
}
