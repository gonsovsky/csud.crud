using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csud.Crud.Models;
using Csud.Crud.Models.Maintenance;

namespace Csud.Crud
{
    public partial interface ICsud
    {
        public void ImportApp<T>(T entity) where T: AppImport
        {
            entity.Step = Const.Import.Uploaded;
            entity.Submitted = DateTime.Now;
            AddEntity(entity);
        }
    }
}
