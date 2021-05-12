using System;

namespace Csud.Crud.DbTool.PromtEx.ConsoleEx
{
    public class Option
    {
        public Page Page { get; private set; }
        public string Name { get; private set; }
        public Action Callback { get; private set; }

        public Option(string name, Page page, Action callback)
        {
            Name = name;
            Page = page;
            Callback = callback;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
