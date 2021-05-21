using System.IO;
using Csud.Crud.Models.Maintenance;

namespace Csud.Crud.Services
{
    public interface IMaintenanceService : IEntityService<AppImport>
    {
        public string GetPath(string filename);
    }

    public class MaintenanceService : EntityService<AppImport>, IMaintenanceService
    {
        protected Config Cfg;
        public MaintenanceService(Config cfg) :  base()
        {
            Cfg = cfg;
        }

        public string GetPath(string filename)
        {
            return Path.Combine(Cfg.Import.Folder, filename);
        }
    }
}
