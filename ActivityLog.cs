using System;
using System.Collections.Generic;
using System.Linq;

namespace PART2_POE_
{
    // Records timestamped descriptions of every key action the chatbot takes.
    // Provides methods to retrieve recent or all entries for display.
    public class ActivityLog
    {
        //Using List as Internal storage
        private readonly List<string> _entries = new List<string>();
        public int Count => _entries.Count;

        //Core logging method
        //Appends a timestamped entry to the log.
        //Call this immediately after every significant chatbot action.
        public void LogAction(string description)
        {
            string entry = $"[{DateTime.Now:HH:mm  dd MMM yyyy}]  {description}";
            _entries.Add(entry);
        }

        //Retrieval methods
        public List<string> GetRecent(int maxCount = 10)
        {
            return _entries
                .Skip(Math.Max(0, _entries.Count - maxCount))
                .Reverse()
                .ToList();
        }

        // Returns all entries in chronological order (oldest first).
        public List<string> GetAll()
        {
            return new List<string>(_entries);
        }
        //Removes all entries from the log.
        public void Clear()
        {
            _entries.Clear();
        }
    }
}
