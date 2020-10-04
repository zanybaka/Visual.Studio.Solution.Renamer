using System.Diagnostics;

namespace Visual.Studio.Solution.Renamer.Desktop.UI
{
    [DebuggerDisplay("Value = {Value}")]
    public class MaskItem
    {
        public MaskItem()
        {
        }

        public MaskItem(string value)
        {
            Value = value;
        }

        public string Value { get; set; }

        public static implicit operator MaskItem(string value)
        {
            return new MaskItem { Value = value };
        }
    }
}