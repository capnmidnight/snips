using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace scan_for_file_references
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var folder = new FolderBrowserDialog();

            folder.SelectedPath = @"C:\Projects\XLerator\websites\Xlerator";
            if (folder.ShowDialog() == DialogResult.OK)
            {
                var dirs = new List<DirectoryInfo>();
                dirs.Add(new DirectoryInfo(folder.SelectedPath));
                var files = new List<FileInfo>();
                var badDirs = new[] { "_svn", "images", "bin", "app_themes", "app_code", "usercontrols", "javascript" };
                while (dirs.Count > 0)
                {
                    var curDir = dirs[0];
                    dirs.RemoveAt(0);
                    if (!badDirs.Contains(curDir.Name.ToLower()))
                    {
                        files.AddRange(curDir.GetFiles());
                        dirs.AddRange(curDir.GetDirectories());
                    }
                }
                var exts = new[] { ".asp", ".css", ".aspx", ".cs", ".js", ".txt" };
                var refs = new List<int>[files.Count];
                for (int i = 0; i < refs.Length; ++i) refs[i] = new List<int>();
                for (int x = 0; x < files.Count; ++x)
                    if (exts.Contains(files[x].Extension.ToLower()))
                        using (var reader = files[x].OpenText())
                        {
                            var text = reader.ReadToEnd().ToUpper();
                            for (int y = 0; y < files.Count; ++y)
                                if (x != y)
                                {
                                    var word = files[y].Name.ToUpper();
                                    var q = text.IndexOf(word);
                                    word += "X";
                                    if (q > -1 && (word.EndsWith("XX") || q != text.IndexOf(word)))
                                        refs[y].Add(x);
                                }
                            Application.DoEvents();
                        }
                var ignore = new[] { ".asax", ".aspx", ".cs", ".config", ".css", ".txt", ".sln", ".suo", ".asa", ".licx" };
                using (var writer = new StreamWriter("C:\\out.xml"))
                {
                    writer.WriteLine("digraph G{");
                    for (int x = 0; x < refs.Length; ++x)
                        print(files, refs, writer, x);
                    //if (refs[x].Count == 0)
                    //    writer.WriteLine("del " + files[x].FullName);

                    writer.WriteLine("}");
                    writer.Flush();
                    writer.Close();
                }
                System.Diagnostics.Process.Start("notepad", "C:\\out.xml");
            }
        }

        private static void print(List<FileInfo> files, List<int>[] refs, StreamWriter writer, int x)
        {
            if (refs[x].Count > 0)
            {
                for (int y = 0; y < refs[x].Count; ++y)
                    writer.WriteLine(string.Format(@" {0} -> {1}", files[x].Name, files[refs[x][y]].Name).Replace(".", "_"));
            }
            else
                writer.WriteLine(files[x].Name.Replace(".", "_") + " -> NONE");
        }
    }
}
