using System;
using System.IO;

namespace PSHostsFile.Core
{
    public class TransformOperation
    {
        public static void ApplyStreamTransform(string targetFile, Action<StreamReader, StreamWriter> transform)
        {
            var ms = new MemoryStream();
            StreamReader streamReader;

            using (var file = File.Open(targetFile, FileMode.Open, FileAccess.Read))
            {
                streamReader = new StreamReader(file);
                streamReader.Peek();
                var streamWriter = new StreamWriter(ms, streamReader.CurrentEncoding);

                transform(streamReader, streamWriter);
                
                streamWriter.Flush();
            }

            File.Delete(targetFile);

            File.WriteAllBytes(targetFile, ms.ToArray());
        }
    }
}