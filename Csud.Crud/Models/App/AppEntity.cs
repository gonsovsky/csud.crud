using System;

namespace Csud.Crud.Models.App
{
    public class AppEntity: AppBase
    {
        public int DistribKey { get; set; }

        public string EntityName { get; set; }

        public int EntityKey { get; set; }

        public override int UseKey
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}
