using System;

namespace Csud.Crud.Models.App
{
    public class AppOperation: AppBase
    {
        public int DistribKey { get; set; }

        public string OperationName { get; set; }

        public int OperationKey { get; set; }

        public override int UseKey
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}
