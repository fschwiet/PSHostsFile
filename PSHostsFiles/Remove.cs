using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PSHostsFiles
{
    public class Remove
    {
        public void RemoveFromStream(string serverName, StreamReader hostsFile, StreamWriter results)
        {
            string line;

            while ((line = hostsFile.ReadLine()) != null)
            {
                var match = HostsFileEntryRegex.TryGetHostsFileEntry(line);

                if (match == null)
                    results.WriteLine(line);
                else if (!match.Host.Equals(serverName, StringComparison.InvariantCultureIgnoreCase))
                    results.WriteLine(line);
            }

            results.Flush();
        }

        public void RemoveFromFile(string hostName, string hostsPath)
        {
            var ms = new MemoryStream();
            StreamReader streamReader;

            using (var file = File.Open(hostsPath, FileMode.Open, FileAccess.Read))
            {
                streamReader = new StreamReader(file);
                streamReader.Peek();
                RemoveFromStream(hostName, streamReader, new StreamWriter(ms, streamReader.CurrentEncoding));
            }

            File.Delete(hostsPath);

            File.WriteAllBytes(hostsPath, ms.ToArray());
        }
    }
}
