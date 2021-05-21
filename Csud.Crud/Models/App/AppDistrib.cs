namespace Csud.Crud.Models.App
{
    public class AppDistrib: AppBase
    {
        public int DistribKey { get; set; }

        public int AppKey { get; set; }

        public int LoadDate { get; set; }

        public int Version { get; set; }

        public override int UseKey
        {
            get => DistribKey;
            set => DistribKey = value;
        }
    }
}
