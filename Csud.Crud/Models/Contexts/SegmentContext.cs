namespace Csud.Crud.Models.Contexts
{
    public class SegmentContext : BaseContext
    {
        public string SegmentName { get; set; }

        public override string ContextType => Const.Context.Segment;
    }
}
