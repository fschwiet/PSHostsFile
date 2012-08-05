namespace PSHostsFile
{
    public class HostsFileEntry
    {
        public string Address;
        public string Host;

        public override string ToString()
        {
            return string.Format("{0}({1}={2})", this.GetType().Name, Host, Address);
        }
    }
}