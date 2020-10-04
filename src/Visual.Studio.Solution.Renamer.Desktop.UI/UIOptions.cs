using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Visual.Studio.Solution.Renamer.Library;

namespace Visual.Studio.Solution.Renamer.Desktop.UI
{
    public class UIOptions : DependencyObject, IRenameOptions
    {
        public static readonly DependencyProperty SolutionFileProperty = DependencyProperty.Register(
            "SolutionFile", typeof(string), typeof(UIOptions), new PropertyMetadata(default(string)));

        public ObservableCollection<MaskItem> MaskCollection { get; set; }
        public bool Preview { get; set; }

        public string ReplaceFrom { get; set; }

        public string ReplaceTo { get; set; }

        public bool Cleanup { get; set; }

        public bool UseCsProj { get; set; }

        public bool Verbose { get; set; }

        public string SolutionFile
        {
            get => (string) GetValue(SolutionFileProperty);
            set => SetValue(SolutionFileProperty, value);
        }

        public string[] Masks
        {
            get => MaskCollection.Select(x => x.Value).ToArray();
            set => MaskCollection = new ObservableCollection<MaskItem>(value.Select(x => new MaskItem(x)));
        }

        public string WorkingDirectory { get; set; }
    }
}