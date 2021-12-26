using System;
using System.Reflection;
using System.Resources;
using System.Xml.Linq;

namespace CompoundWords
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                Task<string[]> dictionary = Task.Run(() => File.ReadAllLinesAsync(@".\Resources\de-dictionary.tsv"));

                Console.WriteLine("Укажите путь файла для обработки. Если оставите поле пустым, я буду использовать файл установленный по умолчанию");
                string originalFilePath = Console.ReadLine();
                Console.WriteLine("Укажите путь файла для записи результата. Если оставите поле пустым, я создам результирующий файл на вашем рабочем столе");
                string resultFilePath = Console.ReadLine();
                
                if (string.IsNullOrEmpty(originalFilePath.Trim()))
                {
                    originalFilePath = @".\Resources\de-test-words.tsv";
                }
                if (string.IsNullOrEmpty(resultFilePath.Trim()))
                {
                    resultFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH'-'mm'-'ss") + ".tsv";
                    if (!File.Exists(resultFilePath))
                    {
                        FileStream fs = File.Create(resultFilePath);
                    }
                }

                Task<string[]> originalFile = Task.Run(() => File.ReadAllLinesAsync(originalFilePath));
                dictionary.Wait();
                originalFile.Wait();
                Console.WriteLine(originalFile.IsCompleted);
                for (int i = 0; i < originalFile.Result.Length; i++)
                {
                    Console.WriteLine(SplitWord(dictionary.Result,originalFile.Result[i]));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public static string SplitWord(string[] dict, string original)
        {
            List<string> Lexemes = new List<string> { };
            original = original.ToLower();
            for (int i = 0; i < dict.Length; i++)
            {
                dict[i] = dict[i].ToLower();
                if (original.Contains(dict[i]))
                {
                    Lexemes.Add(dict[i]);
                }
            }

            if (Lexemes != null)
            {
                List<string> list = new List<string>();
                foreach (string lexeme in Lexemes)
                {
                    var pair1 = Lexemes?.Where(x => x + lexeme == original).FirstOrDefault();
                    if(pair1 != null)
                    {
                        list.Add(pair1 + " " + lexeme);
                    }
                }
                if(list?.Count > 0)
                {
                    return list.FirstOrDefault();
                }
            }
            return original;
        }

    }
}