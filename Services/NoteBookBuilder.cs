using Newtonsoft.Json;

class NotebookBuilder
{
    public void BuildNotebook(List<Block> blocks, string outputFilePath)
    {
        var notebook = new
        {
            cells = blocks.Select(block => new
            {
                cell_type = block.IsMarkdown ? "markdown" : "code",
                metadata = new { },
                source = block.Lines.Select(line => line + "\n").ToList()
            }).ToList(),
            metadata = new
            {
                language_info = new { name = "sql" }
            },
            nbformat = 4,
            nbformat_minor = 2
        };

        string json = JsonConvert.SerializeObject(notebook, Formatting.Indented);
        File.WriteAllText(outputFilePath, json);
    }
}
