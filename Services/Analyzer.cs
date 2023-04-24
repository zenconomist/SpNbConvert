using System.Text.RegularExpressions;

class Analyzer
{
    public List<Tagger> Analyze(string inputFilePath)
    {
        var taggers = new List<Tagger>();
        var lines = File.ReadAllLines(inputFilePath);
        
        Regex signedCommentRegex = new Regex(@"^\s*--\s*SignedComment:", RegexOptions.IgnoreCase);
        Regex newCellBeginRegex = new Regex(@"^\s*--\s*NewCellBegin_(\d+)", RegexOptions.IgnoreCase);
        Regex newCellEndRegex = new Regex(@"^\s*--\s*NewCellEnd_(\d+)", RegexOptions.IgnoreCase);
        Regex demoWhereRegex = new Regex(@"^\s*--\s*DemoWhere:", RegexOptions.IgnoreCase);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            if (signedCommentRegex.IsMatch(line))
            {
                taggers.Add(new Tagger(Guid.NewGuid().ToString(), "SignedComment", i, -1, taggers.Count));
            }
            else
            {
                Match newCellBeginMatch = newCellBeginRegex.Match(line);
                if (newCellBeginMatch.Success)
                {
                    string tag = "NewCellBegin_" + newCellBeginMatch.Groups[1].Value;
                    taggers.Add(new Tagger(Guid.NewGuid().ToString(), tag, i, -1, taggers.Count));
                }
                else
                {
                    Match newCellEndMatch = newCellEndRegex.Match(line);
                    if (newCellEndMatch.Success)
                    {
                        string openingTag = "NewCellBegin_" + newCellEndMatch.Groups[1].Value;
                        var openingTagger = taggers.LastOrDefault(t => t.Tag == openingTag && t.ClosingLine == -1);
                        if (openingTagger != null)
                        {
                            openingTagger.ClosingLine = i;
                        }
                    }
                    else if (demoWhereRegex.IsMatch(line))
                    {
                        taggers.Add(new Tagger(Guid.NewGuid().ToString(), "DemoWhere", i, -1, taggers.Count));
                    }
                }
            }
        }

        return taggers;
    }
}
