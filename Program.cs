
/*
    The Idea:
        A stored procedure sql file can have several tags.
        the tags can be simple or opening/closing tags of type.
        A Block can have one or several lines from the input file, and each block is defined by one or two Tags.
            If a tag type is simple, the block will only have one Tag.
            If a tag type is opening/closing, the block will have two Tags. An opening and a closing tag.
        A Block can by of type markdown or code.
        A Block can have one or several lines of text.

        An Analyzer object creates a list of Tags from the input file, the BlockBuilder creates Blocks from the list of Tags, and the NotebookBuilder creates a Notebook from the list of Blocks.

        The NoteBookBuilder creates Blocks in order of appearance of the tags in the input file.
*/
using System.Text.RegularExpressions;

string inputFilePath = "TestSp2.sql";
string outputFilePath = "TestSp2.ipynb";

var tagTypes = new List<TagType>
{
    new TagType
    {
        Name = "SignedComment",
        Pattern = new Regex(@"^\s*--\s*SignedComment:"),
        IsSimple = true
    },
    new TagType
    {
        Name = "NewCellBegin",
        Pattern = new Regex(@"^\s*--\s*NewCellBegin_\d+"),
        IsOpening = true
    },
    new TagType
    {
        Name = "NewCellEnd",
        Pattern = new Regex(@"^\s*--\s*NewCellEnd_\d+"),
        IsClosing = true
    },
    new TagType
    {
        Name = "DemoWhere",
        Pattern = new Regex(@"^\s*--\s*DemoWhere:"),
        IsSimple = true
    }
};

// Analyze the input file
Analyzer analyzer = new Analyzer(tagTypes);
List<Tagger> taggers = analyzer.Analyze(inputFilePath);

// Build blocks from tags
BlockBuilder blockBuilder = new BlockBuilder();
List<Block> blocks = blockBuilder.BuildBlocks(taggers, inputFilePath);

// Build the Jupyter Notebook
NotebookBuilder notebookBuilder = new NotebookBuilder();
notebookBuilder.BuildNotebook(blocks, outputFilePath);
