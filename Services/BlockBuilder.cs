class BlockBuilder
{
    public List<Block> BuildBlocks(List<Tagger> taggers, string inputFilePath)
    {
        var blocks = new List<Block>();
        var lines = File.ReadAllLines(inputFilePath);
        taggers = taggers.OrderBy(t => t.Order).ToList();

        for (int i = 0; i < taggers.Count; i++)
        {
            Tagger currentTagger = taggers[i];
            var block = new Block();
            
            if (currentTagger.Tag == "SignedComment")
            {
                block.IsMarkdown = true;
            }
            else if (currentTagger.Tag.StartsWith("NewCellBegin"))
            {
                block.IsCode = true;
            }

            int startLine = currentTagger.OpeningLine;
            int endLine = (i + 1 < taggers.Count) ? taggers[i + 1].OpeningLine : lines.Length;

            for (int j = startLine; j < endLine; j++)
            {
                if (block.IsCode && currentTagger.Tag == "DemoWhere")
                {
                    block.Lines.Add(lines[j].Substring(3)); // Uncomment the DemoWhere line
                }
                else if (!block.IsMarkdown || lines[j].StartsWith("--"))
                {
                    block.Lines.Add(lines[j]);
                }
            }

            if (block.IsMarkdown || block.IsCode)
            {
                blocks.Add(block);
            }
        }

        return blocks;
    }
}
