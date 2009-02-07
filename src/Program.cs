using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
namespace SecondLifeLogMerger {
    public class Program {
        static Regex regex = new Regex(@"^\[(.+?/.+?/.+?\s.+?:.+?)\]\s\s(You|\w+\s\w+)(:\s|\s)");
        static void Main(string[] args) {
            String output, input1, input2;
            if (args.Length > 1) {
                input1 = args[0];
                input2 = args[1];
                if (args.Length > 2)
                    output = args[2];
                else
                    output = args[1];
            } else {
                Console.WriteLine("usage:");
                Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Name + " source1 source2 [destination]");
                Console.WriteLine();
                Console.WriteLine("  source1\tChatlog location 1");
                Console.WriteLine("  source2\tChatlog location 2 (read destination argument)");
                Console.WriteLine("  destination\tDestination directory of the chatlogs, if not set it will overwrite the files in the path specified by the source2 argument.");
                return;
            }
            Console.WriteLine("source1:\t" + input1);
            Console.WriteLine("source2:\t" + input2);
            Console.WriteLine("destination:\t" + output);
            List<String> handledFiles = new List<String>();
            FileInfo[] inputFilelist1, inputFilelist2;
            DirectoryInfo inputdir1, inputdir2, outputdir;
            try {
                inputdir1 = new DirectoryInfo(input1);
                inputdir2 = new DirectoryInfo(input2);
                outputdir = new DirectoryInfo(output);
                inputFilelist1 = inputdir1.GetFiles("*.txt");
                inputFilelist2 = inputdir2.GetFiles("*.txt");
                //Test if output directory exists (throws exception if it doesn't)
                outputdir.GetFiles();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return;
            }
            int handled = 0, copied = 0, merged = 0;
            Console.WriteLine("files:");
            // if output equals input no works needs to be done
            if (inputdir1.FullName != outputdir.FullName)
            foreach(FileInfo file1 in inputFilelist1) {
                handled++;
                Console.Write(file1.Name + " ");
                String outputPath = outputdir.FullName + "\\" + file1.Name;
                FileInfo [] res = inputdir2.GetFiles(file1.Name);
                if(res.Length==0) {
                    File.Delete(outputPath);
                    file1.CopyTo(outputPath);
                    Console.WriteLine("copied to " + outputPath);
                    copied++;
                    continue;
                }
                FileInfo file2 = res[0];
                MergeFile(file1, file2, outputPath);
                merged++;
                Console.WriteLine("merged to " + outputPath);
                handledFiles.Add(file1.Name);
            }
            // if output equals input no works needs to be done
            if (inputdir2.FullName != outputdir.FullName)
            foreach (FileInfo file1 in inputFilelist2) {
                if(handledFiles.Contains(file1.Name))
                    continue;
                handled++;
                Console.Write(file1.Name + " ");
                String outputPath = outputdir.FullName + "\\" + file1.Name;
                FileInfo[] res = inputdir1.GetFiles(file1.Name);
                if (res.Length == 0) {
                    File.Delete(outputPath);
                    file1.CopyTo(outputPath);
                    Console.WriteLine("copied to " + outputPath);
                    copied++;
                    continue;
                }
                FileInfo file2 = res[0];
                MergeFile(file1, file2, outputPath);
                merged++;
                Console.WriteLine("merged to " + outputPath);
            }
            Console.WriteLine();
            Console.WriteLine("Handled "+handled+" files, copied " + copied + " and merged " + merged);
            Console.WriteLine("done..");
            Console.ReadKey();
        }
        public static void MergeFile(FileInfo file1, FileInfo file2, String outputPath) {
            StreamReader sr1 = new StreamReader(file1.OpenRead());
            StreamReader sr2 = new StreamReader(file2.OpenRead());
            String tempfile = "~temp.txt";
            StreamWriter sw = new StreamWriter(File.Open(tempfile, FileMode.Create));
            String line1 = sr1.ReadLine(), line2 = sr2.ReadLine();
            while(line1!=null||line2!=null)
            {
                if (line1 == null) {
                    sw.WriteLine(line2);
                    line2 = sr2.ReadLine();
                    continue;
                }
                if (line2 == null) {
                    sw.WriteLine(line1);
                    line1 = sr1.ReadLine();
                    continue;
                }
                if (line1 == line2) {
                    sw.WriteLine(line1);
                    line1 = sr1.ReadLine();
                    line2 = sr2.ReadLine();
                    continue;
                }
                if (!regex.IsMatch(line1)) {
                    sw.WriteLine(line1);
                    line1 = sr1.ReadLine();
                    continue;
                }
                if (!regex.IsMatch(line2)) {
                    sw.WriteLine(line2);
                    line2 = sr2.ReadLine();
                    continue;
                }
                DateTime dLine1 = DateTime.Parse(regex.Match(line1).Result("$1"));
                DateTime dLine2 = DateTime.Parse(regex.Match(line2).Result("$1"));
                if (dLine1 < dLine2) {
                    sw.WriteLine(line1);
                    line1 = sr1.ReadLine();
                } else {
                    sw.WriteLine(line2);
                    line2 = sr2.ReadLine();
                }
            }
            sw.Close();
            sr1.Close();
            sr2.Close();
            File.Delete(outputPath);
            File.Copy(tempfile,outputPath);
            File.Delete(tempfile);
        }
    }
}
