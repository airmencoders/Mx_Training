using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingQuotes : MonoBehaviour
{
    public bool DebugMode = false;
    float debugTimer = 0.0f;
    
    int CurrentQuote = 0;       // What quote is to be selected
    int LastQuote = 99;         // Last selected quote, to update only when needed (99 = uninitialized)
    const int MaxQuote = 8;     // How many quotes are loaded

    string[] quote_text = new string[MaxQuote + 1];        // Quote storage

    Text QuoteText;             // Text UI object


    // Start is called before the first frame update
    void Start()
    {
        if (DebugMode) Debug.Log("CAUTION: Motivational Text Deubgging Enabled");
        
        QuoteText = this.GetComponent<Text>();      // Get access to attached Text UI object
        CurrentQuote = (int)Random.Range(0.0f, 8.0f);       // Select a random quote

        LoadQuotes();       // Quotes are assigned to executable to prevent tampering
    }

    /// <summary>
    /// Checks for quote change, and updates as needed
    /// </summary>
    void Update()
    {
        if (CurrentQuote != LastQuote) ChangeQuote();       // Check for change, and update as needed
        if (DebugMode) DebugTests();
    }

    /// <summary>
    /// Selects a random quote
    /// </summary>
    public void RandomQuote()
    {
        CurrentQuote = (int)Random.Range(0.0f, 8.0f);       // Select a random quote
    }

    /// <summary>
    /// Changes the quote
    /// </summary>
    void ChangeQuote()
    {
        QuoteText.text = quote_text[CurrentQuote];      // Updates visible text
        LastQuote = CurrentQuote;       // Sets value to prevent polling
    }

    void LoadQuotes()
    {
        quote_text[0] = "Suggested by: TSgt Aaron Jagow\n\n" +
                        "Tell me and I forget\n" +
                        "Teach me and I may remember\n" +
                        "Involve me and I learn\n" +
                        " -Benjamin Franklin";
                        

        quote_text[1] = "Bugs? Suggestions? Questions?\n\n" +
                        "E-mail: devin.bable@us.af.mil";

        quote_text[2] = "Suggested by: TSgt Leigh Miller\n\n" +
                        "You have to think a little smarter\n" +
                        "Be proactive, not reactive\n" +
                        " -Frank Abagnale";

        quote_text[3] = "Suggested by: TSgt Julius Caluya\n\n" +
                        "Knowledge is power,\n" +
                        "Knowledge without action is useless\n" +
                        " -Direct source unknown";
                        
        quote_text[4] = "Suggested by: SSgt Mark Walden\n\n" +
                        "NOVRAM reset!\n" +
                        " -Every ICCN C-17 maintainer";

        quote_text[5] = "Suggested by: TSgt Andrew Ferenchak\n\n" +
                        "Nothing in this world can take the place of persistence. \n" +
                        "Talent will not: nothing is more common than unsuccessful men with talent. \n" +
                        "Genius will not; unrewarded genius is almost a proverb. \n" +
                        "Education will not: the world is full of educated derelicts. \n" +
                        "Persistence and determination alone are omnipotent.\n" +
                        " -Calvin Coolidge";

        quote_text[6] = "Suggested by: TSgt Samuel Nelson\n\n" +
                        "Your attitude, not your aptitude, will determine your altitude" +
                        " -Zig Ziglar";
                        

        quote_text[7] = "Suggested by: TSgt Benjamin Hoag\n\n" +
                        "A man does what he must in spite of personal consequences,\n" +
                        "in spite of obstacles and dangers and pressures\n" +
                        "and that is the basis for all human morality\n" +
                        " -Winston S. Churchill";
                        

        quote_text[8] = "Suggested by: TSgt Jonathan Soles\n\n" +
                        "If you win the morning, you win the day" +
                        "-Tim Ferriss";
    }

    /// <summary>
    /// Runs a cycle of all quotes to test for possible issues
    /// </summary>
    void DebugTests()
    {
        debugTimer += Time.deltaTime;       // Increment timer
        if (debugTimer > 0.25f)     // Cycle time
        {
            CurrentQuote++;         // Increment quote
            debugTimer = 0.0f;      // Reset timer
        }
        if (CurrentQuote > MaxQuote) CurrentQuote = 0;      // Clamp quote to max quotes
    }
}
