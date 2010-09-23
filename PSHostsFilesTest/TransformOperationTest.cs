using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PSHostsFiles;

namespace PSHostsFilesTest
{
    [TestFixture]
    public class TransformOperationTest : ReadWriteScenario
    {
        [Test]
        public void preserves_original_encoding_UTF8()
        {
            preserves_original_encoding(Encoding.UTF8);
        }

        [Test]
        public void preserves_original_encoding_UTF32()
        {
            preserves_original_encoding(Encoding.UTF32);
        }

        public void preserves_original_encoding(Encoding encoding)
        {
            string expectedContents = String.Join("", Enumerable.Range(0, 2048)
                                                      .Select(i => (char) i)
                                                      .Where(c => c != '\n' && c != '\r')
                                                      .Select(c => c.ToString() + "\r\n")
                                                      .ToArray());

            string filename = GetFileWithContents(expectedContents, encoding);

            TransformOperation.ApplyStreamTransform(filename, (r,w) =>
                {
                    string line;
                    while((line = r.ReadLine()) != null)
                    {
                        w.WriteLine(line);
                    }
                });

            Encoding encodingUsed;
            string fileContents = ReadFileContents(filename, out encodingUsed);

            Assert.That(encodingUsed, Is.EqualTo(encoding));
            Assert.That(fileContents, Is.EqualTo(expectedContents));
        }
    }
}
