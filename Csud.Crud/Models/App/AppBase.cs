using System;

namespace Csud.Crud.Models.App
{
    public partial class AppBase : Base
    {
        public string XmlGuid { get; set; } = Guid.Empty.ToString();

    }
}
