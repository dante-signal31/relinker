using NUnit.Framework;
using relinker;

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

        // [Test]
        // public void TestWriteLink()
        // {
        //     string linkPathName = "..\\..\\..\\test_data\\Original root\\Folder 2\\Folder A.lnk";
        //     
        // }
    }
}