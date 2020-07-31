using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MotivationalText : MonoBehaviour {

    bool debugMode = false;
    int debugSelect = 0;
    float debugTimer = 0.0f;

    string[] quote_text = new string[9];
    string[] quote_author = new string[9];
    string[] quote_suggested = new string[9];

    TextMesh motivationalText;

    // Use this for initialization
    void Start () {
        if (debugMode) Debug.Log("Motivational text debugging enabled");
        SetQuotes();
        motivationalText = GetComponent<TextMesh>();
        motivationalText.text = "";
        int r = (int)Random.Range(0.0f, 8.0f);

        if (r > 8)
        {
            string dbgString = "ERROR: A non-existant quote was selected (" + r + ")";
            Debug.Log(dbgString);
        }

        if (!debugMode)motivationalText.text = quote_text[r] + "\n\n" + quote_author[r] + "\n\n\nSuggested by:\n" + quote_suggested[r];
    }

    private void Update()
    {
        if (!debugMode) return;
        motivationalText.text = quote_text[debugSelect] + "\n\n" + quote_author[debugSelect] + "\n\n\nSuggested by:\n" + quote_suggested[debugSelect];
        
        
        debugTimer += Time.deltaTime;
        if (debugTimer > 5.0f)
        {
            debugSelect++;
            debugTimer = 0.0f;
            if (debugSelect > 8) debugSelect = 0;
            if (debugSelect == 1) debugSelect++;                    // Skip this one for now
        }
        

    }

    public void RandomQuote()
    {
        int r = (int)Random.Range(0.0f, 8.0f);

        if (r > 8)
        {
            string dbgString = "ERROR: A non-existant quote was selected (" + r + ")";
            Debug.Log(dbgString);
        }

        if (!debugMode) motivationalText.text = quote_text[r] + "\n\n" + quote_author[r] + "\n\n\nSuggested by:\n" + quote_suggested[r];
    }

    void SetQuotes()
    {
        quote_text[0] =    "Tell me and I forget\n" +
                            "Teach me and I may remember\n" +
                            "Involve me and I learn";
        quote_author[0] =  "-Benjamin Franklin";
        quote_suggested[0] = "TSgt Aaron Jagow";

        quote_text[1] = "Suggest a new quote!\n\nE-mail: devin.bable@us.af.mil";
        quote_author[1] = "";
        quote_suggested[1] = "";

        quote_text[2] =    "You have to think a little smarter\n" +
                           "Be proactive, not reactive";
        quote_author[2] = "-Frank Abagnale";
        quote_suggested[2] = "TSgt Leigh Miller";

        quote_text[3] = "Knowledge is power,\n" +
            "Knowledge without action is useless";
        quote_author[3] = "-Unknown";
        quote_suggested[3] = "TSgt Julius Caluya";

        quote_text[4] = "NOVRAM reset!";
        quote_author[4] = "-Every ICCN C-17 maintainer ever.";
        quote_suggested[4] = "SSgt Mark Walden";

        quote_text[5] = "Nothing in this world can take the place of persistence. \n" +
            "Talent will not: nothing is more common than unsuccessful men with talent. \n" +
            "Genius will not; unrewarded genius is almost a proverb. \n" +
            "Education will not: the world is full of educated derelicts. \n" +
            "Persistence and determination alone are omnipotent.";
        quote_author[5] = "-Calvin Coolidge";
        quote_suggested[5] = "TSgt Ferenchak";

        quote_text[6] = "Your attitude, not your aptitude, will determine your altitude";
        quote_author[6] = "-Zig Ziglar";
        quote_suggested[6] = "TSgt Samuel Nelson";

        quote_text[7] = "A man does what he must in spite of personal consequences,\n" +
            "in spite of obstacles and dangers and pressures\n" +
            "and that is the basis for all human morality";
        quote_author[7] = "-Winston S. Churchill";
        quote_suggested[7] = "TSgt Benjamin Hoag";

        quote_text[8] = "If you win the morning, you win the day";
        quote_author[8] = "-Tim Ferriss";
        quote_suggested[8] = "TSgt Jonathan Soles";
    }
}
