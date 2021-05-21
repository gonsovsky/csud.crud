namespace Csud.Crud.Models.App
{
    public class App: AppBase
    {
        public int AppKey { get; set; }

        public string Name { get; set; }

        public int LastDistribKey { get; set; }

        public override int UseKey
        {
            get => AppKey;
            set => AppKey = value;
        }

    }
}
