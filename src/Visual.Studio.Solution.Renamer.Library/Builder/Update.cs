using System;
using System.Collections.Generic;
using System.Linq;
using Visual.Studio.Solution.Renamer.Library.Entity.Folder;
using Visual.Studio.Solution.Renamer.Library.Entity.Project;
using Visual.Studio.Solution.Renamer.Library.Task;
using Visual.Studio.Solution.Renamer.Library.Task.Common;

namespace Visual.Studio.Solution.Renamer.Library.Builder
{
    public static class Update
    {
        public static UpdateFolder Folder => new UpdateFolder();
        public static UpdateProject Project => new UpdateProject();
    }

    public class UpdateProject
    {
        public UpdateProjectDefaultAssemblyNameTaskCreator DefaultAssemblyName => new UpdateProjectDefaultAssemblyNameTaskCreator();
        public UpdateProjectDefaultRootNamespaceTaskCreator DefaultRootNamespace => new UpdateProjectDefaultRootNamespaceTaskCreator();
        public UpdateProjectNamesTaskCreator Name => new UpdateProjectNamesTaskCreator();
    }

    public class UpdateProjectDefaultRootNamespaceTaskCreator : ProjectTaskCreator
    {
        internal override string Description => "Update project default RootNamespace tags";

        protected override IProjectTask<ProjectInSolution> CreateTaskProjectInSolution()
        {
            return new SyncProjectDefaultRootNamespaceTask();
        }

        protected override IProjectTask<ProjectInFileSystem> CreateTaskProjectInFileSystem()
        {
            return new SyncProjectDefaultRootNamespaceTask();
        }
    }

    public class UpdateProjectDefaultAssemblyNameTaskCreator : ProjectTaskCreator
    {
        internal override string Description => "Update project default AssemblyName tags";

        protected override IProjectTask<ProjectInSolution> CreateTaskProjectInSolution()
        {
            return new SyncProjectDefaultAssemblyNameTask();
        }

        protected override IProjectTask<ProjectInFileSystem> CreateTaskProjectInFileSystem()
        {
            return new SyncProjectDefaultAssemblyNameTask();
        }
    }

    public class UpdateFolder
    {
        private readonly List<string> _masks;

        public UpdateFolder()
        {
            _masks = new List<string>();
        }

        private UpdateFolder(IEnumerable<string> masks) : this()
        {
            _masks.AddRange(masks);
        }

        public UpdateFolderNamesTaskCreator Names => new UpdateFolderNamesTaskCreator();

        public RemoveFolderTaskCreator Remove => new RemoveFolderTaskCreator(_masks);

        public RenameFolderOrFileTaskCreator ReplaceText => new RenameFolderOrFileTaskCreator();

        public ReplaceContentInFileTaskCreator ReplaceContent => new ReplaceContentInFileTaskCreator();

        public UpdateFolder FilteredByName(params string[] masks)
        {
            return new UpdateFolder(_masks.Union(masks));
        }
    }

    public class UpdateProjectNamesTaskCreator : ProjectTaskCreator
    {
        internal override string Description => "Rename project names";

        protected override IProjectTask<ProjectInSolution> CreateTaskProjectInSolution()
        {
            return new SyncProjectNameTask();
        }

        protected override IProjectTask<ProjectInFileSystem> CreateTaskProjectInFileSystem()
        {
            throw new NotSupportedException("Please specify a solution file");
        }
    }

    public class UpdateFolderNamesTaskCreator : ProjectTaskCreator
    {
        internal override string Description => "Sync folder names with project names";

        protected override IProjectTask<ProjectInSolution> CreateTaskProjectInSolution()
        {
            return new SyncFolderNameAndProjectNameTask();
        }

        protected override IProjectTask<ProjectInFileSystem> CreateTaskProjectInFileSystem()
        {
            return new SyncFolderNameTask();
        }
    }

    public class RemoveFolderTaskCreator : FolderOrFileTaskCreator<RemoveFolderTaskCreator>
    {
        public RemoveFolderTaskCreator(IEnumerable<string> masks)
        {
            Masks = masks.ToArray();
        }

        internal string[] Masks { get; }

        public override AtPath<RemoveFolderTaskCreator> At => new AtPath<RemoveFolderTaskCreator>(this);

        internal override string Description => $"Remove folder(s): {string.Join(", ", Masks)}";

        public TargetFolder<RemoveFolderTaskCreator> Folder =>
            new TargetFolder<RemoveFolderTaskCreator>(
                UpdateFolderOptions.Create(Path).WithMasks(Masks),
                this);

        internal override IFolderOrFileTask Create()
        {
            return new DeleteFolderTask();
        }
    }

    public class ReplaceContentInFileTaskCreator : FolderOrFileTaskCreator<ReplaceContentInFileTaskCreator>
    {
        public override AtPath<ReplaceContentInFileTaskCreator> At => new AtPath<ReplaceContentInFileTaskCreator>(this);

        internal override string Description => "Replace file contents";

        internal override IFolderOrFileTask Create()
        {
            return new ReplaceContentInFileTask();
        }
    }

    public class WithTargetFolderOrFile<TCreator>
        where TCreator : TaskCreatorBase
    {
        public WithTargetFolderOrFile(FolderOrFileTaskCreator<TCreator> taskCreator)
        {
            if (string.IsNullOrEmpty(taskCreator.Path))
            {
                throw new ArgumentException("Path is not set.");
            }

            File = new TargetFile<TCreator>(
                UpdateFileOptions.Create(taskCreator.Path),
                taskCreator);

            Folder = new TargetFolder<TCreator>(
                UpdateFolderOptions.Create(taskCreator.Path),
                taskCreator);
        }

        public TargetFile<TCreator> File { get; }

        public TargetFolder<TCreator> Folder { get; }
    }

    public class RenameFolderOrFileTaskCreator : FolderOrFileTaskCreator<RenameFolderOrFileTaskCreator>
    {
        public override AtPath<RenameFolderOrFileTaskCreator> At => new AtPath<RenameFolderOrFileTaskCreator>(this);

        internal override string Description => "Rename folders/files";

        internal override IFolderOrFileTask Create()
        {
            return new RenameFolderOrFileTask();
        }
    }

    public class WithTargetProject
    {
        public WithTargetProject(ProjectTaskCreator taskCreator)
        {
            if (string.IsNullOrEmpty(taskCreator.Path))
            {
                throw new ArgumentException("Path is not set.");
            }

            Project = new TargetProject(UpdateProjectOptions.AllProjects(taskCreator.Path), taskCreator);
        }

        public TargetProject Project { get; }
    }

    public class InTargetSolution : TargetBase<ProjectTaskCreator, UpdateProjectOptions>
    {
        public InTargetSolution(ProjectTaskCreator taskCreator) : this(UpdateProjectOptions.Empty(), taskCreator)
        {
        }

        private InTargetSolution(UpdateProjectOptions options, ProjectTaskCreator taskCreator) : base(options, taskCreator)
        {
        }

        public ITaskRunner Names => CreateTaskRunner();

        protected override ITaskRunner CreateTaskRunner()
        {
            return new ProjectTaskRunner(new TargetProject(Options, TaskCreator), TaskCreator);
        }

        public InTargetSolution Solution(string slnFile)
        {
            return new InTargetSolution(Options.Clone().WithSolution(TaskCreator.Path, slnFile), TaskCreator);
        }

        public InTargetSolution Replacing(string from, string to)
        {
            if (string.IsNullOrEmpty(from))
            {
                throw new ArgumentException("Field 'from' is not set");
            }

            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentException("Field 'to' is not set");
            }

            return new InTargetSolution(Options.Clone().WithReplace(from, to), TaskCreator);
        }
    }

    public class TargetProject : TargetBase<ProjectTaskCreator, UpdateProjectOptions>
    {
        public TargetProject(UpdateProjectOptions options, ProjectTaskCreator taskCreator) : base(options, taskCreator)
        {
        }

        public ITaskRunner Names => CreateTaskRunner();

        protected override ITaskRunner CreateTaskRunner()
        {
            return new ProjectTaskRunner(this, TaskCreator);
        }

        public TargetProject All()
        {
            return new TargetProject(UpdateProjectOptions.AllProjects(Options.WorkingDirectory), TaskCreator);
        }

        public TargetProject DeclaredInSolution(string slnFile)
        {
            return new TargetProject(UpdateProjectOptions.ProjectsFromTheSolution(Options.WorkingDirectory, slnFile), TaskCreator);
        }

        public TargetProject FilteredByMask(string mask)
        {
            return FilteredByMask(new[] { mask });
        }

        public TargetProject FilteredByMask(string[] masks)
        {
            return new TargetProject(UpdateProjectOptions.ProjectsWithMask(Options.WorkingDirectory, masks), TaskCreator);
        }
    }

    public class TargetFolder<TCreator> : TargetBase<FolderOrFileTaskCreator<TCreator>, UpdateFolderOptions>
        where TCreator : TaskCreatorBase
    {
        public TargetFolder(UpdateFolderOptions options, FolderOrFileTaskCreator<TCreator> taskCreator)
            : base(options, taskCreator)
        {
        }

        public ITaskRunner Itself => CreateTaskRunner();

        protected override ITaskRunner CreateTaskRunner()
        {
            return new FolderTaskRunner<TCreator>(this, TaskCreator);
        }

        public TargetFolder<TCreator> Recursively()
        {
            return new TargetFolder<TCreator>(Options.Clone().WithSubdirectories(), TaskCreator);
        }

        public TargetFolder<TCreator> FilteredByName(string value)
        {
            return new TargetFolder<TCreator>(Options.Clone().WithMasks(new[] { value }), TaskCreator);
        }

        public TargetFolder<TCreator> From(string value)
        {
            return new TargetFolder<TCreator>(Options.Clone().WithFrom(value), TaskCreator);
        }

        public TargetFolder<TCreator> To(string value)
        {
            return new TargetFolder<TCreator>(Options.Clone().WithTo(value), TaskCreator);
        }
    }

    public class TargetFile<TCreator> : TargetBase<FolderOrFileTaskCreator<TCreator>, UpdateFileOptions>
        where TCreator : TaskCreatorBase
    {
        public TargetFile(UpdateFileOptions options, FolderOrFileTaskCreator<TCreator> taskCreator)
            : base(options, taskCreator)
        {
        }

        public ITaskRunner Names => CreateTaskRunner();

        protected override ITaskRunner CreateTaskRunner()
        {
            return new FileTaskRunner<TCreator>(this, TaskCreator);
        }

        public TargetFile<TCreator> Recursively()
        {
            return new TargetFile<TCreator>(Options.Clone().WithSubdirectories(), TaskCreator);
        }

        public TargetFile<TCreator> FilteredByName(params string[] mask)
        {
            return new TargetFile<TCreator>(Options.Clone().WithMasks(mask), TaskCreator);
        }

        public TargetFile<TCreator> From(string value)
        {
            return new TargetFile<TCreator>(Options.Clone().WithFrom(value), TaskCreator);
        }

        public TargetFile<TCreator> To(string value)
        {
            return new TargetFile<TCreator>(Options.Clone().WithTo(value), TaskCreator);
        }
    }
}