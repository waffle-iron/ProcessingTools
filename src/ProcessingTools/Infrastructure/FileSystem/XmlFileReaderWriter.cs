﻿namespace ProcessingTools.FileSystem
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Xml;
    using ProcessingTools.Constants;
    using ProcessingTools.FileSystem.Contracts;

    public class XmlFileReaderWriter : IXmlFileReaderWriter
    {
        private const int MaximalNumberOfTrialsToGenerateNewFileName = 100;

        private XmlReaderSettings readerSettings;
        private XmlWriterSettings writerSettings;

        public XmlFileReaderWriter()
        {
            this.ReaderSettings = new XmlReaderSettings
            {
                Async = true,
                CloseInput = true,
                DtdProcessing = DtdProcessing.Ignore,
                IgnoreComments = false,
                IgnoreProcessingInstructions = false,
                IgnoreWhitespace = false
            };

            this.WriterSettings = new XmlWriterSettings
            {
                Async = true,
                CloseOutput = true,
                Encoding = Defaults.Encoding,
                Indent = false,
                IndentChars = " ",
                NamespaceHandling = NamespaceHandling.OmitDuplicates,
                NewLineChars = "\n",
                NewLineOnAttributes = false,
                NewLineHandling = NewLineHandling.Replace,
                OmitXmlDeclaration = false,
                WriteEndDocumentOnClose = true
            };
        }

        public XmlReaderSettings ReaderSettings
        {
            get
            {
                return this.readerSettings;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(this.ReaderSettings));
                }

                this.readerSettings = value;
            }
        }

        public XmlWriterSettings WriterSettings
        {
            get
            {
                return this.writerSettings;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(this.WriterSettings));
                }

                this.writerSettings = value;
            }
        }

        public XmlReader GetXmlReader(string fileName, string basePath = null)
        {
            return XmlReader.Create(this.ReadToStream(fileName, basePath), this.ReaderSettings);
        }

        public Stream ReadToStream(string fileName, string basePath = null)
        {
            string path = this.CombineFileName(fileName, basePath);
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return stream;
        }

        public Task Delete(string fileName, string basePath = null)
        {
            return Task.Run(() =>
            {
                string path = this.CombineFileName(fileName, basePath);
                File.Delete(path);
            });
        }

        public async Task<long> Write(Stream stream, string fileName, string basePath = null)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            string path = this.CombineFileName(fileName, basePath);
            using (var writer = XmlWriter.Create(path, this.WriterSettings))
            {
                using (var reader = XmlReader.Create(stream, this.ReaderSettings))
                {
                    await writer.WriteNodeAsync(reader, true);
                    reader.Close();
                }

                await writer.FlushAsync();
                writer.Close();
            }

            var contentLength = new FileInfo(path).Length;
            return contentLength;
        }

        public Task<string> GetNewFilePath(string fileName, string basePath, int length)
        {
            return Task.Run(() =>
            {
                string path = this.CombineFileName(this.GenerateFileName(fileName, length), basePath);
                int pathLength = path.Length;

                string result = path;

                for (int i = 0; (i < MaximalNumberOfTrialsToGenerateNewFileName) && File.Exists(result); ++i)
                {
                    string suffix = i.ToString();
                    result = path.Substring(0, pathLength - suffix.Length) + suffix;
                }

                if (File.Exists(result))
                {
                    throw new ApplicationException("Can not generate unique file name.");
                }

                return result;
            });
        }

        private string GenerateFileName(string prefix, int length)
        {
            Regex matchInvalidFileNameSymbols = new Regex(@"[^A-Za-z0-9_\-\.]");
            string prefixString = matchInvalidFileNameSymbols.Replace(prefix, "_");

            prefixString = Path.GetFileNameWithoutExtension(prefixString);
            prefixString = prefixString.Substring(0, Math.Min(prefixString.Length, length / 2));

            string timeStamp = matchInvalidFileNameSymbols.Replace(DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss"), "-");

            string fileName = $"{prefixString}-{timeStamp}-{Guid.NewGuid().ToString()}";

            return fileName.PadRight(length, 'X')
                .Substring(0, length);
        }

        private string CombineFileName(string fileName, string basePath)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            Regex matchDriveLetter = new Regex(@"\A\W+:/");

            string path = null;
            if (string.IsNullOrWhiteSpace(basePath))
            {
                path = fileName;
            }
            else
            {
                path = Path.Combine(basePath, matchDriveLetter.Replace(fileName, string.Empty));
            }

            return path;
        }
    }
}
