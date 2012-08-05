using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NJasmine;

namespace PSHostsFileTest.BetterTests
{
    public class Can_stream_input : GivenWhenThenFixture
    {
        public override void Specify()
        {
            var configuredRunspace = arrange(() => PowershallRunspaceFactory.GetPowershellRunspace());

            it("can stream input", () =>
            {
                using(var command = configuredRunspace.CreatePipeline("@(,@(1,2), ,@(2,3)) | % { $_ }"))
                {
                    foreach (var result in command.Invoke().Select(r => r.BaseObject))
                    {
                        Console.WriteLine("{0} {1}", result.GetType().Name, result.ToString());
                    }
                }
            });
        }
    }
}
