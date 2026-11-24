using System;

namespace YMH219_KeyEventLogger
{
    public class KeyLogEntry
    {
        public string KeyName { get; }
        public string EventType { get; }
        public DateTime Time { get; }

        public KeyLogEntry(string keyName, string eventType)
        {
            KeyName = keyName;
            EventType = eventType;
            Time = DateTime.Now;
        }

        public override string ToString()
            => $"{Time:HH:mm:ss.fff} | {EventType,-4} | {KeyName}";
    }
}