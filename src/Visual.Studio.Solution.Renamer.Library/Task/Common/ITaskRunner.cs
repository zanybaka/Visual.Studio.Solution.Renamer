using System;

namespace Visual.Studio.Solution.Renamer.Library.Task.Common
{
    public interface ITaskRunner
    {
        ITaskRunner OnUpdate(Action action);
        bool Run(bool preview);
        bool Preview();
        bool Apply();
    }
}