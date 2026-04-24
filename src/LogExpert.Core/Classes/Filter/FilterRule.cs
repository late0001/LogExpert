using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LogExpert.Core.Classes.Filter;

[Serializable]
[XmlRoot("FilterRule")]
public class FilterRule
{
    public string Text { get; set; }
    public string Description { get; set; }
    public bool IsExclude { get; set; }
    public bool MatchCase { get; set; }
    public bool IsRegex { get; set; }
    [XmlIgnore]
    public Color TextColor { get; set; } = Color.Black;
    [XmlIgnore]
    public Color Background { get; set; } = Color.Transparent;

    // 👇 XML 序列化用（自动转颜色）
    [XmlElement("TextColor")]
    public string TextColorXml
    {
        get => ColorTranslator.ToHtml(TextColor);
        set => TextColor = ColorTranslator.FromHtml(value);
    }

    [XmlElement("Background")]
    public string BackgroundXml
    {
        get => ColorTranslator.ToHtml(Background);
        set => Background = ColorTranslator.FromHtml(value);
    }
}

[Serializable]
[XmlRoot("FilterRules")]
public class FilterRuleList
{
    [XmlArray("Rules"), XmlArrayItem("Rule")]
    public List<FilterRule> Rules { get; set; } = new();
}
