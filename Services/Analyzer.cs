using System.Text.RegularExpressions;

class Analyzer
{
    private List<TagType> _tagTypes;

    public Analyzer(List<TagType> tagTypes)
    {
        _tagTypes = tagTypes;
    }

    public List<Tagger> Analyze(string inputFilePath)
    {
        var taggers = new List<Tagger>();
        var lines = File.ReadAllLines(inputFilePath);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            foreach (var tagType in _tagTypes)
            {
                Match match = tagType.Pattern.Match(line);

                if (match.Success)
                {
                    int closingLine = -1;

                    if (tagType.IsOpening)
                    {
                        var closingTagType = _tagTypes.FirstOrDefault(t => t.Name == tagType.Name && t.IsClosing);

                        if (closingTagType != null)
                        {
                            closingLine = FindClosingLine(lines, closingTagType.Pattern, i);
                        }
                    }

                    var tagger = new Tagger(Guid.NewGuid().ToString(), tagType.Name, i, closingLine, taggers.Count);
                    taggers.Add(tagger);
                }
            }
        }

        return taggers;
    }

    private int FindClosingLine(string[] lines, Regex closingPattern, int openingLine)
    {
        for (int i = openingLine + 1; i < lines.Length; i++)
        {
            if (closingPattern.IsMatch(lines[i]))
            {
                return i;
            }
        }

        return -1;
    }

}