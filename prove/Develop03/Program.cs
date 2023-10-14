using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

class Word
{
    public string Text { get; }
    public bool Hidden { get; private set; }

    public Word(string text)
    {
        Text = text;
        Hidden = false;
    }

    public void Hide()
    {
        Hidden = true;
    }
}

class Reference
{
    public string Text { get; }

    public Reference(string text)
    {
        Text = text;
    }
}

class Scripture
{
    public Reference Reference { get; }
    private List<Word> Words { get; }

    public Scripture(Reference reference, string text)
    {
        Reference = reference;
        Words = text.Split(' ').Select(word => new Word(word)).ToList();
    }

    public bool IsCompletelyHidden()
    {
        return Words.All(word => word.Hidden);
    }

    public void Display()
    {
        Console.Clear();
        Console.WriteLine($"{Reference.Text}\n");
        foreach (var word in Words)
        {
            Console.Write(word.Hidden ? "______ " : word.Text + " ");
        }
        Console.WriteLine();
    }

    public string GetHiddenWord()
    {
        var hiddenWord = Words.Find(word => word.Hidden);
        return hiddenWord?.Text;
    }

    public void HideRandomWord()
    {
        var unhiddenWords = Words.Where(word => !word.Hidden).ToList();
        if (unhiddenWords.Count > 0)
        {
            var random = new Random();
            var indexToHide = random.Next(unhiddenWords.Count);
            unhiddenWords[indexToHide].Hide();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        string scriptureText = "For God so loved the world that he gave his one and only Son, that whoever believes in him shall not perish but have eternal life. - John 3:16";
        string referenceText = "John 3:16";

        Reference reference = new Reference(referenceText);
        Scripture scripture = new Scripture(reference, scriptureText);

        while (!scripture.IsCompletelyHidden())
        {
            scripture.Display();
            Console.Write("Guess the hidden word (or type 'quit' to exit): ");
            string userInput = Console.ReadLine();
            if (userInput.ToLower() == "quit")
            {
                break;
            }
            string hiddenWord = scripture.GetHiddenWord();
            if (userInput.ToLower() == hiddenWord.ToLower())
            {
                scripture.HideRandomWord();
            }
        }

        scripture.Display();
        Console.WriteLine("You've completed the scripture hiding and guessing game!");
    }
}
