using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Text;
using NJasmine;

namespace PSHostsFileTest.BetterTests
{
    public class SetHostsFileEntryTests : GivenWhenThenFixture
    {
        public override void Specify()
        {
            var configuredRunspace = arrange(() => PowershallRunspaceFactory.GetPowershellRunspace());

            it("can set hosts when hosts file is empty", () =>
            {
                var address = "192.168.0.12";
                var hostName = "someserver.net";

                var hostsFile = ReadWriteScenario.GetFileWithContents(@"", Encoding.UTF8);

                run_set_host_command(configuredRunspace, hostName, address, hostsFile);

                Encoding ignored;
                var result = ReadWriteScenario.ReadFileContents(hostsFile, out ignored);

                expect(() => result == "192.168.0.12\t\tsomeserver.net\r\n");
            });

            it("can set hosts when host file is nonempty", () =>
            {
                var address = "192.168.0.12";
                var hostName = "someserver.net";

                var hostsFile = ReadWriteScenario.GetFileWithContents(@"
#line 1 meh

127.0.0.1           someotherserver.net

", Encoding.UTF8);

                run_set_host_command(configuredRunspace, hostName, address, hostsFile);

                AssertFile.MatchesIgnoringNewlines(hostsFile, @"
#line 1 meh

192.168.0.12" + "\t\t" + @"someserver.net
127.0.0.1           someotherserver.net

");                
            });

            it("can set multiple host files at once", () =>
            {
                var hostsFile = ReadWriteScenario.GetFileWithContents(@"", Encoding.UTF8);

                using (var command = configuredRunspace.CreatePipeline(
                    string.Format("@(@('someserver.net', '192.168.0.12'), @('someOtherServer.net', '192.168.0.1')) | set-HostsFileEntry -f \"{0}\"", hostsFile)))
                {
                    var results = command.Invoke();

                    expect(() => results.Count() == 0);
                }

                Encoding ignored;
                var result = ReadWriteScenario.ReadFileContents(hostsFile, out ignored);

                expect(() => result == "192.168.0.12\t\tsomeserver.net\r\n192.168.0.1\t\tsomeOtherServer.net\r\n");
            });
        }

        void run_set_host_command(Runspace configuredRunspace, string hostName, string address, string hostsFile)
        {
            using (var command = configuredRunspace.CreatePipeline(
                string.Format("set-HostsFileEntry {0} {1} -f \"{2}\"", hostName, address, hostsFile)))
            {
                var results = command.Invoke();

                expect(() => results.Count() == 0);
            }
        }
    }
}
