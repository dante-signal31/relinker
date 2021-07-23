using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using relinker;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestIterateTroughFolderTree()
        {
            // Setup test.
            string root = "..\\..\\..\\test_data\\Original root";
            (string, FileWalker.EntryType)[] expectedFileNames = new []
            {
                ("..\\..\\..\\test_data\\Original root", FileWalker.EntryType.Folder),
                ("..\\..\\..\\test_data\\Original root\\Folder 1", FileWalker.EntryType.Folder),
                ("..\\..\\..\\test_data\\Original root\\Folder 1\\Folder A", FileWalker.EntryType.Folder),
                ("..\\..\\..\\test_data\\Original root\\Folder 1\\Folder A\\File_to_ignore_2.txt", FileWalker.EntryType.File),
                ("..\\..\\..\\test_data\\Original root\\Folder 1\\File_to_ignore.txt", FileWalker.EntryType.File),
                ("..\\..\\..\\test_data\\Original root\\Folder 1\\Folder B", FileWalker.EntryType.Folder),
                ("..\\..\\..\\test_data\\Original root\\Folder 2", FileWalker.EntryType.Folder),
                ("..\\..\\..\\test_data\\Original root\\Folder 2\\Folder A.lnk", FileWalker.EntryType.File),
                ("..\\..\\..\\test_data\\Original root\\Folder 2\\Folder B.lnk", FileWalker.EntryType.File),
                ("..\\..\\..\\test_data\\Original root\\Folder 2\\Folder C", FileWalker.EntryType.Folder),
                ("..\\..\\..\\test_data\\Original root\\Folder 3", FileWalker.EntryType.Folder),
                ("..\\..\\..\\test_data\\Original root\\Folder 3\\Folder A.lnk", FileWalker.EntryType.File),
                ("..\\..\\..\\test_data\\Original root\\Folder 3\\Folder C.lnk", FileWalker.EntryType.File)  
            };
            HashSet<FileWalker.Entry> expectedEntries = new HashSet<FileWalker.Entry>();
            foreach ((string name, FileWalker.EntryType type) in expectedFileNames)
            {
                expectedEntries.Add(new FileWalker.Entry(name, type));
            }
            
            // Run test.
            HashSet<FileWalker.Entry> recoveredEntries = new HashSet<FileWalker.Entry>();
            foreach (FileWalker.Entry entry in FileWalker.FileWalk(root))
            {
                recoveredEntries.Add((entry));
            }
            
            Assert.True(expectedEntries.SetEquals(recoveredEntries));
        }
    }
}