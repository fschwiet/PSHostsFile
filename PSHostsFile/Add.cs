using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PSHostsFile
{
    public class Add : TransformOperation
    {
        void SetToStream(string hostName, string address, StreamReader reader, StreamWriter writer)
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (HostsFileUtil.IsLineAHostFilesEntry(line))
                {
                    break;
                }

                writer.WriteLine(line);
            }

            writer.WriteLine(address + "\t\t" + hostName);

            if (line != null)
                writer.WriteLine(line);

            while ((line = reader.ReadLine()) != null)
            {
                writer.WriteLine(line);
            } 
        }

        public void AddToFile(string hostName, string address, string hostsFile)
        {
            ApplyStreamTransform(hostsFile, (r, w) =>
                {
                    this.SetToStream(hostName, address, r, w);
                });
        }
    }
}
