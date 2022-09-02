using System;
using CipherThing;
//Use command line parameters
//1 - path of input file
//2 - path of output file
//3 - keyword for altering grid
//4 - keyword for enciphering text
//5 - path of custom polybius square
string[] getArgs = Environment.GetCommandLineArgs();
string inputFilePath = getArgs[1], outputFilePath = getArgs[2], gridAlterKeyword = "", cipherKeyword = "", pathCustomSquare = "";
bool doGridAltering = false, useKeyword = false, useCustomSquare = false;

if (getArgs.Length > 3)
{
    if (!String.IsNullOrEmpty(getArgs[3]))
    {
        gridAlterKeyword = getArgs[3];
        doGridAltering = true;
    }
}
if (getArgs.Length > 4)
{
    if (!String.IsNullOrEmpty(getArgs[4]))
    {
        cipherKeyword = getArgs[4];
        useKeyword = true;
    }
}
if (getArgs.Length > 5)
{
    if (!String.IsNullOrEmpty(getArgs[5]))
    {
        pathCustomSquare = getArgs[5]; 
        useCustomSquare = true;
        
        Ciphers.MakeCustomSquare(pathCustomSquare);
    }
}

//Take the file at getArgs[1] and encipher it, putting the output in getArgs[2]
//Use the optional arguments in getArgs[3] and getArgs[4] to shuffle the polybius square or as a keyword
string[] I = System.IO.File.ReadAllLines(inputFilePath);
string[] O = new string[I.Length];

if (doGridAltering && !String.IsNullOrEmpty(gridAlterKeyword)) 
{ 
    Ciphers.PolybiusSquareShift(gridAlterKeyword, useCustomSquare); 
}

for (int k = 0; k < I.Length; k++)
{
    O[k] = CipherThing.Ciphers.EncodePolybiusToString(I[k], cipherKeyword, useKeyword);
}

System.IO.File.WriteAllLines(outputFilePath, O);

#region old demo code
//string plaintext = "Meet Me At The Fountain";
//string[] testkeys = { "LEMON", "ORANGE", "NAZARETH2" };
//string ciphertext, ciphertext2;

//Console.WriteLine("Polybius square cipher test!\nBefore Shuffling:\n");

//Console.WriteLine(Ciphers.Polybius.ToMatrixString());

//Console.WriteLine("Use keyword {0} to alter the grid", testkeys[2] );

//Ciphers.PolybiusSquareShift(testkeys[2]);

//Console.WriteLine(Ciphers.Polybius.ToMatrixString());

//ciphertext = CipherThing.Ciphers.EncodePolybiusToString(plaintext, " ", false);
//ciphertext2 = CipherThing.Ciphers.EncodePolybiusToString(plaintext, testkeys[0], true);

//Console.WriteLine("Polybius Square cipher test, using this shuffled matrix\n\nPLAINTEXT:\n\t{0}\nCIPHERTEXT:\n\t{1}\nCIPHERTEXT (USING KEYWORD {2}):\n\t{3}",plaintext,ciphertext,testkeys[0],ciphertext2);
#endregion
