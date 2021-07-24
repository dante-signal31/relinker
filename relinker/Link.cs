using ShellLink;

namespace relinker
{
    /// <summary>
    /// Class to read target path from a lnk file and to modify it.
    ///
    /// Just creat an instance of this class with link absolute pathname and
    /// read and write its target content using its <c>Target</c> property.
    /// </summary>
    public class Link
    {
        private string _linkPathName;
        private Shortcut _link;
        
        public Link(string linkPathName)
        {
            _linkPathName = linkPathName;
            _link = ShellLink.Shortcut.ReadFromFile(linkPathName);
        }

        public string Target
        {
            get => _link.LinkTargetIDList.Path;
            set
            {
                _link.LinkTargetIDList.Path = value;
                _link.WriteToFile(this._linkPathName);
            }
        }
    }
}