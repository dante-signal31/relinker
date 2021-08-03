using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Cryptography;

namespace relinker
{
    public class FileWalker
    {
        /// <summary>
        /// FileWalker returns two kind of objects: File and Folder.
        /// </summary>
        public enum EntryType
        {
            File,
            Folder
        }

        /// <summary>
        /// Type returned by FileWalk iterator.
        /// </summary>
        public struct Entry: IEqualityComparer<Entry>
        {
            private string _name;
            private EntryType _type;

            /// <summary>
            /// Create a new Entry object.
            /// </summary>
            /// <param name="Name">Entry pathname.</param>
            /// <param name="Type">Type pathname.</param>
            public Entry(string Name, EntryType Type)
            {
                this._name = Name;
                this._type = Type;
            }

            /// <summary>
            /// Read-write property to entry pathname.
            /// </summary>
            public string Name
            {
                get => _name;
                set => _name = value;
            }

            /// <summary>
            /// Read-write property to entry type.
            /// </summary>
            public EntryType Type
            {
                get => _type;
                set => _type = value;
            }

            public bool IsFile() => this._type == EntryType.File ? true : false;
            public bool IsFolder() => this._type == EntryType.Folder ? true : false;
            public bool Equals(Entry x, Entry y)
            {
                return ((x.Name.Equals(y.Name)) && (x.Type.Equals(y.Type)));
            }

            public int GetHashCode(Entry obj)
            {
                string hashString = obj.Name;
                switch (obj.Type)
                {
                    case FileWalker.EntryType.File:
                        hashString += 1;
                        break;
                    case FileWalker.EntryType.Folder:
                        hashString += 2;
                        break;
                }
                return hashString.GetHashCode();
            }
        }
        
        /// <summary>
        /// Iterate through a folder tree starting at given root folder yielding every entry found.
        ///
        /// Yielded entries can be of both types, files and folder identified by Type attribute.
        /// </summary>
        /// <param name="root">Base path where start to iterate.</param>
        /// <returns>Entry</returns>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<Entry> FileWalk(string root)
        {
            if (!System.IO.Directory.Exists(root))
            {
                throw new ArgumentException("Given folder root does not exist.");
            }
            
            Stack<Entry> entries = new Stack<Entry>();
            Entry initial_entry = new Entry(root, EntryType.Folder);
            entries.Push(initial_entry);
            
            while (entries.Count > 0)
            {
                Entry entry = entries.Pop();
                yield return entry;
                if (entry.IsFolder())
                {
                    foreach (string subFile in System.IO.Directory.GetFiles(entry.Name))
                    {
                        yield return new Entry(subFile, EntryType.File);
                    }
                    foreach (string subFolder in System.IO.Directory.GetDirectories(entry.Name))
                    {
                        entries.Push(new Entry(subFolder, EntryType.Folder));
                    }
                } 
                else if (entry.IsFile())
                {
                    yield return entry;
                }
            }
        }
    }
}