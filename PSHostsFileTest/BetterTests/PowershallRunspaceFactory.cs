using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using PSHostsFile.CmdLets;

namespace PSHostsFileTest.BetterTests
{
    class PowershallRunspaceFactory
    {
        public static Runspace GetPowershellRunspace()
        {
            var siblingTypeToAllCmdlets = typeof (GetHostsFileEntry);

            var configuration = RunspaceConfiguration.Create();

            foreach (var type in siblingTypeToAllCmdlets.Assembly.GetTypes().Where(t => typeof (Cmdlet).IsAssignableFrom(t)))
            {
                var cmdletAttribute = type.GetCustomAttributes(typeof (CmdletAttribute), false).Single() as CmdletAttribute;
                string name = cmdletAttribute.VerbName + "-" + cmdletAttribute.NounName;
                configuration.Cmdlets.Append(new CmdletConfigurationEntry(name, type, "help_lol.xml"));
            }

            var result = RunspaceFactory.CreateRunspace(configuration);

            result.Open();

            return result;
        }
    }
}
