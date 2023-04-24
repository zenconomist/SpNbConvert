
class Block
{
    public bool IsMarkdown { get; set; }
    public bool IsCode { get; set; }

    public bool UnComment { get; set; }
    public Tagger OpeningTag { get; set; }
    public Tagger ClosingTag { get; set; }
    public List<string> Lines { get; set; }

    public Block()
    {
        Lines = new List<string>();
    }
}
