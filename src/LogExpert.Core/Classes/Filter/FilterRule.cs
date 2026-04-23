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
    public Color HighlightColor { get; set; } = Color.Transparent;
}

[Serializable]
[XmlRoot("FilterRules")]
public class FilterRuleList
{
    [XmlArray("Rules"), XmlArrayItem("Rule")]
    public List<FilterRule> Rules { get; set; } = new();
}
