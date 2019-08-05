using System;
using System.IO;
using System.Text.RegularExpressions;

namespace C_ {

    class Program {

        static void Main(string[] args) {

            Console.Clear();

            Console.WriteLine("\nSubtitle Renamer 1.0\n--------------------------------------------------------------------\n");

            Console.WriteLine("Directory:");
            String dir = Console.ReadLine();

            Console.WriteLine("Video Extension:");
            String vidExtension = Console.ReadLine();

            Console.WriteLine("Subtitle Extension:");
            String subExtension = Console.ReadLine();

            Console.WriteLine("Subtitle Language: (en for English etc... leave blank for no language code)");
            String subLang = Console.ReadLine();

            Console.WriteLine("Video Episode Number Index (0 based):");
            String vi = Console.ReadLine();

            Console.WriteLine("Subtitle Episode Number Index (0 based):");
            String si = Console.ReadLine();

            int subIndex = Convert.ToInt32(si);
            int vidIndex = Convert.ToInt32(vi);

            // Check var formats

            //24 23

            if (vidExtension.Trim().Length == 0 || subExtension.Trim().Length == 0 || dir.Trim().Length == 0) {
                Console.WriteLine("[ERROR] Please enter valid parameters");
                Environment.Exit(0);
            }

            if (vidExtension.Substring(0,1) != ".") {
                vidExtension = "." + vidExtension;
            }

            if (subExtension.Substring(0,1) != ".") {
                subExtension = "." + subExtension;
            }

            if (dir.Substring(dir.Length - 1) != "\\") {
                dir = dir + "\\";
            }

            if (subLang.Length > 0) {
                subLang = "." + subLang;
            }

            // Check directory

            if (!Directory.Exists(dir)) {
                Console.WriteLine("[ERROR] Directory does not exist");
                Environment.Exit(0);
            }

            if (Directory.GetFiles(dir, "*" + vidExtension).Length == 0 || Directory.GetFiles(dir, "*" + subExtension).Length == 0) {
                Console.WriteLine("[ERROR] Directory does not contain any files with the subtitle/video extension");
                Environment.Exit(0);
            }

            // -----------------

            renameSubs(dir, vidExtension, subExtension, vidIndex, subIndex, subLang);

        }

        public static void renameSubs(String dir, String ve, String se, int vi, int si, String lang) {

            Console.WriteLine("\n\nRenaming subtitles with the format " + se + " for videos with the format " + ve + " in \"" + dir + "\"\n\n");

            String[] subtitles = Directory.GetFiles(dir, "*" + se);
            String[] videos = Directory.GetFiles(dir, "*" + ve);
            int dirLen = dir.Length;

            foreach (String vid in videos) {
                Console.WriteLine("[VID] " + vid);

                int ep = Convert.ToInt32(vid.Substring(dirLen + vi, 2));

                foreach (String sub in subtitles) {
                    int subEp = Convert.ToInt32(sub.Substring(dirLen + si, 2));

                    if (subEp == ep) {
                        String name = Path.GetFileNameWithoutExtension(vid);

                        try {
                            File.Move(sub, dir + name + lang + se);
                            Console.WriteLine("[SUB] " + dir + name + se);
                        } catch (IOException e) {
                            Console.WriteLine("[ERROR] " + e.ToString());
                        }

                        break;
                    }
                }
            }

            Console.WriteLine("\n\nFinished");

        }

    }

}
