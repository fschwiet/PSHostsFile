using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PSHostsFiles;

namespace PSHostsFilesTest
{
    [TestFixture]
    public class RemoveTest
    {
        [Test]
        public void can_remove_an_entry()
        {
            var expectedString = SampleHostsFile.AsString.Replace("\r\n192.168.1.1         anotherserver.net", "");
            Assert.That(expectedString, Is.Not.EqualTo(SampleHostsFile.AsString), "verify replace setup actualy did something.");

            var hostsFile = SampleHostsFile.AsStreamReader();

            var result = new MemoryStream();
            var resultStream = new StreamWriter(result);

            var sut = new Remove();

            sut.RemoveFromStream("anotherserver.net", hostsFile, resultStream);

            result.Seek(0, SeekOrigin.Begin);
            Assert.That(new StreamReader(result).ReadToEnd(), Is.EqualTo(expectedString));
        }

        [Test]
        public void can_read_and_write_to_the_same_file_UTF8()
        {
            can_read_and_write_to_the_same_file(Encoding.UTF8);
        }

        [Test]
        public void can_read_and_write_to_the_same_file_UTF32()
        {
            can_read_and_write_to_the_same_file(Encoding.UTF32);
        }

        public void can_read_and_write_to_the_same_file(Encoding encoding)
        {
            var filename = Path.Combine(Path.GetTempPath(), "PSHostsFileTest.temp.hosts");
            if (File.Exists(filename))
                File.Delete(filename);

            File.WriteAllText(filename, @"
127.0.0.1           localhost
10.90.82.100        somehost

", encoding);

            var sut = new Remove();
            sut.RemoveFromFile("somehost", filename);

            using (var stream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Read))
            {
                var streamReader = new StreamReader(stream);
                var result = streamReader.ReadToEnd();

                Assert.That(streamReader.CurrentEncoding, Is.EqualTo(encoding));
                Assert.That(result, Is.EqualTo(@"
127.0.0.1           localhost

"));
            }
        }
    }
}
