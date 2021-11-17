using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FileBackup
{
    class Program
    {
        public static void Pause(string s)
        {
            if (s == "") s = "Нажмите клавишу для продолжения";
            Console.WriteLine(s);
            Console.ReadKey();
        }

        static DateTime creationTime2;
        static void Main(string[] args)
        {

            string SourcePath = @"..\..\..\this";
            string DestinationPath = @"..\..\..\backup";
            bool exist;

            SourcePath = Path.GetFullPath(SourcePath);
            DestinationPath = Path.GetFullPath(DestinationPath);
            
            Console.WriteLine("Base source dir: " + SourcePath); 
            Console.WriteLine("Base backup dir: " + DestinationPath);
            Pause("");

            string ReplacePath;
            
            // создаем бэкап каталоги 
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                        SearchOption.AllDirectories))
            {
                ReplacePath = dirPath.Replace(SourcePath, DestinationPath);
                Directory.CreateDirectory(ReplacePath);
            }

            //Копируем все файлы из newPath в ReplacePath
            //при совпадении имен сравниваем даты модификации исходного файла и backup
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                SearchOption.AllDirectories))
            {
                DateTime creationTime1 = File.GetLastWriteTime(newPath);
                
                ReplacePath = newPath.Replace(SourcePath, DestinationPath);
             

                if ((!File.Exists(ReplacePath)))
                {
                    File.Copy(newPath, ReplacePath, true);
                    Console.WriteLine("File.Not.Exist.Copy: " + newPath + " >> " + ReplacePath);
                }
                else
                {
                    creationTime2 = File.GetCreationTime(ReplacePath);
                    if (DateTime.Compare(creationTime1, creationTime2) > 0)
                   {
                        File.Copy(newPath, ReplacePath, true);
                        Console.WriteLine("File.New.Copy: " + newPath + " >> " + ReplacePath);
                    } else { Console.WriteLine("Файл не требует копирования: " + ReplacePath);  }
                }         
            }

            // Запускаем таск шедуллер виндоус
            Process p = new Process();
            p.StartInfo.FileName = "c:\\WINDOWS\\system32\\taskschd.msc";
            p.StartInfo.Arguments = @"/s";
            p.Start();

            Console.Write("\nНастройка шедулера: ");
            Pause("https://www.windowscentral.com/how-create-automated-task-using-task-scheduler-windows-10");
        }
        
    }
}
