using System.Collections.Generic;
using System.Text;
using System;
using static System.Console;
using System.IO;
using System.Windows;
using HtmlAgilityPack;

namespace RomScraper_DesktopApp
{
    static class DirectoryFetcher
    {
        //Static variable to get the path of the current directory of the running app
        public static string currentDirectory = Directory.GetCurrentDirectory();

        //Function to check the existance of ROMs directory. In case of directory not found, this one would be created
        public static void CheckRomsDirectory()
        {
            if (!Directory.Exists($@"{currentDirectory}/roms"))
            {
                try
                {
                    WriteLine($"Creating root directory (roms): {currentDirectory}/roms");
                    Directory.CreateDirectory($@"{currentDirectory}/roms");
                }
                catch
                {
                    WriteLine("Folder creation failed.");
                }
            }
        }

        public static bool CheckRedundantRom(HtmlNode node, string platformDirectory)
        {
            string[] files = Directory.GetFiles(platformDirectory);
            var filesNames = new List<string>();

            foreach (string file in files)
            {
                filesNames.Add(Path.GetFileName(file));
            }

            if (filesNames.Contains(node.InnerText + ".zip") || filesNames.Contains(node.InnerText + ".bin"))
            {
                return true;
            }
            return false;
        }

        //Function to check the existance of the specific platoform we are downloading. In case of directory not found, this one would be created
        public static string CheckPlatformDirectory(string dir)
        {
            string path = currentDirectory + "/roms/" + dir;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory($@"{path}");
                    return $"Creating directory: {path}";
                }
                catch
                {
                    return "Folder creation failed.";
                }
            }
            return null;
        }

        //Function to show the current library with all the platforms and number of games
        public static void LibraryFecther(LibraryViewModel lb)
        {
            var directories = Directory.GetDirectories($@"{currentDirectory}/roms");

            if (Directory.Exists($@"{currentDirectory}/roms") && directories.Length > 0)
            {
                foreach (string directory in directories)
                {
                    string folderName = new DirectoryInfo(directory).Name;
                    var files = Directory.GetFiles(directory);
                    lb.LibraryRows.Add(new PlatformLibrary(folderName, files.Length));
                }
            }
        }
    }
}