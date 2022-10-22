using System;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading.Channels;
using Muslivets_LeoNeed_laba2_OS;

namespace ConsoleApplication1
{
  class Program
  {
    const string PATH = "passwordHashes.txt";
    static public bool foundFlag = false;

    static void printMenu()
    {
      bool flag = true;
      while (flag)
      {
        Console.WriteLine("1. Выполнить задание.");
        Console.WriteLine("2. Очистить консоль.");
        Console.WriteLine("3. Выйти из программы.");
        Console.Write("Выберите пункт меню: ");
        int choice = int.Parse(Console.ReadLine());
        switch (choice)
        {
          case 1:
            Console.WriteLine("\tВыберите по какому хеш значению SHA-256 подобрать пароль: ");
            Console.WriteLine("\t1. 1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad");
            Console.WriteLine("\t2. 3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b");
            Console.WriteLine("\t3. 74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f");
            Console.Write("\t---> ");
            int sign = int.Parse(Console.ReadLine());
            string[] readText = File.ReadAllLines(PATH);
            string passwordHash = readText[sign - 1].ToUpper();
            Console.Write("\tВведите количество потоков: ");
            int countStream = int.Parse(Console.ReadLine());
            Console.WriteLine("\tОжидайте подбор пароля...");

            //создаю общий канал данных
            Channel<string> channel = Channel.CreateBounded<string>(countStream);
            Stopwatch time = new();
            time.Reset();
            time.Start();
            //создается производитель
            var prod = Task.Run(() => { new Producer(channel.Writer); });
            Task[] streams = new Task[countStream + 1];
            streams[0] = prod;
            //создаются потребители 
            for (int i = 1; i < countStream + 1; i++)
            {
              streams[i] = Task.Run(() => { new Consumer(channel.Reader, passwordHash); });
            }
            //Ожидает завершения выполнения всех указанных объектов Task 
            Task.WaitAny(streams);
            time.Stop();
            Console.WriteLine($"\tЗатраченное время на подбор: {time.Elapsed}");
            Console.WriteLine("\tВведите ENTER, чтобы выйти в главное меню.");
            Console.WriteLine();
            Console.ReadKey();
            foundFlag = false;
            break;
          case 2:
            Console.Clear();
            break;
          case 3:
            flag = false;
            break;
          default:
            Console.WriteLine("\tВыбранного пункта нет в меню.");
            break;
        }
      }
    }

    static public void Main()
    {
      printMenu();
    }
  }
}
