using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ookii.Dialogs.Wpf;
using Microsoft.Win32;
using System.Windows.Input;
using System.IO;
using System.Diagnostics;

namespace CryptoTools.Utilities
{
    public class FileUtils
    {
       
        public static void OpenDirectoryInExplorer(String path, bool select)
        {
            Process.Start("explorer.exe", (select ? "/select," : "") + path);
        }
        /// <summary>
        /// Partner to Path.ChangeExtension. This function changes the base filename portion
        /// </summary>
        /// <param name="path"></param>
        /// <param name="appendToFilename"> </param>
        /// <returns></returns>
        public static String ChangeFilename(String path, string appendToFilename)
        {
            String directoryName = Path.GetDirectoryName(path); //e.g. "C:\Temp\foo.dat" ==> "C:\Temp"
            String filename = Path.GetFileNameWithoutExtension(path);
            String extension = Path.GetExtension(path); //e.g. "C:\Temp\foo.dat" ==> ".dat"

            //Reassemble as
            //    directoryName \ newFilename dotExtension
            return String.Format("{0}{1}{2}{3}{4}",
                  directoryName,
                  Path.DirectorySeparatorChar,
                  filename,
                  appendToFilename,
                  extension);
        }
        /// <summary>
        /// Creates a temp file name for a given filename
        /// Ex. certs.pfx => certs_tmp.pfx
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static String GetTempBrotherFileName(String filepath)
        {
            String filename = Path.GetFileNameWithoutExtension(filepath);
            String ext = Path.GetExtension(filepath);
            String parent = Path.GetDirectoryName(filepath);
            String tempBroFile = FindFreeFileName(parent,String.Format("{0}_tmp{1}",filename,ext));
            return tempBroFile;
        }

        /// <summary>
        /// Displays the folder browser dialog
        /// </summary>
        /// <param name="title"></param>
        /// <param name="filename"></param>
        public static void FolderBrowserUI(String title, out String filename)
        {
            filename = null;
            var dialog = new VistaFolderBrowserDialog
                                                  {Description = title, UseDescriptionForTitle = true};
            if (dialog.ShowDialog() == true)
            {
                filename = dialog.SelectedPath;
            }
        }

        public static void OpenFileUI(String title, out String filename, String filter)
        {
            filename = null;
            var ofd = new OpenFileDialog();
            ofd.Filter = filter;
            ofd.Title = title;
            if (ofd.ShowDialog() == true)
            {
                filename = ofd.FileName;
            }
        }

        public static void OpenFileUI(String title, out String filename)
        {
            OpenFileUI(title,out filename, "All Files|*.*");
        }

        public static void SaveFileUI(String title, out String filename,String filter)
        {
            filename = null;
            var sfd = new SaveFileDialog();
            sfd.Title = title;
            if (sfd.ShowDialog() == true)
            {
                filename = sfd.FileName;
            }
        }

        public static void SaveFileUI(String title, out String filename)
        {
            SaveFileUI(title, out filename, "All Files|*.*");
        }

        /// <summary>
        /// Finds a free direcrtory name in the given parent directory.
        /// </summary>
        /// <param name="parent">Parent directory absolute path</param>
        /// <param name="name">preferred directiry name</param>
        /// <returns>returns a unique directory name</returns>
        public static String FindFreeFileName(String parent, String name)
        {
            int i = 1;
            String path = Path.Combine(parent, name);
            String tmp = name;
            while (File.Exists(path))
            {
                tmp = String.Format("{0}({1})", name, i);
                path = Path.Combine(parent, tmp);
                i++;
            }
            return tmp;
        }
    }
}
