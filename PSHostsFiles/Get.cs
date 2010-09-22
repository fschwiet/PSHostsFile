using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace PSHostsFiles
{
    public class Get
    {
        static public Regex _regexHostsEntry = new Regex(@"\s*(\S+)\s+(\S+)\s*", RegexOptions.Compiled);

        public IEnumerable<HostsFileEntry> LoadFromStream(Stream input)
        {
            List<HostsFileEntry> results = new List<HostsFileEntry>();
            var reader = new StreamReader(input);

            string line;

            while((line = reader.ReadLine()) != null)
            {
                if (line.Trim().Length == 0)
                    continue;
                else if (line.TrimStart().StartsWith("#"))
                    continue;

                var match = _regexHostsEntry.Match(line);

                if (!match.Success)
                    throw new InvalidDataException();

                results.Add(new HostsFileEntry()
                    {
                        Address =  match.Groups[1].Value,
                        Host = match.Groups[2].Value
                    });
            }

            return results;
        }

        public IEnumerable<HostsFileEntry> LoadFromHostsFiles()
        {
            var systemPath = System.Environment.GetEnvironmentVariable("SystemRoot");
            var hostsPath = System.IO.Path.Combine(systemPath, "system32\\drivers\\etc\\hosts");

            if (!File.Exists(hostsPath))
                throw new FileNotFoundException("Hosts file not found at expected location.");

            using(var file = new FileStream(hostsPath, FileMode.Open, FileAccess.Read))
            {
                return LoadFromStream(file);
            }
        }
    }
}