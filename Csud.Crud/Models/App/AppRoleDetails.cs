﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csud.Crud.Models.App
{
    public class AppRoleDetails: AppBase
    {
        public int RoleKey { get; set; }

        public int OperationKey { get; set; }

        public override int UseKey
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}
