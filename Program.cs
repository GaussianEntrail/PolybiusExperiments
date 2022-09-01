using CipherThing;

string plaintext = "Meet Me At The Fountain";
string[] testkeys = { "LEMON", "ORANGE", "NAZARETH2" };
string ciphertext, ciphertext2;


Console.WriteLine("Polybius square cipher test!\nBefore Shuffling:\n");

Console.WriteLine(Ciphers.Polybius.ToMatrixString());

Console.WriteLine("Use keyword {0} to alter the grid", testkeys[2] );

Ciphers.PolybiusSquareShift(testkeys[2]);

Console.WriteLine(Ciphers.Polybius.ToMatrixString());

ciphertext = CipherThing.Ciphers.EncodePolybiusToString(plaintext, " ", false);
ciphertext2 = CipherThing.Ciphers.EncodePolybiusToString(plaintext, testkeys[0], true);

Console.WriteLine("Polybius Square cipher test, using this shuffled matrix\n\nPLAINTEXT:\n\t{0}\nCIPHERTEXT:\n\t{1}\nCIPHERTEXT (USING KEYWORD {2}):\n\t{3}",plaintext,ciphertext,testkeys[0],ciphertext2);
