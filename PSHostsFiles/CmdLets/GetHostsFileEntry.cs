using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;


namespace PSHostsFiles.CmdLets
{
    [Cmdlet("get", "HostsFileEntry")]
    public class GetHostsFileEntry : Cmdlet
    {
        protected override void EndProcessing()
        {
            foreach(var entry in new Get().LoadFromHostsFiles())
                base.WriteObject(entry);
        }
    }

    [Cmdlet("get", "FakeDnsEntry")]
    public class GetFakeDnsEntry : GetHostsFileEntry
    {
    }
}
