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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public static string SomeAlgoritm(string[] dict, string original)
        {
            return string.Empty;
        }

    }
}