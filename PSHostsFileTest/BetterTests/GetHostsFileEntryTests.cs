using NJasmine;
using PSHostsFile;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;

namespace PSHostsFileTest.BetterTests
{
    public class GetHostsFileEntryTests : GivenWhenThenFixture
    {
        public override void Specify()
        {
            var configuredRunspace = arrange(() => PowershallRunspaceFactory.GetPowershellRunspace());

            it("the host entries can be read", () =>
            {
                var hostsFile = SampleHostsFile.AsFile();

                using (var command = configuredRunspace.CreatePipeline("get-HostsFileEntry -f `" + hostsFile + "`"))
                {
                    Collection<PSObject> psResults = command.Invoke();

                    var results = psResults.Select(r => r.BaseObject).Cast<HostsFileEntry>();

                    expect(() => results.Single(r => r.Hostname == "www.testserver.com").Address == "127.0.0.1");
                    expect(() => results.Single(r => r.Hostname == "anotherserver.net").Address == "192.168.1.1");
                }
            });
        }
    }
}
