using ConsoleApplication1;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Muslivets_LeoNeed_laba2_OS
{
  class Producer
  {
    private ChannelWriter<string> Writer;
    private ChannelWriter<string> writer;

    public Producer(ChannelWriter<string> _writer)
    {
      Writer = _writer;
      Task.WaitAll(Run());
    }

    private async Task Run()
    {
      //ожидает, когда освободиться место для записи элемента.
      while (await Writer.WaitToWriteAsync())
      {
        char[] word = new char[5];
        for (int i = 97; i < 123; i++)
        {
          word[0] = (char)i;
          for (int k = 97; k < 123; k++)
          {
            word[1] = (char)k;
            for (int l = 97; l < 123; l++)
            {
              word[2] = (char)l;
              for (int m = 97; m < 123; m++)
              {
                word[3] = (char)m;
                for (int n = 97; n < 123; n++)
                {
                  word[4] = (char)n;
                  if (!Program.foundFlag)
                  {
                    await Writer.WriteAsync(new string(word));
                  }
                  else
                  {
                    Writer.Complete();
                    return;
                  }
                }
              }
            }
          }
        }
      }
    }
  }
}
