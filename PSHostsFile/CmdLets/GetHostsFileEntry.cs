using System.Management.Automation;


namespace PSHostsFile.CmdLets
{
    [Cmdlet("get", "HostsFileEntry")]
    public class GetHostsFileEntry : Cmdlet
    {
        [Parameter(Mandatory = false, HelpMessage = @"Hosts filepath (defaults to SystemRoot\system32\drivers\etc\hosts).")]
        [Alias("f")]
        public string FilePath;
        
        protected override void EndProcessing()
        {
            foreach(var entry in HostsFile.Get(FilePath))
                base.WriteObject(entry);
        }
    }
}
