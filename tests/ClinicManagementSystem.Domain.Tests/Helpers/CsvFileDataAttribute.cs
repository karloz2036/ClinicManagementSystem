using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollector;
using Xunit.Sdk;

namespace ClinicManagementSystem.Domain.Tests.Helpers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CsvFileDataAttribute : DataAttribute
    {
        private readonly string _filePath;
        private readonly bool _hasHeader;
        private readonly char _separator;

        public CsvFileDataAttribute(string filePath, bool hasHeader = true, char separator = ',')
        {
            _filePath = filePath;
            _hasHeader = hasHeader;
            _separator = separator;
        }

        public override IEnumerable<object[]> GetData(System.Reflection.MethodInfo testMethod)
        {
            if (testMethod == null)
                throw new ArgumentNullException(nameof(testMethod));

            var path = _filePath;
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), _filePath);
            }

            if (!File.Exists(path))
                throw new ArgumentException($"Could not find file at path: {path}");

            var lines = File.ReadAllLines(path).ToList();

            if (_hasHeader && lines.Count > 0)
                lines = lines.Skip(1).ToList();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = SplitCsvLine(line, _separator);
                yield return parts.Cast<object>().ToArray();
            }
        }

        private static string[] SplitCsvLine(string line, char separator)
        {
            // Simple CSV splitter - does not support quoted separators. Sufficient for our test data.
            return line.Split(separator).Select(p => p.Trim()).ToArray();
        }
    }
}
