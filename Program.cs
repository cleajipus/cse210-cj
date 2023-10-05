using System;
using System.Collections.Generic;
using System.IO;

class Entry
{
    public string Prompt { get; set; }
    public string Response { get; set; }
    public string Date { get; set; }
}

class Journal
{
    private List<Entry> entries = new List<Entry>();

    public void WriteEntry(string prompt, string response)
    {
        Entry entry = new Entry
        {
            Prompt = prompt,
            Response = response,
            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
        entries.Add(entry);
    }

    public void DisplayJournal()
    {
        foreach (Entry entry in entries)
        {
            Console.WriteLine($"Date: {entry.Date}");
            Console.WriteLine($"Prompt: {entry.Prompt}");
            Console.WriteLine($"Response: {entry.Response}");
            Console.WriteLine();
        }
    }

    public void SaveToFile(string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            foreach (Entry entry in entries)
            {
                writer.WriteLine($"Date: {entry.Date}");
                writer.WriteLine($"Prompt: {entry.Prompt}");
                writer.WriteLine($"Response: {entry.Response}");
                writer.WriteLine();
            }
        }
        Console.WriteLine($"Journal saved to {filename}");
    }

    public void LoadFromFile(string filename)
    {
        entries.Clear();
        try
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                Entry entry = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("Date: "))
                    {
                        entry = new Entry();
                        entry.Date = line.Substring(6);
                    }
                    else if (line.StartsWith("Prompt: "))
                    {
                        entry.Prompt = line.Substring(8);
                    }
                    else if (line.StartsWith("Response: "))
                    {
                        entry.Response = line.Substring(10);
                        entries.Add(entry);
                    }
                }
            }
            Console.WriteLine($"Journal loaded from {filename}");
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found.");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Journal journal = new Journal();

        string[] prompts = {
            "Who was the most interesting person I interacted with today?",
            "What was the best part of my day?",
            "How did I see the hand of the Lord in my life today?",
            "What was the strongest emotion I felt today?",
            "If I had one thing I could do over today, what would it be?"
        };

        while (true)
        {
            Console.WriteLine("\nJournal Program Menu:");
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display the journal");
            Console.WriteLine("3. Save the journal to a file");
            Console.WriteLine("4. Load the journal from a file");
            Console.WriteLine("5. Quit");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Select a prompt (1-5):");
                    for (int i = 0; i < prompts.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {prompts[i]}");
                    }
                    int promptChoice;
                    if (int.TryParse(Console.ReadLine(), out promptChoice) && promptChoice >= 1 && promptChoice <= prompts.Length)
                    {
                        Console.WriteLine("Enter your response:");
                        string response = Console.ReadLine();
                        journal.WriteEntry(prompts[promptChoice - 1], response);
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Please select a valid prompt.");
                    }
                    break;

                case "2":
                    journal.DisplayJournal();
                    break;

                case "3":
                    Console.WriteLine("Enter the filename to save the journal to:");
                    string saveFilename = Console.ReadLine();
                    journal.SaveToFile(saveFilename);
                    break;

                case "4":
                    Console.WriteLine("Enter the filename to load the journal from:");
                    string loadFilename = Console.ReadLine();
                    journal.LoadFromFile(loadFilename);
                    break;

                case "5":
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }
        }
    }
}
