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
                    using (FileStream fs = File.Create(resultFilePath))
                    {
                        Console.WriteLine("Файл был создан на вашем рабочем столе");
                    }
                }

                Task<string[]> originalFile = Task.Run(() => File.ReadAllLinesAsync(originalFilePath));
                dictionary.Wait();
                originalFile.Wait();

                List<string> result = new List<string>();

                for (int i = 0; i < originalFile.Result.Length; i++)
                {
                    result.Add(GetSplitedWord(dictionary.Result, originalFile.Result[i]));
                }
                Task resultTask = Task.Run(() => File.WriteAllLinesAsync(resultFilePath, result));
                while (!resultTask.IsCompleted)
                {
                    Console.WriteLine("Обработка...");
                }
                Console.WriteLine("Обработка завершена");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public static string GetSplitedWord(string[] dict, string original)
        {
            List<string> lexemes = new List<string> { };
            original = original.ToLower();
            for (int i = 0; i < dict.Length; i++)
            {
                dict[i] = dict[i].ToLower();
                if (original.Contains(dict[i]))
                {
                    lexemes.Add(dict[i]);
                }
            }

            if (lexemes != null)
            {
                return SplitIntoLexeme(lexemes, original);
            }
            return original;
        }

        private static string SplitIntoLexeme(List<string> lexemes, string original)
        {
            List<string> startsWithLexemes = lexemes.Where(x => original.StartsWith(x) && x != original).OrderByDescending(x => x.Length).ToList();
            List<string> endsWithLexemes = lexemes.Where(x => !startsWithLexemes.Contains(x) && x != original).OrderByDescending(x => x.Length).ToList();

            foreach (var startsWithLexeme in startsWithLexemes)
            {
                var residueLexemes = endsWithLexemes.Where(x => x.Length <= original.Length - startsWithLexeme.Length).OrderByDescending(x => x.Length).ToList();
                foreach (var residue in residueLexemes)
                {
                    if (startsWithLexeme + residue == original)
                    {
                        return startsWithLexeme + " " + residue;
                    }
                    else if (original.StartsWith(startsWithLexeme + residue))
                    {
                        string result = SplitIntoLexeme(residueLexemes, original.Remove(0, (startsWithLexeme + residue).Length));
                        if (lexemes.Contains(result))
                            return startsWithLexeme + " " + residue + " " + result;
                    }
                }
            }
            return original;
        }
    }
}