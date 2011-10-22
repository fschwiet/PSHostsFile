using System.IO;

namespace PSHostsFile.Core
{
    public class Add : TransformOperation
    {
        public void AddToFile(string hostName, string address, string hostsFile)
        {
            new Remove().RemoveFromFile(hostName, hostsFile);

            ApplyStreamTransform(hostsFile, (r, w) =>
                {
                    string line;

                    while ((line = r.ReadLine()) != null)
                    {
                        if (HostsFileUtil.IsLineAHostFilesEntry(line))
                        {
                            break;
                        }

                        w.WriteLine(line);
                    }

                    w.WriteLine(address + "\t\t" + hostName);

                    if (line != null)
                        w.WriteLine(line);

                    while ((line = r.ReadLine()) != null)
                    {
                        w.WriteLine(line);
                    }
                });
        }
    }
}
