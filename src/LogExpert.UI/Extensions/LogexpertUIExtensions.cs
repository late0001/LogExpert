using System.Runtime.Versioning;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace LogExpert.UI.Extensions;

[SupportedOSPlatform("windows")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1708:Identifiers should differ by more than case", Justification = "Intentionally")]
internal static class LogexpertUIExtensions
{
    /// <seealso href="https://stackoverflow.com/a/4842576/198788"/>
    public static int GetMaxTextWidth (this ComboBox comboBox)
    {
        if (comboBox == null)
            return 0;

        int maxTextWidth = comboBox.Width;

        foreach (var item in comboBox.Items)
        {
            if (item == null)
                continue;

            int textWidthInPixels = TextRenderer.MeasureText(item.ToString(), comboBox.Font).Width;

            if (textWidthInPixels > maxTextWidth)
            {
                maxTextWidth = textWidthInPixels;
            }
        }

        return maxTextWidth;
    }

    /// <summary>
    /// Enumerates all controls within the specified parent control, including nested child controls.
    /// </summary>
    /// <param name="parent">The parent control whose child controls are to be enumerated. Cannot be <see langword="null"/>.</param>
    /// <returns>An <see cref="IEnumerable{Control}"/> of <see cref="Control"/> objects representing all controls within the parent,
    /// including nested children.</returns>
    [SupportedOSPlatform("windows")]
    public static IEnumerable<Control> ControlsRecursive (this Control parent)
    {
        ArgumentNullException.ThrowIfNull(parent, nameof(parent));

        foreach (Control control in parent.Controls)
        {
            yield return control;

            // recurse into children
            foreach (var child in control.ControlsRecursive())
            {
                yield return child;
            }
        }
    }
}
