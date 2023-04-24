using System.Text.RegularExpressions;

class TagType
{
    public string Name { get; set; }
    public Regex Pattern { get; set; }
    public bool IsOpening { get; set; }
    public bool IsClosing { get; set; }
    public bool IsSimple { get; set; }
}
