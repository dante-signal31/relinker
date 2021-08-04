using System;
using System.IO;
using CommandLine;
using TestCommon.Fs;

namespace relinker
{
    public class Program
    {
        public class Options
        {
            [Option('r', "root", Required = true, HelpText = "Base folder to search links from")]
            public string Root { get; set; }

            [Option('o', "original", Required = true, HelpText = "String to be substituted in target paths. If not found nothing is done.")]
            public string StringToBeSubstituted { get; set; }
            
            [Option('m', "modified", Required = true, HelpText = "String to modify target paths.")]
            public string StringToInclude { get; set; }
            
            [Option('v', "verbose", HelpText = "Print details during execution.")]
            public bool Verbose { get; set; }
            
            [Option('s', "simulate", HelpText = "Perform search but do not change link files. Normally used with /v flag.")]
            public bool Simulate { get; set; }
            
            [Option('b', "backup", Required = false, HelpText = "folder to store a backup folder tree with original links.")]
            public string BackupFolder { get; set; }
            
        }

        /// <summary>
        /// Identify link file by its extension.
        /// </summary>
        /// <param name="filePathName">File to check.</param>
        /// <returns>True if file has a lnk extension or false if not.</returns>
        private static bool IsLinkFile(string filePathName)
        {
            string extension = Path.GetExtension(filePathName);
            if ((extension != null) && extension.ToLower().Equals(".lnk"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Main(string[] args)
        {
            bool verbose = false;
            bool simulate = false;
            string stringToModify = null;
            string newString = null;
            string rootPath = null;
            string backupPath = null;
            
            // Parse arguments.
            var parserResult = Parser.Default.ParseArguments<Options>(args);
            parserResult
                .WithParsed<Options>(o =>
                {
                    if (o.Verbose)
                    {
                        verbose = true;
                    }
            
                    if (o.Simulate)
                    {
                        simulate = true;
                    }
            
                    if (o.BackupFolder != null)
                    {
                        backupPath = o.BackupFolder;
                    }
            
                    rootPath = o.Root;
                    stringToModify = o.StringToBeSubstituted;
                    newString = o.StringToInclude;
                });

            // If needed arguments are not provided then abort.
            // Exit codes are taken from:
            //      https://docs.microsoft.com/en-us/windows/win32/debug/system-error-codes
            if (rootPath == null)
            {
                Console.WriteLine("ERROR: Root path not provided.");
                Environment.Exit(3);
            } 
            else if (!Directory.Exists(rootPath))
            {
                Console.WriteLine("ERROR: Root path does not exist.");
                Environment.Exit(3);
            }
            
            if ((stringToModify == null) || (newString == null))
            {
                Console.WriteLine("ERROR: String to be modified or new string not provided.");
                Environment.Exit(87);
            }

            if (backupPath != null)
            {
                if (Directory.Exists(backupPath))
                {
                    if (Directory.GetDirectories(backupPath).Length > 0 || Directory.GetFiles(backupPath).Length > 0)
                    {
                        Console.WriteLine("ERROR: Backup folder is not empty.");
                        Environment.Exit(87);
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: Backup path does not exist.");
                    Environment.Exit(3);
                }
            }

            // Walk through folder tree.
            foreach (FileWalker.Entry entry in FileWalker.FileWalk(rootPath))
            {
                if (entry.IsFile())
                {
                    if (IsLinkFile(entry.Name))
                    {
                        Link link = new Link(entry.Name);
                        string linkTarget = link.Target;
                        string newTarget = linkTarget.Replace(stringToModify, newString);
                        if (backupPath != null)
                        {
                            string fileRelativePath = Path.GetRelativePath(rootPath, entry.Name);
                            Ops.copy_file(entry.Name, Path.Combine(backupPath, fileRelativePath));
                        }
                        if (!simulate)
                        {
                            link.Target = newTarget;
                        }
                        if (verbose)
                        {
                            Console.WriteLine(entry.Name);
                            Console.WriteLine("\t --" + linkTarget);
                            Console.WriteLine("\t ++" + newTarget);
                        }
                    }
                } 
                else if (entry.IsFolder() && (backupPath != null))
                {
                    string folderRelativePath = Path.GetRelativePath(rootPath, entry.Name);
                    Directory.CreateDirectory(Path.Combine(backupPath, folderRelativePath));
                }
            }
        }
    }
}