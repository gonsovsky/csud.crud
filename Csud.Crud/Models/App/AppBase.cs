using System;
using Csud.Crud.Models.Internal;

namespace Csud.Crud.Models.App
{
    public abstract class AppBase : Base
    {
        public string XmlGuid { get; set; } = Guid.Empty.ToString();

    }
}
