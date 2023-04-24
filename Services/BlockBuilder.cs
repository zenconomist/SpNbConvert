class BlockBuilder
{
    public List<Block> BuildBlocks(List<Tagger> taggers, string inputFilePath)
    {
        var blocks = new List<Block>();
        var lines = File.ReadAllLines(inputFilePath);
        taggers = taggers.OrderBy(t => t.Order).ToList();

        Stack<Block> blockStack = new Stack<Block>();

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
                block.OpeningTag = currentTagger;
                blockStack.Push(block);
                continue;
            }
            else if (currentTagger.Tag.StartsWith("NewCellEnd"))
            {
                if (blockStack.Count > 0)
                {
                    var openBlock = blockStack.Pop();
                    openBlock.ClosingTag = currentTagger;
                    blocks.Add(openBlock);
                }
                continue;
            }
            else if (currentTagger.Tag == "DemoWhere")
            {
                if (blockStack.Count > 0)
                {
                    blockStack.Peek().Lines.Add(lines[currentTagger.OpeningLine].Substring(3));
                }
                continue;
            }

            int startLine = currentTagger.OpeningLine;
            int endLine = (i + 1 < taggers.Count) ? taggers[i + 1].OpeningLine : lines.Length;

            for (int j = startLine; j < endLine; j++)
            {
                if (!block.IsMarkdown || lines[j].StartsWith("--"))
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
