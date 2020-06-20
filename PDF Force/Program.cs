using iTextSharp.text.exceptions;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDF_Force
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            List<string> dictionary = null;
            StopwatchController stopwatch = new StopwatchController(new System.Diagnostics.Stopwatch());
            DitionaryCreator ditionaryCreator = new DitionaryCreator();
            if (args.Any(a => a == "help"))
            {
                PrintManual();
                return;
            }
            if (args.Any(a => a == "-bd"))
            {
                PrintPDfForce();
                dictionary = GenerateBirthdayDictionary(stopwatch, ditionaryCreator);
            }
            if (args.Any(a => a == "-kw"))
            {
                PrintPDfForce();
                dictionary = GenerateKeywordsDictionary(stopwatch, ditionaryCreator);
            }
            if (args.Any(a => a == "-df"))
            {
                PrintPDfForce();
                dictionary = LoadDictionary();
            }
            FindPassword(args, dictionary, stopwatch);
        }

        private static void FindPassword(string[] args, List<string> dictionary, StopwatchController stopwatch)
        {
            if (dictionary is null || dictionary.Count < 1)
            {
                return;
            }
            Console.WriteLine("Write path of PDF File:");
            string pdfPath = Console.ReadLine();
            if (!File.Exists(pdfPath))
            {
                Console.WriteLine("404! 404! abort!");
                return;
            }
            if (!IsPasswordProtected(pdfPath))
            {
                Console.WriteLine("Doesn't appear to be password protected...");
                return;
            }
            stopwatch.Start();
            if (args.Any(a => a == "-p"))
            {
                Parallel.ForEach(dictionary, (str, pls) =>
                {
                    if (IsPasswordValid(pdfPath, GetBytes(str)))
                    {
                        Console.WriteLine("Correct password is: {0}", str);
                        stopwatch.Stop();
                        Console.WriteLine($"Working time: {stopwatch.Time}");
                        pls.Break();
                    }
                });
                return;
            }
            else
            {
                foreach (var password in dictionary)
                {
                    if (IsPasswordValid(pdfPath, GetBytes(password)))
                    {
                        Console.WriteLine("Correct password is: {0}", password);
                        stopwatch.Stop();
                        Console.WriteLine($"Working time: {stopwatch.Time}");
                        return;
                    }
                }
            }
            stopwatch.Stop();
            Console.WriteLine($"Password not find. Working time: {stopwatch.Time}");
        }

        public static bool IsPasswordProtected(string pdfFullname)
        {
            try
            {
                var pdfReader = new PdfReader(pdfFullname);
                return false;
            }
            catch (BadPasswordException)
            {
                return true;
            }
        }
        public static bool IsPasswordValid(string pdfFullname, byte[] password)
        {
            try
            {
                var pdfReader = new PdfReader(pdfFullname, password);
                return true;
            }
            catch (BadPasswordException)
            {
                return false;
            }
        }
        private static List<string> LoadDictionary()
        {
            List<string> dictionary = new List<string>();
            Console.WriteLine("Write path of dictionary:");
            string path = Console.ReadLine(); 
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    dictionary.AddRange(reader.ReadToEnd().Split(Environment.NewLine.ToCharArray()));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
            dictionary.RemoveAll(p => p == "");
            return dictionary;
        }
        private static List<string> GenerateKeywordsDictionary(StopwatchController stopwatch, DitionaryCreator ditionaryCreator)
        {
            List<string> dictionary = new List<string>();
            string str = "";
            List<string> keywords = new List<string>();
            while (true)
            {
                Console.WriteLine("Write keyword or stop for exit:");
                str = Console.ReadLine();
                if (str == "stop")
                {
                    break;
                }
                if (!string.IsNullOrEmpty(str))
                {
                    keywords.Add(str);
                }
            }
            stopwatch.Start();
            try
            {
                keywords = ditionaryCreator.FirstCharUp(keywords);
                dictionary = ditionaryCreator.GenerateForKeywords(keywords, 2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                stopwatch.Stop();
                Console.WriteLine($"Working time: {stopwatch.Time}\tKeywords: {keywords?.Count}\tDictionary length: {dictionary?.Count}");
            }
            return dictionary;
        }
        private static List<string> GenerateBirthdayDictionary(StopwatchController stopwatch, DitionaryCreator ditionaryCreator)
        {
            List<string> dictionary;
            DateTime startDate = new DateTime(1960, 1, 1);
            DateTime finishDate = DateTime.Now.AddYears(-10);
            Console.WriteLine($"Generate dictionary between {startDate.ToString("dd/MM/yyyy")} and {finishDate.ToString("dd/MM/yyyy")}");
            stopwatch.Start();
            dictionary = ditionaryCreator.GenerateForBirthday(startDate, finishDate);
            stopwatch.Stop();
            Console.WriteLine($"Working time: {stopwatch.Time}\tDictionary length: {dictionary.Count}");
            return dictionary;
        }
        private static void PrintPDfForce()
        {
            Console.Clear();
            Console.WriteLine("\t               ████████████████████████");
            Console.WriteLine("\t████─████──███ █───█────█────█────█───█");
            Console.WriteLine("\t█──█─█──██─█   █─███─██─█─██─█─██─█─███");
            Console.WriteLine("\t████─█──██─███ █───█─██─█────█─████───█");
            Console.WriteLine("\t█────█──██─█   █─███─██─█─█─██─██─█─███");
            Console.WriteLine("\t█────████──█   █─███────█─█─██────█───█");
            Console.WriteLine("\t               ████████████████████████\n");
        }
        private static void PrintManual()
        {
            PrintPDfForce();
            Console.WriteLine("Description: This is a console app designed to testing PDF password for security.\n");
            Console.WriteLine("List of keywords:");
            Console.WriteLine("\thelp -- print user's manual;");
            Console.WriteLine();
            Console.WriteLine("List of flags:");
            Console.WriteLine("\t-p -- parallel attack;");
            Console.WriteLine("\t-bd -- use birthdays dictionary (ddmmyyyy);");
            Console.WriteLine("\t-kw -- use keywords dictionary;");
            Console.WriteLine("\t-df -- use passwords dictionary from path.");
        }
        private static byte[] GetBytes(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }
    }
}
