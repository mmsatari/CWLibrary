using System;
using System.Collections.Generic;
using System.Linq;

namespace CWLibrary
{

    public class TextToMorseSymbols
    {
        public TextToMorseSymbols()
        {
        }

        // Returns a byte array in the Wave file format containing the given
        // text in morse code
        public string ConvertToMorse(string text)
        {
            string symbols = GetText(text.ToLower());
            return symbols;
        }

        // Return a single character as a waveform
        private string GetCharacter(string character)
        {
            return Characters.Symbols[character];
        }

        // Return a word as a waveform
        private string GetWord(string word)
        {
            string data = "";

            for (int i = 0; i < word.Length; i++)
            {
                if (i > 0)
                    data += " ";
                if (word[i] == '<')
                {
                    // Prosign
                    int end = word.IndexOf('>', i);
                    if (end < 0)
                        throw new ArgumentException();
                    data += GetCharacter(word.Substring(i, end + 1 - i));
                    i = end;
                }
                else
                {
                    data += GetCharacter(word[i].ToString());
                }
            }

            return data;
        }

        // Return a string (lower case text only, unrecognized characters
        // throw an exception -- see Characters.cs for the list of recognized
        // characters) as a waveform wrapped in a DataChunk, ready to by added
        // to a wave file.
        private string GetText(string text)
        {
            string data = "";

            string[] words = text.Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                if (i > 0)
                    data += " ";
                data += GetWord(words[i]);
            }

            return data;
        }
    }
}