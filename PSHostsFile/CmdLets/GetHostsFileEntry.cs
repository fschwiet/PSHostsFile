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
        protected override void EndProcessing()
        {
            foreach(var entry in HostsFile.Get())
                base.WriteObject(entry);
        }
    }
}
