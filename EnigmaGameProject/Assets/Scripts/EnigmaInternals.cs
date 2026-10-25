 
using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class EnigmaInternals : MonoBehaviour
{

    [System.NonSerialized]
    public Dictionary<char, char> plugChars = new Dictionary<char, char>();
    [System.NonSerialized]
    public Dictionary<char, char> plugCharsReverse = new Dictionary<char, char>();
    public int[] plugIndex = new int[] {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25};
    private char[] config1;
    private char[] config2;
    private char[] config3;

    [SerializeField] public bool selectingPlugs;

    public char rotorForwardSir(char letter,int rotorStepCount1,int rotorStepCount2, int rotorStepCount3)
    {
        letter = char.ToUpper(letter);

        config1 = new char[]
        {
            'H', 'D', 'R', 'K', 'B', 'F', 'A', 'C', 'O',
            'Y', 'G', 'M', 'J', 'X', 'N', 'T', 'I', 'Q',
            'Z', 'V', 'P', 'W', 'U', 'S', 'L', 'E'
        };
        config2 = new char[]
        {
            'G', 'P', 'X', 'C', 'M', 'A', 'N', 'Q', 'I',
            'W', 'O', 'Z', 'R', 'V', 'B', 'U', 'S', 'H',
            'F', 'T', 'K', 'E', 'J', 'L', 'D', 'Y'
        };
        config3 = new char[]
        {
            'B', 'C', 'D', 'T', 'M', 'S', 'F', 'O', 'X',
            'Q', 'H', 'J', 'N', 'Z', 'V', 'G', 'P', 'W',
            'R', 'E', 'Y', 'U', 'A', 'I', 'K', 'L'
        };
        
        if (letter < 'A' || letter > 'Z')
        {
            return letter;
        }
        int index = ((letter - 'A' + rotorStepCount1) % 26);
        letter = config1[index];
        int signal = (letter - 'A' - rotorStepCount1 + 26) % 26;

        index = ((signal + rotorStepCount2) % 26);
        char letter2 = config2[index];
        signal = (letter2 - 'A' - rotorStepCount2 + 26) % 26;

        index = ((signal + rotorStepCount3) % 26);
        char letter3 = config3[index];
        signal = (letter3 - 'A' - rotorStepCount3 + 26) % 26;
        return (char)(signal+'A');
    }
    public char reflect(char letter)
    {
        char [] config;
        config = new char[]
        {
            'Y', 'R', 'U', 'H', 'Q', 'S', 'L', 'D', 'P', 'X', 'N', 'G', 'O',
            'K', 'M', 'I', 'E', 'B', 'F', 'Z', 'C', 'W', 'V', 'J', 'A', 'T'
        };
        int index = letter - 'A';
        return config[index];
    }

    public char rotorBackwardSir(char letter,int rotorStepCount1,int rotorStepCount2, int rotorStepCount3)
    {
        letter = char.ToUpper(letter);
        char [] config1Back = new char[26];
        char [] config2Back = new char[26];
        char [] config3Back = new char[26];
        for(int i = 0; i < 26; i++)
        {
            config1Back[config1[i]-'A'] = (char)(i + 'A');
            config2Back[config2[i]-'A'] = (char)(i + 'A');
            config3Back[config3[i]-'A'] = (char)(i + 'A');
        }
        
        if (letter < 'A' || letter > 'Z')
        {
            return letter;
        }
        int index = ((letter - 'A' + rotorStepCount3) % 26);
        char letter3 = config3Back[index];
        int signal = (letter3 - 'A' - rotorStepCount3 + 26) % 26;

        index = ((signal + rotorStepCount2) % 26);
        char letter2 = config2Back[index];
        signal = (letter2 - 'A' - rotorStepCount2 + 26) % 26;


        index = ((signal + rotorStepCount1) % 26);
        char letter1 = config1Back[index];
        signal = (letter1 - 'A' - rotorStepCount1 + 26) % 26;
        return (char)(signal+'A');
    }
    public string Convert(char letter,int rotorStepCount1,int rotorStepCount2,int rotorStepCount3)
    {
        char convertedLetter;
        // Starting conversion 
        // Non-letter characters are left unchanged.
        char theChar = char.ToUpper(letter);
        char char2;
        // Plugboard Forward 
        Debug.Log("IN: "+theChar);
        if(plugChars.TryGetValue(theChar, out char2)){//check if char1 is in the dictionary and if it is output char2 and set it to char 1
            theChar = char2;
        } else if(plugCharsReverse.TryGetValue(theChar, out char2)){
            theChar = char2;
        }
        Debug.Log(theChar);
        // Rotors Forward
        theChar = rotorForwardSir(theChar,rotorStepCount1,rotorStepCount2,rotorStepCount3);
        // Reflector
        theChar = reflect(theChar);
        // Rotors Backward
        theChar = rotorBackwardSir(theChar,rotorStepCount1,rotorStepCount2,rotorStepCount3);
        // Plugboard Backward
        Debug.Log(theChar);
        if(plugCharsReverse.TryGetValue(theChar, out char2)){//check if char1 is in the dictionary and if it is output char2 and set it to char 1
            theChar = char2;
        } else if(plugChars.TryGetValue(theChar, out char2)){//check if char1 is in the dictionary and if it is output char2 and set it to char 1
            theChar = char2;
        }
        Debug.Log("OUT: "+theChar);
        // Save character
        convertedLetter = theChar;
        return convertedLetter.ToString();
    }
}
