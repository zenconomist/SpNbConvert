
public class Tagger
{

    // Tagger will have following properties: Id guid, Tag string, OpeningLine int, ClosingLine int, Order int, Pattern string
    public string Id { get; set; }
    public string Tag { get; set; }
    public int OpeningLine { get; set; }
    public int ClosingLine { get; set; }
    public int Order { get; set; }
    public string Pattern { get; set; }

    public Tagger(string id, string tag, int openingLine, int closingLine, int order)
    {
        Id = id;
        Tag = tag;
        OpeningLine = openingLine;
        ClosingLine = closingLine;
        Order = order;
    }

}
