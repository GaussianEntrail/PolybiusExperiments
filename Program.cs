using System;
using CipherThing;
//Use command line parameters
//1 - path of input file
//2 - path of output file
//3 - keyword for altering grid
//4 - keyword for enciphering text
//5 - path of custom polybius square
string[] getArgs = Environment.GetCommandLineArgs();
string gridAlterKeyword = "", cipherKeyword = "", pathCustomSquare = "";
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

if (doGridAltering && !String.IsNullOrEmpty(gridAlterKeyword)) 
{ 
    Ciphers.PolybiusSquareShift(gridAlterKeyword, useCustomSquare); 
}
try
{
    using (StreamReader I = new(getArgs[1]))
    {
        using (StreamWriter O = new(getArgs[2]))
        {
            string? line;
            while ((line = I.ReadLine()) != null)
            {
                O.WriteLine(Ciphers.EncodePolybiusToString(line, cipherKeyword, useKeyword));
            }
        }
    }
} catch (Exception ex) { Console.WriteLine(ex.Message); }
