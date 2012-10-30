using System;
using System.Collections.Generic;
using System.Text;

namespace Wof.Controller
{
    public class DebugInfo
    {
        private string debugName;
      
        private SortedDictionary<string, string> items;

        private List<string> keysUpdated = new List<string>();

        public DebugInfo(string debugName)
        {
            lock (this)
            {
                this.debugName = debugName;
                items = new SortedDictionary<string, string>();
            }
        }

        public void ClearNotUpdated()
        {
            lock (this)
            {
                foreach (string key in keysUpdated)
                {
                    items.Remove(key);
                }
                keysUpdated.Clear();
            }
        }


        public void Update(string key, string value)
        {
            lock (this)
            {
                if (!keysUpdated.Contains(key))
                {
                    keysUpdated.Add(key);
                }
                items[key] = value;
            }
        }

        public string[] ToStringArray()
        {
            lock (this)
            {
                string[] ret = new string[items.Count + 1];
                int i = 0;
                ret[i] = "Name: " + debugName;

                foreach (KeyValuePair<string, string> item in items)
                {
                    i++;
                    ret[i] = item.Key + ": " + item.Value;
                }

                return ret;
            }
        }

    }
}
