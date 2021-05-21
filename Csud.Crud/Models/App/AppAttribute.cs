using System;

namespace Csud.Crud.Models.App
{
    public class AppAttribute: AppBase
    {
        public int DistribKey { get; set; }

        public string AttributeType { get; set; }

        public int AttributeKey { get; set; }

        public override int UseKey
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}
