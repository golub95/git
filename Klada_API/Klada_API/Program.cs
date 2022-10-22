using System.IO;
using Klada_API.Kladionice.Admiral;
using Klada_API.Kladionice.Favbet;

namespace Klada_API
{
    class Program
    {
        static void Main(string[] args)
        {
            Admiral_Nogomet.API_Admiral();
            //Favbet.API_Favbet();
        }

        public static void DownloadData(string resultText, string path)
        {
            File.WriteAllText(path, resultText);
        }
    }
}
