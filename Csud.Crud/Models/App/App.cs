﻿namespace Csud.Crud.Models.App
{
    public class App: AppBase
    {
        public string Name { get; set; }

        public int LastDistribKey { get; set; }

        public virtual string DisplayName { get; set; }
    }
}
