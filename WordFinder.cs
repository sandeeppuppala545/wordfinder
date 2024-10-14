using System;
using System.Collections.Generic;
using System.Linq;

public class WordFinder
{
    private HashSet<string> validWords;
    private char[,] matrix;
    private int rows;
    private int columns;

    public WordFinder(IEnumerable<string> input)
    {
        if (input == null || !input.Any())
        {
            throw new ArgumentException("Input cannot be null or empty.");
        }

        rows = input.Count();
        columns = input.First().Length;
        matrix = new char[rows, columns];
        validWords = new HashSet<string>();

        int r = 0;
        foreach (var line in input)
        {
            if (line.Length != columns)
            {
                throw new ArgumentException("All lines must have the same length.");
            }
            for (int c = 0; c < columns; c++)
            {
                matrix[r, c] = line[c];
            }
            r++;
        }

        ExtractValidWords();
    }

    private void ExtractValidWords()
    {
        // Horizontal words
        for (int r = 0; r < rows; r++)
        {
            validWords.Add(new string(Enumerable.Range(0, columns).Select(c => matrix[r, c]).ToArray()));
        }

        // Vertical words
        for (int c = 0; c < columns; c++)
        {
            validWords.Add(new string(Enumerable.Range(0, rows).Select(r => matrix[r, c]).ToArray()));
        }
    }

    public IEnumerable<string> Find(IEnumerable<string> wordstream)
    {
        if (wordstream == null || !wordstream.Any())
            return Enumerable.Empty<string>();

        var wordCount = new Dictionary<string, int>();
        foreach (var word in wordstream)
        {
            if (validWords.Contains(word))
            {
                if (!wordCount.ContainsKey(word))
                {
                    wordCount[word] = 0;
                }
                wordCount[word]++;
            }
        }

        return wordCount.OrderByDescending(c => c.Value)
                        .Select(c => c.Key)
                        .Take(10);
    }

    public static void Main(string[] args)
    {
        var input = new List<string>
        {
            "chil",
            "cold",
            "wind",
            "exam"
        };

        var wordstream = new List<string>
        {
            "chil", "cold", "word", "wind", "hell", "worl", "exam", "exam"
        };

        var wordFinder = new WordFinder(input);
        var foundWords = wordFinder.Find(wordstream);

        Console.WriteLine("Found words:");
        foreach (var word in foundWords)
        {
            Console.WriteLine(word);
        }
    }
}
