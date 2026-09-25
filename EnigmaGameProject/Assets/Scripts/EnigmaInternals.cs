
using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class EnigmaInternals : MonoBehaviour
{
    public System.Collections.Generic.Dictionary<char, char> plugChars = new Dictionary<char, char>();
    private char letter;
    private List<EnigmaPlug> plugsInter;

    private char[] config1;
    private char[] config2;
    private char[] config3;

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
        int index = (((letter - 'A') + rotorStepCount1)% 26) - 1;
        char letter2 = config1[index];
        index = (((letter2 - 'A') + rotorStepCount2)% 26) - 1;
        char letter3 = config2[index];
        index = (((letter3 - 'A') + rotorStepCount3)% 26) - 1;
        return config3[index];
    }
    public char reflect(char letter)
    {
        char [] config;
        config = new char[]
        {
            'Y', 'R', 'U', 'H', 'Q', 'S', 'L', 'D', 'P',
            'X', 'N', 'G', 'O', 'K', 'M', 'I', 'E', 'B',
            'F', 'Z', 'C', 'W', 'V', 'J', 'A', 'T'
        };
        int index = letter - 'A';
        return config[index];
    }

    public char rotorBackwardSir(char letter,int rotorStepCount1,int rotorStepCount2, int rotorStepCount3)
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
        int index = (((letter - 'A') + rotorStepCount1)% 26) - 1;
        char letter2 = config1[index];
        index = (((letter2 - 'A') + rotorStepCount2)% 26) - 1;
        char letter3 = config2[index];
        index = (((letter3 - 'A') + rotorStepCount3)% 26) - 1;
        return config3[index];
    }
    public string Convert(char letter,int rotorStepCount1,int rotorStepCount2,int rotorStepCount3)
    {
        char convertedLetter;
        // Starting conversion
        char theChar = char.ToUpper(letter);
        // Non-letter characters are left unchanged.
        if (theChar < 'A' || theChar > 'Z')
        {
        }
        else return null;

        // Plugboard Forward    
        if(plugChars.TryGetValue(char1, out char char2)){//check if char1 is in the dictionary and if it is output char2 and set it to char 1
            theChar = char2;
        }
        // Rotors Forward
        theChar = rotorForwardSir(theChar,rotorStepCount1,rotorStepCount2,rotorStepCount3);
        // Reflector
        theChar = reflect(theChar);
        // Rotors Backward
        theChar = rotorBackwardSir(theChar,rotorStepCount1,rotorStepCount2,rotorStepCount3);
        // Plugboard Backward
        if(plugChars.TryGetValue(char1, out char char2)){//check if char1 is in the dictionary and if it is output char2 and set it to char 1
            theChar = char2;
        }
        // Save character
        convertedLetter = theChar;
        return convertedLetter.ToString();
    }
}

internal class EnigmaPlug
{
}