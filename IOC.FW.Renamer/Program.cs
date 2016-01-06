using System;
using System.Linq;
using System.IO;
using System.Configuration;

namespace IOC.FW.Renamer
{
    public class Program
    {
        private static readonly string MARK = ConfigurationManager.AppSettings["MARK"];
        private static readonly string[] WHITE_LIST = ConfigurationManager.AppSettings["WHITELIST"].Split(
            new char[] { ',' }, 
            StringSplitOptions.RemoveEmptyEntries
        );

        static int Main(string[] args)
        {
            if (args.Length >= 3)
                Rename(args[0], args[1], args[2]);
            else
                Console.WriteLine("Pass two arguments: {{customer name}} and {{project name}}");
            
            return 1;
        }

        static void Rename(string customer, string project, string dir)
        {
            var appName = AppDomain.CurrentDomain.FriendlyName.Replace(".exe", string.Empty);
            var solutionName = string.Concat(customer, ".", project);
#if DEBUG
            appName = appName.Replace(".vshost", string.Empty);
#endif

            var foldersToRename = Directory
                .EnumerateDirectories(
                    dir, 
                    $"*{MARK}*", 
                    SearchOption.AllDirectories
                );

            var filesToRename = Directory
                .EnumerateFiles(dir, "*.*", SearchOption.AllDirectories)
                .Select(file => new FileInfo(file))
                .Where(file =>
                    !file.Name.Contains(appName)
                    && WHITE_LIST.Contains(
                        file.Extension
                    )
                );

            foreach (var file in filesToRename)
            {
                var content = string.Empty;

                using (var reader = new StreamReader(file.FullName))
                    content = reader.ReadToEnd();

                if (content.Contains(MARK))
                {
                    content = content.Replace(MARK, solutionName);
                    using (var writer = new StreamWriter(file.FullName))
                        writer.Write(content);
                }

                if (file.Name.Contains(MARK))
                {
                    var newFile = Path.Combine(
                        file.Directory.FullName,
                        file.Name.Replace(MARK, solutionName)
                    );

                    file.MoveTo(newFile);
                }
            }

            foreach (var folder in foldersToRename)
            {
                var info = new DirectoryInfo(folder);
                if (info.Name.Contains(MARK))
                {
                    info.MoveTo(info.FullName.Replace(MARK, solutionName));
                }
            }
        }
    }
}
