//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.IO;
using System.Collections;
using System.Diagnostics;

using nanoFramework.Json;

namespace nanoFramework.Hosting.Configuration.Json
{
    internal sealed class JsonConfigurationParser
    {
        private readonly Hashtable _data = new Hashtable();

        public static Hashtable Parse(Stream input)
            => new JsonConfigurationParser().ParseStream(input);

        private Hashtable ParseStream(Stream input)
        {
            try
            {
                var jsonHashtable = (Hashtable)JsonConvert.DeserializeObject(input, typeof(Hashtable));

                foreach (DictionaryEntry entry in jsonHashtable)
                {
                    foreach (DictionaryEntry value in GetValues(entry))
                    {
                        Debug.WriteLine($"{value.Key} = {value.Value}");
                        _data.Add(value.Key, value.Value);
                    }
                }
            }
            catch 
            {
                throw new FormatException();
            }

            return _data;
        }

        private static Hashtable GetValues(DictionaryEntry dictionaryEntry)
        {
            var items = new Hashtable();
            
            if (dictionaryEntry.Value is Hashtable hashtable)
            {
                foreach (DictionaryEntry entry in hashtable)
                {
                    var key = string.Concat(dictionaryEntry.Key, ":", entry.Key);                    
                    var values = GetValues(new DictionaryEntry(key, entry.Value));

                    foreach (DictionaryEntry item in values)
                    {
                        items.Add(item.Key.ToString(), item.Value.ToString());
                    }
                }

                return items;
            }

            if (dictionaryEntry.Value is ArrayList arrayList)
            {
                for(int i = 0; i < arrayList.Count; i++)
                {
                    var key = string.Concat(dictionaryEntry.Key, ":", i);
                    var values = GetValues(new DictionaryEntry(key, arrayList[i]));

                    foreach (DictionaryEntry item in values)
                    {
                        items.Add(item.Key.ToString(), item.Value.ToString());
                    }
                }

                return items;
            }

            items.Add(dictionaryEntry.Key.ToString(), dictionaryEntry.Value.ToString());

            return items;
        }
    }
}

