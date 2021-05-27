using System;
using System.Xml.Linq;

namespace Csud.Crud.DbTool.Log
{
    internal interface ILogService
    {
        public void Log(XElement xel, string msg);
    }
    internal class LogService: ILogService
    {
        public LogService()
        {
            if (System.IO.File.Exists("!.txt"))
                System.IO.File.Delete("!.txt");
        }

        public void Log(XElement xel, string msg)
        {
            var str = xel.BaseUri + ": " + msg;
            System.IO.File.AppendAllText("!.txt",str + "\r\n");
            Console.WriteLine(str);
        }
    }
}
