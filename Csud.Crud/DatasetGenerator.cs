using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.IO;

namespace Csud.Crud
{
    public class DatasetGenerator
    {
        private ICsud _csud;

        public DatasetGenerator(ICsud csud)
        {
            _csud = csud;
        }

        public void Generate()
        {
            using (StreamReader sr = new StreamReader(DataFile))
            {
                string currentLine;
                while ((currentLine = sr.ReadLine()) != null)
                {
                    if (currentLine.IndexOf("I/RPTGEN", StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        Console.WriteLine(currentLine);
                    }
                }
            }

        }
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string DataFile
        {
            get
            {
                var file = Path.Join(AssemblyDirectory, "../../../../data/scsm.csv");
                return file;
            }
        }

    }
}
