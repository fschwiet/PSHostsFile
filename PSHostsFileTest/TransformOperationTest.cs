﻿using NUnit.Framework;
using PSHostsFile.Core;
using System;
using System.Linq;
using System.Text;

namespace PSHostsFileTest
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

            TransformOperation.TransformFile(filename, lines => lines);

            Encoding encodingUsed;
            string fileContents = ReadFileContents(filename, out encodingUsed);

            Assert.That(encodingUsed, Is.EqualTo(encoding));
            Assert.That(fileContents, Is.EqualTo(expectedContents));
        }
    }
}
