using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management.Automation;
using System.Text;

namespace PSHostsFiles.CmdLets
{
    [RunInstaller(true)]
    public class SnapIn : PSSnapIn
    {
        public override string Name
        {
            get { return "PSHostsFile"; }
        }

        public override string Vendor
        {
            get { return "Frank Schwieterman"; }
        }

        public override string Description
        {
            get { return "Commands to work with the system's HOSTS file."; }
        }
    }
}
