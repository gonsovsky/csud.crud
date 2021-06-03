using System;
using Csud.Crud.Models.Internal;

namespace Csud.Crud.Models.Maintenance
{
    public class AppImport: Base
    {
        protected override string QueueName => "AppImport";
        public DateTime Submitted { get; set; }

        public DateTime Accomplished { get; set; }

        public  string Step { get; set; } = Const.Import.Undefined;

        public string Details { get; set; }

        public string Applicant { get; set; }

        public string Email { get; set; }

        public string FileName { get; set; }

        public string Comment { get; set; }

    }
}
