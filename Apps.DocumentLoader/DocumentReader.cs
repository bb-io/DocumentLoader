using System.Text;
using DocumentFormat.OpenXml.Packaging;
using UglyToad.PdfPig;

namespace Apps.DocumentLoader;

public static class DocumentReader
{
    public static async Task<string> ReadDocument(byte[] file, string filename)
    {
        var fileExtension = filename.Split(".")[^1];
        string text;
        
        if (fileExtension == "txt")
            text = await file.ReadTxtFile();
        else if (fileExtension == "pdf")
            text = await file.ReadPdfFile();
        else if (fileExtension == "docx" || fileExtension == "doc")
            text = await file.ReadDocxFile();
        else
            throw new ArgumentException("Unsupported document format. Please provide docx, pdf or txt file.");

        return text;
    }
    
    private static async Task<string> ReadTxtFile(this byte[] file)
    {
        var stringBuilder = new StringBuilder();
        using (var stream = new MemoryStream(file))
        using (var reader = new StreamReader(stream))
        {
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                stringBuilder.Append(line);
            }
        }

        var document = stringBuilder.ToString();
        return document;
    }

    private static async Task<string> ReadPdfFile(this byte[] file)
    {
        using (var stream = new MemoryStream(file))
        {
            var document = PdfDocument.Open(stream);
            var text = string.Join(" ", document.GetPages().Select(p => p.Text));
            return text;
        }
    }
    
    private static async Task<string> ReadDocxFile(this byte[] file)
    { 
        using (var stream = new MemoryStream(file))
        {
            var document = WordprocessingDocument.Open(stream, false);
            var text = document.MainDocumentPart.Document.Body.InnerText;
            return text;
        }
    }
}