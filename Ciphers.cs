using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherThing
{
    public class Ciphers
    {

        #region polybius square cipher
        public static char[,] Polybius =
        {
            {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I'},
            
            {'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R' },
            
            {'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0' },
            
            {'1', '2', '3', '4', '5', '6', '7', '8', '9' },
            
            {'`', '~', '!', '@', '#', '$', '%', '^', '&' },
            
            {'*', '(', ')', '-', '_', '+', '=', '{', '}' },
            
            {'[', ']', '|', '\\', ':', ';', '\'', '\"', '<' },

            {'>', '/', 'Æ', '÷', 'Œ', 'Ø', 'ß', 'È', 'Ä' },
            
            {'Ñ', 'Ö', 'Ü', 'Ð', '¢', '£', '¤', 'Ì', ' ' },
        };

        public static char[,] custom;

        public static void MakeCustomSquare(string filePath)
        {
            //Use a Custom Polybius square from a file
            string[] input = System.IO.File.ReadAllLines(filePath);
            
            int h = input.Length;
            int w = input[0].Length;
            char[,] customSquare = new char[h, w];

            for (int line = 0; line < h; line++)
            {
                char[] charas = input[line].ToCharArray();
                for (int c = 0; c < w; c++)
                {
                    customSquare[line, c] = charas[c];
                }
            }
            custom = customSquare;
        }
 
        static void ResetPolybiusSquare(bool useCustomSquare)
        {
            if (!useCustomSquare)
            {
                Polybius = new char[,] {
                    { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I'},

                    { 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R' },

                    { 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0' },

                    { '1', '2', '3', '4', '5', '6', '7', '8', '9' },

                    { '`', '~', '!', '@', '#', '$', '%', '^', '&' },

                    { '*', '(', ')', '-', '_', '+', '=', '{', '}' },

                    { '[', ']', '|', '\\', ':', ';', '\'', '\"', '<' },

                    { '>', '/', 'Æ', '÷', 'Œ', 'Ø', 'ß', 'È', 'Ä' },

                    { 'Ñ', 'Ö', 'Ü', 'Ð', '¢', '£', '¤', 'Ì', ' ' },
                };
            }
            else
            {
                Polybius = custom.MakeDuplicate();
            }
        }

        static int FindPolybiusCoords(char c)
        {
            Tuple<int, int> coords = Polybius.CoordinatesOf(char.IsLetter(c) ? char.ToUpper(c) : c);

            int v = ((coords.Item1 + 1) * 10) + (coords.Item2 + 1);

            return v > 0 ? v : 0;
        }

        static int[] EncodePolybius(string plainText, string keyword, bool useKey)
        {
            
            int[] cipherCode = new int[plainText.Length];

            int h, i = 0, j = 0, k;
            while (i < plainText.Length)
            {
                h = FindPolybiusCoords(plainText[i]);

                if (h > 0)
                {
                    k = useKey ? FindPolybiusCoords(keyword[j % keyword.Length]) : 0;
                    cipherCode[i] = h + k;
                    
                    j++;
                }
                else { cipherCode[i] = 0; }

                i++; 
            }

            return cipherCode;
        }

        public static string EncodePolybiusToString(string plainText, string keyword, bool useKey) {

            int[] cipherCode = EncodePolybius(plainText, keyword, useKey);

            StringBuilder cipherText = new();
            foreach (int code in cipherCode)
            {
                cipherText.AppendFormat("{0} ", code.ToString());
            }
            return cipherText.ToString();
        }

        public static string DecodePolybiusFromString(string cipherText, string keyword, bool useKey)
        {
            int[] polycoords = cipherText.Split(' ').Select(x => string.IsNullOrEmpty(x) ? 0 : int.Parse(x) ).ToArray();
            
            StringBuilder plainText = new();

            int h, j, x, y;
            for (int i = 0; i < polycoords.Length; i++)
            {
                j = polycoords[i];
                if (polycoords[i] == 0) { plainText.Append(' '); continue; }
                h = useKey ? FindPolybiusCoords(keyword[i % keyword.Length]) : 0;
                x = ((j - h) % 10) - 1;
                y = ((j - h) / 10) - 1;

                plainText.Append( Polybius[y,x] );
            }

            return plainText.ToString();
        }



        public static void PolybiusSquareShift (string keyword, bool useCustomSquare)
        {
            ResetPolybiusSquare(useCustomSquare);
            //use a keyword  to shift the square's rows and columns according to a keyword
            int[] shiftAmounts = EncodePolybius(keyword, "nokeyhere", false);
            for (int k = 0; k < shiftAmounts.Length; k++)
            {
                //alternate between shifting rows and shifting columns
                Polybius.MatrixRowShift(k, shiftAmounts[k], (k%2 == 0));
            }
            return;
        }
        #endregion

        #region substitution cipher/vigenere cipher code/chaocipher
        //for various purposes
        public static string defaultAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        
        //for the chaocipher
        private static string LeftAlphabet = "HXUCZVAMDSLKPEFJRIGTWOBNYQ";
        private static string RightAlphabet = "PTLNBQDEOYSFAVZKGJRIHWXUMC";


        static int getShiftAmount(char c) // gets character index
        {
            if (!char.IsLetter(c)) { return 0; }
            char offset = char.IsLower(c) ? 'a' : 'A';
            return MiscMethods.Mod(c, -offset, 26); 
        }

        static char EncipherChar (char c, char i, bool decrypt)
        {
            if (!char.IsLetter(c)) { return c; }

            int shift = decrypt ? -getShiftAmount(i) : getShiftAmount(i);
            char offset = char.IsLower(c) ? 'a' : 'A';
            
            return (char) ( offset + MiscMethods.Mod(c - offset, shift, 26) );
        }

        public static string ScrambleString (string inputString)
        {
            if (string.IsNullOrEmpty(inputString)) { throw new Exception("Cannot input an empty string"); }
            int l = inputString.Length;
            char[] scramble = inputString.ToCharArray();

            scramble.Shuffle();
            string o = new string(scramble);

            if (string.IsNullOrEmpty(o)) { throw new Exception("What happened here?"); }
            return o;
        }

        public static string RandomCipherAlphabet() => ScrambleString(defaultAlphabet);

        public static string CaesarShift(string plainText, char shift, bool decrypt) {
            StringBuilder cipherText = new(plainText.Length);
            foreach (char c in plainText)
            {
                cipherText.Append( EncipherChar(c, shift, decrypt) );
            }
            return cipherText.ToString();
        }

        public static string VigenereCipher(string plainText, string key, bool decrypt)
        {
            StringBuilder cipherText = new(plainText.Length);
            int plainTextWithoutSpaces = plainText.Trim().Length;

            int i = 0; int j = 0;
            while (i < plainText.Length) { 
                if (!char.IsLetter(plainText[i])) { cipherText.Append(plainText[i]); i++; continue; }
                char keyChar = key[j % key.Length];
                cipherText.Append( EncipherChar(plainText[i], keyChar, decrypt) );
                i++; j++;
            }
            return cipherText.ToString();
        }

        public static string VigenereCipherMultipleKeywords(string plainText, string[] keys, bool decrypt)
        {
            string cipherText = plainText;
            foreach (string key in keys)
            {
                cipherText = VigenereCipher(cipherText, key, decrypt);
            }
            return cipherText;
        }

        public static string AutokeyCipher(string plainText, string primer, bool decrypt) => VigenereCipher(plainText, primer + plainText, decrypt);

        public static string SubstitutionCipher(string plainText, string CipherAlphabet, bool encrypt) {
            //assumes that plainText uses ABCDEFGHIJKLMNOPQRSTUVWXYZ
            if (CipherAlphabet.Length < 26) {
                throw new Exception("Cipher alphabet is too small");
            }
            if (CipherAlphabet.Length > 26)
            {
                throw new Exception("Please only define the 26 letters of the alphabet");
            }
            StringBuilder cipherText = new(plainText.Length);
            char _c;
            int index;

            foreach (char c in plainText) {
                _c = c;
                if (char.IsLetter(c))
                {
                    if (encrypt)
                    {
                        index = defaultAlphabet.IndexOf(char.ToUpper(c));
                        _c = CipherAlphabet[index];
                    }
                    else
                    {
                        index = CipherAlphabet.IndexOf(char.ToUpper(c));
                        _c = defaultAlphabet[index];
                    }
                    if (char.IsLower(c)) { char.ToLower(_c); }
                    if (char.IsUpper(c)) { char.ToUpper(_c); }
                }
                cipherText.Append(c);
            }

            return cipherText.ToString();
        }

        public static string Chaocipher(string plainText, bool decrypt) {
            int len = plainText.Length;
            char[] left = LeftAlphabet.ToCharArray();
            char[] right = RightAlphabet.ToCharArray();
            char[] text = new char[len];
            char[] temp = new char[26];

            int index;
            char curr;
            for (int i = 0; i < len; i++)
            {
                curr = plainText[i];
                if (!char.IsLetter(curr)) { text[i] = curr; continue; }

                if (!decrypt)
                {   
                    index = Array.IndexOf(right, char.ToUpper(curr));
                    text[i] = char.IsLower(curr) ? char.ToLower( left[index]) : left[index];
                } else
                {
                    index = Array.IndexOf(left, char.ToUpper(curr));
                    text[i] = char.IsLower(curr) ? char.ToLower(right[index]) : right[index];
                }
                //permute left;
                for (int j = index; j < 26; ++j) temp[j - index] = left[j];
                for (int j = 0; j < index; ++j) temp[26 - index + j] = left[j];
                var store = temp[1];
                for (int j = 2; j < 14; ++j) temp[j - 1] = temp[j];
                temp[13] = store;
                temp.CopyTo(left, 0);
                //permute right;
                for (int j = index; j < 26; ++j) temp[j - index] = right[j];
                for (int j = 0; j < index; ++j) temp[26 - index + j] = right[j];
                store = temp[0];
                for (int j = 1; j < 26; ++j) temp[j - 1] = temp[j];
                temp[25] = store;
                store = temp[2];
                for (int j = 3; j < 14; ++j) temp[j - 1] = temp[j];
                temp[13] = store;
                temp.CopyTo(right, 0);

            }


            return new string(text);
        }
        #endregion


    }
}
