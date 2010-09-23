using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Text;

namespace PSHostsFiles.CmdLets
{
    [Cmdlet("remove", "HostsFileEntry")]
    public class RemoveHostsFileEntry : Cmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = @"The hostname to be removed from c:\windows\system32\drivers\etc\hosts", 
            ValueFromPipeline = true, ValueFromRemainingArguments = true)]
        public string HostName;

        protected override void EndProcessing()
        {
            var hostsPath = Get.GetHostsPath();

            new Remove().RemoveFromFile(HostName, hostsPath);
        }
    }

    [Cmdlet("remove", "FakeDnsEntry")]
    public class FakeDnsEntry : RemoveHostsFileEntry
    {
    }
}
