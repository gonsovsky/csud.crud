namespace Csud.Crud.Models.Internal
{
    public interface IEntityKey
    {
       void CopyTo(Base entity);
       IEntityKey CopyFrom(Base entity);
    }

    public class EntityKey: IEntityKey
    {
        public virtual int Key { get; set; }

        public virtual void CopyTo(Base entity)
        {
            entity.Key = Key;
        }

        public virtual IEntityKey CopyFrom(Base entity)
        {
            return new EntityKey() {Key = entity.Key};
        }
    }
}
