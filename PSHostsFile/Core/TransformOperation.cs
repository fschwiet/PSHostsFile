using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PSHostsFile.Core
{
    public class TransformOperation
    {
        public static void TransformFile(string hostsFile, Func<IEnumerable<string>, IEnumerable<string>> transform)
        {
            var encoding = HostsFileUtil.GetEncoding(hostsFile);
            var contents = File.ReadAllLines(hostsFile);

            var result = transform(contents);

            File.WriteAllLines(hostsFile, result.ToArray(), encoding);
        }
    }
}