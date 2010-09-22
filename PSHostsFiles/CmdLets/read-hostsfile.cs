using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;


namespace PSHostsFiles.CmdLets
{
    [Cmdlet("read", "hostsfile")]
    public class ReadHostsFile : Cmdlet
    {
        protected override void EndProcessing()
        {
            foreach(var entry in new Get().LoadFromHostsFiles())
                base.WriteObject(entry);
        }
    }
}
