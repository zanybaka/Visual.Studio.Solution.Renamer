using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Serilog;
using Visual.Studio.Solution.Renamer.Library;

namespace Visual.Studio.Solution.Renamer.Desktop.UI
{
    public partial class MainWindow
    {
        private readonly UIOptions _options;
        private readonly StringBuilder _sb = new StringBuilder();

        public MainWindow()
        {
            InitializeComponent();

            _options = new UIOptions
            {
                WorkingDirectory = Environment.CurrentDirectory,
                Cleanup          = true,
                UseCsProj        = false,
                Masks            = new[] { "*.sln", "*.xml", "*.xaml", "*.cs", "*.csproj" },
                Preview          = true,
                Verbose          = true
            };
            DataContext = _options;
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnRenameClick(object sender, RoutedEventArgs e)
        {
            txtOutput.Clear();
            _sb.Clear();
            IsEnabled = false;
            var button  = (Button) sender;
            var oldName = button.Content;
            button.Content = GetProgressName(button.Content);
            try
            {
                var logLevel = new LoggerConfiguration().MinimumLevel;

                Log.Logger =
                    (_options.Verbose || _options.Preview
                        ? logLevel.Verbose()
                        : logLevel.Information())
                    .WriteTo.Logger(CreateLogger())
                    .CreateLogger();

                Updates.Cleanup(_options, () => OnUpdate(() => button.Content = GetProgressName(button.Content)));

                if (_options.UseCsProj)
                {
                    Updates.UpdateFoldersAndProjects(_options, () => OnUpdate(() => button.Content = GetProgressName(button.Content)));
                }
                else
                {
                    Updates.UpdateFoldersProjectsAndSolution(_options, () => OnUpdate(() => button.Content = GetProgressName(button.Content)));
                }
            }
            catch (Exception exception)
            {
                txtOutput.Text = exception.Message;
            }
            finally
            {
                txtOutput.Text = _sb.ToString();
                txtOutput.ScrollToEnd();
                button.Content = oldName;
                IsEnabled      = true;
            }
        }

        private string GetProgressName(object content)
        {
            return content switch
            {
                "|" => "/",
                "/" => "-",
                "-" => @"\",
                _   => "|"
            };
        }

        private void OnUpdate(Action action)
        {
            action();
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(
                DispatcherPriority.Background,
                new DispatcherOperationCallback(
                    delegate(object f)
                    {
                        ((DispatcherFrame) f).Continue = false;
                        return null;
                    }), frame);
            Dispatcher.PushFrame(frame);
        }

        private ILogger CreateLogger()
        {
            return new UILogger(x => _sb.AppendLine(x), _options.Verbose);
        }

        private void OnAddMask(object sender, RoutedEventArgs e)
        {
            _options.MaskCollection.Insert(0, new MaskItem("*.ext"));
        }

        private void OnRemoveMask(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is int index)
            {
                _options.MaskCollection.RemoveAt(index);
            }
        }

        private void OnRemoveAllMasks(object sender, RoutedEventArgs e)
        {
            _options.MaskCollection.Clear();
        }

        private void OnWorkingDirectoryChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string text = (sender as TextBox)?.Text;
                if (Path.IsPathRooted(text))
                {
                    _options.SolutionFile = Directory.EnumerateFiles(text, "*.sln").FirstOrDefault();
                }
            }
            catch
            {
            }
        }
    }
}