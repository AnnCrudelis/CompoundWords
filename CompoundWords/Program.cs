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
            //TODO: метод разбивает всего на 2 составные части. Необходимо, чтобы он мог разбивать на большее количество частей, если это возможно
            //TODO: но учитывать условие из тз: "При реализации алгоритма, следует брать из словаря, максимально “длинное” слово, насколько это возможно."
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
                List<string> list = new List<string>();
                foreach (string lexeme in lexemes)
                {
                    var pair1 = lexemes?.Where(x => x + lexeme == original).FirstOrDefault();
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