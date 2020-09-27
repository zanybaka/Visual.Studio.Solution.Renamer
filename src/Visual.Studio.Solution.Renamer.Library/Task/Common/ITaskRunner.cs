namespace Visual.Studio.Solution.Renamer.Library.Task.Common
{
    public interface ITaskRunner
    {
        bool Run(bool preview);
        bool Preview();
        bool Apply();
    }
}