using System;
using System.Collections.Generic;
using System.IO;

namespace YMH219_KeyEventLogger
{
    public class KeyLogManager
    {
        private readonly List<KeyLogEntry> _entries = new List<KeyLogEntry>();

        public bool IsRecording { get; private set; }
        public int Count => _entries.Count;

        public void Start() => IsRecording = true;
        public void Stop() => IsRecording = false;
        public void Clear() => _entries.Clear();

        public KeyLogEntry Add(string keyName, string eventType)
        {
            var entry = new KeyLogEntry(keyName, eventType);
            _entries.Add(entry);
            return entry;
        }

        public void SaveToFile(string path)
        {
            using (var sw = new StreamWriter(path, false))
            {
                sw.WriteLine($"Kayıt tarihi: {DateTime.Now}");
                foreach (var e in _entries)
                    sw.WriteLine(e.ToString());
            }
        }
    }
}