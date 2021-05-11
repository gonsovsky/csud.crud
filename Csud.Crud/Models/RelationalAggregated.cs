using System.Collections.Generic;

namespace Csud.Crud.Models
{
    public class RelationalAggregated<TGroup,TLinked> where TGroup : Base, IRelational where TLinked : Base
    {
        public TGroup Group { get; set; }
        public IEnumerable<TGroup> Index { get; set; } = new List<TGroup>();

        public TLinked Subject { get; set; }
        public IEnumerable<TLinked> Relations { get; set; } = new List<TLinked>();
    }
}
