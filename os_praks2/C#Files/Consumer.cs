using ConsoleApplication1;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Muslivets_LeoNeed_laba2_OS
{
  class Consumer
  {
    private ChannelReader<string> Reader;
    private string PasswordHash;

    public Consumer(ChannelReader<string> _reader, string _passwordHash)
    {
      Reader = _reader;
      PasswordHash = _passwordHash;
      Task.WaitAll(Run());
    }

    private async Task Run()
    {
      // ожидает, когда освободиться место для чтения элемента.
      while (await Reader.WaitToReadAsync())
      {
        if (!Program.foundFlag)
        {
          var item = await Reader.ReadAsync();
          //Console.WriteLine($"получены данные {item}");
          if (FoundHash(item.ToString()) == PasswordHash)
          {
            Console.WriteLine($"\tПароль подобран - {item}");
            Program.foundFlag = true;
          }
        }
        else return;
      }
    }
    /// <summary>
    /// Находит хеш str
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    static public string FoundHash(string str)
    {
      SHA256 sha256Hash = SHA256.Create();
      //Из строки в байтовый массив
      byte[] sourceBytes = Encoding.ASCII.GetBytes(str);
      byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
      string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
      return hash;
    }

  }
}
