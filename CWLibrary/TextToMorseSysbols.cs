using System;
using System.Collections.Generic;
using System.Linq;

namespace CWLibrary
{
    public class TextToMorseSymbols
    {
        public TextToMorseSymbols(int charSpeed, int wordSpeed, double frequency)
        {
            CharacterSpeed = charSpeed;
            WordSpeed = wordSpeed;
            Frequency = frequency;
        }

        public TextToMorseSymbols(int charSpeed, int wordSpeed)
            : this(charSpeed, wordSpeed, 600.0)
        {
        }

        public TextToMorseSymbols(int wpm)
            : this(wpm, wpm)
        {
        }

        public TextToMorseSymbols()
            : this(20)
        {
        }

        // Character speed in WPM
        public int CharacterSpeed { get; set; }

        // Overall speed in WPM (must be <= character speed)
        public int WordSpeed { get; set; }

        // Tone frequency
        public double Frequency { get; set; }

        // Returns a byte array in the Wave file format containing the given
        // text in morse code
        public string ConvertToMorse(string text)
        {
            string symbols = GetText(text.ToLower());
            return symbols;
        }

        // Return given number of seconds of sine wave
        private short[] GetWave(double seconds)
        {
            short[] waveArray;
            int samples = (int) (11025*seconds);

            waveArray = new short[samples];

            for (int i = 0; i < samples; i++)
            {
                waveArray[i] = Convert.ToInt16(32760*Math.Sin(i*2*Math.PI*Frequency/11025));
            }

            return waveArray;
        }

        // Return given number of seconds of flatline. This could also be
        // achieved with slnt chunks inside a wavl chunk, but the resulting
        // file might not be universally readable. If saving space is that
        // important, it would be better to compress the output as mp3 or ogg
        // anyway.
        private short[] GetSilence(double seconds)
        {
            short[] waveArray;
            int samples = (int) (11025*seconds);

            waveArray = new short[samples];

            return waveArray;
        }

        // Return a single character as a waveform
        private string GetCharacter(string character)
        {
            return Characters.InternationalSymbols[character];
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