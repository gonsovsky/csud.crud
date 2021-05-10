using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csud.Crud.Models
{
    public class RelationalAggregated<TGroup,TEntity> where TGroup : Base, IRelational where TEntity : Base
    {
        public TGroup Group { get; set; }
        public IEnumerable<TGroup> Index { get; set; } = new List<TGroup>();

        public TEntity Subject { get; set; }
        public IEnumerable<TEntity> Relations { get; set; } = new List<TEntity>();
    }
}
