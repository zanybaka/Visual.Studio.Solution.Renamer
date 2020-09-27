namespace Visual.Studio.Solution.Renamer.Library.Builder
{
    public class UpdateOptions
    {
        internal UpdateOptions()
        {
            Preview = true;
        }

        internal UpdateOptions(string directory) : this()
        {
            WorkingDirectory = directory;
        }

        public string WorkingDirectory { get; internal set; }

        public bool Preview { get; private set; }

        public UpdateOptions WithPreview(bool value)
        {
            Preview = value;
            return this;
        }
    }
}