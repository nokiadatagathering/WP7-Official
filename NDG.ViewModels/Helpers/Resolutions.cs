namespace NDG.ViewModels.Helpers
{
    public class Resolution
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public string Text { get; set; }
        public Resolutions Name { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

    public enum Resolutions
    {
        Small,
        Medium,
        Large,
        Max
    }
}
