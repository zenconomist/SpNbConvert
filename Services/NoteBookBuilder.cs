using Newtonsoft.Json;

class NotebookBuilder
{
    public void BuildNotebook(List<Block> blocks, string outputFilePath)
    {
        // Create the IPYNB file structure
        dynamic notebook = new
        {
            cells = new List<dynamic>(),
            metadata = new
            {
                kernelspec = new
                {
                    display_name = "SQL",
                    language = "sql",
                    name = "mssql"
                },
                language_info = new
                {
                    codemirror_mode = "sql",
                    file_extension = ".sql",
                    mimetype = "text/x-sql",
                    name = "sql"
                }
            },
            nbformat = 4,
            nbformat_minor = 5
        };

        foreach (var block in blocks)
        {
            dynamic cell = new
            {
                cell_type = block.IsMarkdown ? "markdown" : "code",
                metadata = new { },
                source = block.Lines.Select(line => line + "\n").ToList()
            };

            notebook.cells.Add(cell);
        }

        string json = JsonConvert.SerializeObject(notebook, Formatting.Indented);
        File.WriteAllText(outputFilePath, json);
    }
}
