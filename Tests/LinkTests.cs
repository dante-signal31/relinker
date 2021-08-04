using System.IO;
using NUnit.Framework;
using relinker;
using TestCommon.Fs;

namespace Tests
{
    public class LinkTests
    {

        [Test]
        public void TestReadLink()
        {
            string linkPathName = "..\\..\\..\\test_data\\Original root\\Folder 2\\Folder A.lnk";
            // Be aware you should change this path for your own to test in your box.
            string expectedPath = "F:\\Desarrollos\\relinker\\relinker\\test_data\\Original root\\Folder 1\\Folder A";
            
            // Test reading.
            relinker.Link link = new Link(linkPathName);
            string recoveredPath = link.Target;
            Assert.True(expectedPath.Equals(recoveredPath));
        }

        [Test]
        public void TestWriteLink()
        {
            string linkPathName = "..\\..\\..\\test_data\\Original root\\Folder 2\\Folder A.lnk";
            using (var tempFolder = new Temp(Temp.TempType.Folder))
            {
                // Copy test link to a temporal location. I don't want to mess with original.
                string baseLinkName = Path.GetFileName(linkPathName);
                string temporalLinkPathName = Path.Combine(tempFolder.TempPath, baseLinkName);
                Ops.copy_file(linkPathName, temporalLinkPathName);
                // Test changing path.
                relinker.Link link = new Link(temporalLinkPathName);
                string originalPath = link.Target;
                string expectedOriginalPath = "F:\\Desarrollos\\relinker\\relinker\\test_data\\Original root\\Folder 1\\Folder A";
                Assert.True(originalPath.Equals(expectedOriginalPath));
                string modifiedPath = "F:\\Desarrollos\\relinker\\relinker\\test_data\\Modified folder\\Folder 1\\Folder A";
                link.Target = modifiedPath;
                // Link path should have been updated. Check if actually it is.
                relinker.Link newLink = new Link(temporalLinkPathName);
                string recoveredPath = newLink.Target;
                Assert.True(recoveredPath.Equals(modifiedPath));
            }
        }
    }
}