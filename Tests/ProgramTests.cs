using System.IO;
using NUnit.Framework;
using relinker;
using TestCommon.Fs;

namespace Tests
{
    public class ProgramTests
    {
        [Test]
        public void TestBackupMode()
        {
            using (Temp tempFolder = new Temp(Temp.TempType.Folder))
            {
                // Populate temp folder.
                Ops.recreate_folder_tree("..\\..\\..\\test_data\\Original root", tempFolder.TempPath);
                // Create backup folder.
                using (Temp backupTempFolder = new Temp(Temp.TempType.Folder))
                {
                    // Test arguments
                    string[] args = new[]
                    {
                        "--root=" + tempFolder.TempPath,
                        "--original=" + "Original root",
                        "--modified=" + "Modified root",
                        "--backup=" + backupTempFolder.TempPath,
                        "-v",
                        "-s"
                    };
                    Program.Main(args);
                    // Check links have been backup.
                    Assert.True(File.Exists(Path.Combine(backupTempFolder.TempPath, "Folder 2\\Folder A.lnk")));
                    Assert.True(File.Exists(Path.Combine(backupTempFolder.TempPath, "Folder 2\\Folder B.lnk")));
                    Assert.True(File.Exists(Path.Combine(backupTempFolder.TempPath, "Folder 3\\Folder A.lnk")));
                    Assert.True(File.Exists(Path.Combine(backupTempFolder.TempPath, "Folder 3\\Folder C.lnk")));
                }
            }
        }

        [Test]
        public void TestRewriteLinks()
        {
            using (Temp tempFolder = new Temp(Temp.TempType.Folder))
            {
                // Populate temp folder.
                Ops.recreate_folder_tree("..\\..\\..\\test_data\\Original root", tempFolder.TempPath);
                string linkToCheck = Path.Combine(tempFolder.TempPath, "Folder 2\\Folder A.lnk");
                Link link = new Link(linkToCheck);
                string originalPath = link.Target;
                string expectedOriginalPath = @"F:\Desarrollos\relinker\relinker\test_data\Original root\Folder 1\Folder A";
                Assert.True(originalPath.Equals(expectedOriginalPath));
                // Test arguments
                string[] args = new[]
                {
                    "--root=" + tempFolder.TempPath,
                    "--original=" + "Original root",
                    "--modified=" + "Modified root",
                    "-v"
                };
                Program.Main(args);
                // Check link have been modified.
                Link newLink = new Link(linkToCheck);
                string recoveredPath = newLink.Target;
                string expectedRecoveredPath = @"F:\Desarrollos\relinker\relinker\test_data\Modified root\Folder 1\Folder A";
                Assert.True(recoveredPath.Equals(expectedRecoveredPath));
            }
        }
    }
}