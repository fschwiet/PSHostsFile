using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using NJasmine;
using PSHostsFile;
using PSHostsFile.CmdLets;

namespace PSHostsFileTest.BetterTests
{
    public class Can_test_cmdlet_implementation : GivenWhenThenFixture
    {
        public override void Specify()
        {
            given("the cmdlet is registered", () =>
            {
                var configuredRunspace = arrange(() =>
                {
                    var siblingTypeToAllCmdlets = typeof (GetHostsFileEntry);

                    var configuration = RunspaceConfiguration.Create();

                    foreach (var type in siblingTypeToAllCmdlets.Assembly.GetTypes().Where(t => typeof(Cmdlet).IsAssignableFrom(t)))
                    {
                        var cmdletAttribute = type.GetCustomAttributes(typeof(CmdletAttribute),false).Single() as CmdletAttribute;
                        string name = cmdletAttribute.VerbName + "-" + cmdletAttribute.NounName;
                        configuration.Cmdlets.Append(new CmdletConfigurationEntry(name, type, "help_lol.xml"));
                    }

                    var result = RunspaceFactory.CreateRunspace(configuration);
                    
                    result.Open();

                    return result;
                });

                then("the host entries can be read", () =>
                {
                    var hostsFile = SampleHostsFile.AsFile();

                    using (var command = configuredRunspace.CreatePipeline("get-HostsFileEntry -f `" + hostsFile + "`"))
                    {
                        Collection<PSObject> psResults = command.Invoke();

                        var results = psResults.Select(r => r.BaseObject).Cast<HostsFileEntry>();

                        expect(() => results.Single(r => r.Host == "www.testserver.com").Address == "127.0.0.1");
                        expect(() => results.Single(r => r.Host == "anotherserver.net").Address == "192.168.1.1");
                    }
                });
            });
        }
    }
}
