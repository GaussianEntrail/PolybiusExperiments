using System;
using CipherThing;

//Use command line parameters
//1 - path of input file
//2 - path of output file
//3 - keyword for altering grid
//4 - keyword for enciphering text
//5 - decode mode
//6 - path of custom polybius square
string[] getArgs = Environment.GetCommandLineArgs();
string gridAlterKeyword = "", cipherKeyword = "", pathCustomSquare = "";
bool doGridAltering = false, useKeyword = false, useCustomSquare = false, decodeMode = false;

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
    decodeMode = (int.Parse( getArgs[5] ) == 1);
}
if (getArgs.Length > 6)
{
    if (!string.IsNullOrEmpty(getArgs[6]))
    {
        pathCustomSquare = getArgs[6]; 
        useCustomSquare = true;
        
        Ciphers.MakeCustomSquare(pathCustomSquare);
    }
}

//Take the file at getArgs[1] and encipher it, putting the output in getArgs[2]
//Use the optional arguments in getArgs[3] and getArgs[4] to shuffle the polybius square or as a keyword
try
{
    if (doGridAltering && !String.IsNullOrEmpty(gridAlterKeyword))
    {
        Ciphers.PolybiusSquareShift(gridAlterKeyword, useCustomSquare);
    }


    using (StreamReader I = new(getArgs[1]))
    {
        using (StreamWriter O = new(getArgs[2]))
        {
            string? line;
            string ciphered;
            while ((line = I.ReadLine()) != null)
            {
                ciphered = decodeMode ? Ciphers.DecodePolybiusFromString(line, cipherKeyword, useKeyword) : Ciphers.EncodePolybiusToString(line, cipherKeyword, useKeyword);
                O.WriteLine(ciphered);
            }
        }
    }

} catch (Exception ex) { Console.WriteLine(ex.Message); }
