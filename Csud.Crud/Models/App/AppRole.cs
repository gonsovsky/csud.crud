using System;

namespace Csud.Crud.Models.App
{
    public class AppRole : AppBase
    {
        public int DistribKey { get; set; }

        public string RoleName { get; set; }

        public int RoleKey { get; set; }

        public string DisplayName { get; set; }

        public override int UseKey
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}
