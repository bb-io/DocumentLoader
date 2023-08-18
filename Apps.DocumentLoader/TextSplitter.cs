using System.Text.RegularExpressions;

namespace Apps.DocumentLoader;

public class TextSplitter
{
    private readonly List<string> _separators = new() { "\n\n", "\n", "   ", "  ", " ", "" };
    private readonly int _maximumChunkSize; 
    private readonly int _maximumChunkOverlap;

    public TextSplitter(int maximumChunkSize = 4000, int maximumChunkOverlap = 200)
    {
        _maximumChunkSize = maximumChunkSize;
        _maximumChunkOverlap = maximumChunkOverlap;
    }

    public List<string> SplitText(string text)
    {
        return SplitText(text, _separators);
    }
    
    private List<string> SplitText(string text, List<string> separators)
    {
        var finalChunks = new List<string>();
        var separator = separators[^1];
        var separatorsLeft = new List<string>();

        for (int i = 0; i < separators.Count; i++)
        {
            var currentSeparator = separators[i];
            var escapedSeparator =  Regex.Escape(currentSeparator);

            if (currentSeparator == "")
            {
                separator = separators[i];
                break;
            }

            if (Regex.IsMatch(text, escapedSeparator)) 
            {
                separator = currentSeparator;
                separatorsLeft = separators.GetRange(i + 1, separators.Count - (i + 1));
                break;
            }
        }

        var separatorToUse = Regex.Escape(separator);
        var splits = SplitTextWithRegex(text, separatorToUse);
        
        var goodSplits = new List<string>();
        separatorToUse = separator;

        foreach (var split in splits)
        {
            if (split.Length < _maximumChunkSize)
                goodSplits.Add(split);
            else
            {
                if (goodSplits.Count > 0)
                {
                    List<string> mergedText = MergeSplits(goodSplits, separatorToUse);
                    finalChunks.AddRange(mergedText);
                    goodSplits.Clear();
                }

                if (separatorsLeft.Count == 0)
                    finalChunks.Add(split);
                else
                {
                    List<string> otherInfo = SplitText(split, separatorsLeft);
                    finalChunks.AddRange(otherInfo);
                }
            }
        }

        if (goodSplits.Count > 0)
        {
            List<string> mergedText = MergeSplits(goodSplits, separatorToUse);
            finalChunks.AddRange(mergedText);
        }

        return finalChunks;
    }

    private IEnumerable<string> SplitTextWithRegex(string text, string separator)
    {
        var splits = Regex.Split(text, separator);
        return splits.Where(s => s != "");
    }

    private List<string> MergeSplits(List<string> splits, string separator)
    {
        var separatorLength = separator.Length;
        var texts = new List<string>();
        var currentTexts = new List<string>();
        var totalLength = 0;
    
        foreach (var split in splits)
        {
            var splitLength = split.Length;
            
            if (totalLength + splitLength + (currentTexts.Count > 0 ? separatorLength : 0) > _maximumChunkSize)
            { 
                if (currentTexts.Count > 0)
                {
                    var text = JoinTexts(currentTexts, separator);
                    if (!string.IsNullOrWhiteSpace(text))
                        texts.Add(text);
                    
                    while (totalLength > _maximumChunkOverlap 
                           || (totalLength + splitLength + (currentTexts.Count > 0 ? separatorLength : 0) > _maximumChunkSize 
                               && totalLength > 0))
                    {
                        totalLength -= currentTexts[0].Length + (currentTexts.Count > 1 ? separatorLength : 0);
                        currentTexts.RemoveAt(0);
                    }
                }
            }
        
            currentTexts.Add(split);
            totalLength += splitLength + (currentTexts.Count > 1 ? separatorLength : 0);
        }
    
        var finalText = JoinTexts(currentTexts, separator);
        if (!string.IsNullOrWhiteSpace(finalText))
            texts.Add(finalText);

        return texts;
    }
    
    private string JoinTexts(List<string> texts, string separator)
    {
        var text = string.Join(separator, texts).Trim();
        return text;
    }
}