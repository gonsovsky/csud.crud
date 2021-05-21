using System.Linq;
using Csud.Crud.Models;

namespace Csud.Crud.Services
{
    public interface IOneToAnyService<TEntity, TLinked> where TEntity : Base where TLinked : Base
    {
        public TEntity Look(int key);
        public TEntity Look(TEntity entity);
    }

    public abstract class OneToAnyService<TEntity, TLinked>: IOneToAnyService<TEntity, TLinked> where TEntity : Base  where TLinked : Base
    {

        protected readonly IEntityService<TEntity> EntitySvc;

        protected readonly IEntityService<TLinked> LinkedSvc;

        protected OneToAnyService(IEntityService<TEntity> entitySvc, IEntityService<TLinked> linkedSvc)
        {
            EntitySvc = entitySvc;
            LinkedSvc = linkedSvc;
        }

        public TEntity Look(int key)
        {
            return EntitySvc.Select().First(a => a.Key == key);
        }

        public virtual TEntity Look(TEntity entity)
        {
            return EntitySvc.Select().First(a => a.Key == entity.Key);
        }
    }
}
