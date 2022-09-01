# PolybiusExperiments
## Experimenting with Ciphers and Polybius Squares specificall
### Basic Cipher
With a basic Polybius cipher, you take a message, and map each character in the message to it's coordinates in a grid

For example, using the grid below
```
    1    2    3    4    5
  ┌───────────────────────┐
1 │ A    B    C    D    E |
2 | F    G    H    I/J  K |
3 | L    M    N    O    P |
4 | Q    R    S    T    U |
5 | V    W    X    Y    Z |
  └───────────────────────┘
```

The message
```
MEET ME AT THE FOUNTAIN
```
Becomes
```
32 15 15 44  32 15  11 44  44 23 15  21 34 43 33 44 11 24 33
```

### Modifications

#### Use of a keyword to encode the message

Using a keyword
```
LEMON
```
We encode it according to the grid above and get
```
31 15 32 34 33
```
We then add these to each of the "coordinates" in the cipher text as follows:
```
32  15  15  44    32  15    11  44    44  23  15    21  34  43  33  44  11  24  33
31  15  32  34    33  31    15  32    34  33  31    15  32  34  33  31  15  32  34
──  ──  ──  ──    ──  ──    ──  ──    ──  ──  ──    ──  ──  ──  ──  ──  ──  ──  ──
63  30  47  78    55  46    26  76    78  56  46    36  66  77  66  75  26  56  67
```

#### "Shuffling" the grid before enciphering

Scrambling the grid before using it to encipher messages. This can be done either with a random number generator or by using a keyword.

Using the keyword
```
ORANGE
```
With the unmodified square, we get
```
34 42 11 33 22 15
```
We use these to alternatingly shift each row and column by the specified amount- 
* the first row gets shifted 34 spaces (effectively a shift of 34%5 or 4)
* the second column gets shifted 42 spaces (effectively a shift of 2)
* and so on

These shifts result in the scrambled Square of

```
    1    2    3    4    5
  ┌───────────────────────┐
1 │ B    R    D    N    A |
2 | F    W    H    T    K |
3 | P    L    C    Y    O |
4 | Q    G    S    E    U |
5 | I/J  Z    V    M    X |
  └───────────────────────┘
```

With this square, the message
```
MEET ME AT THE FOUNTAIN
```
Becomes
```
54 44 44 24  54 44  15 24  24 23 44  21 35 45 14 24 15 51 14
```

We can combine this with the keyword method from the previous section

#### Using a larger grid

Instead of just using a 5x5 grid consisting of only the alphabet we could use a 6x6 grid comprising letters and digits:

```
    1    2    3    4    5    6
  ┌────────────────────────────┐
1 │ A    B    C    D    E    F |
2 | G    H    I    J    K    L |
3 | M    N    O    P    Q    R |
4 | S    T    U    V    W    X |
5 | Y    Z    0    1    2    3 |
6 | 4    5    6    7    8    9 |
  └────────────────────────────┘
```
Or an even larger grid featuring punctuation and other characters:

```
    1       2       3       4       5       6       7       8       9
  ┌───────────────────────────────────────────────────────────────────┐
1 │ A       B       C       D       E       F       G       H       I │
2 │ J       K       L       M       N       O       P       Q       R │
3 │ S       T       U       V       W       X       Y       Z       0 │
4 │ 1       2       3       4       5       6       7       8       9 │
5 │ `       ~       !       @       #       $       %       ^       & │
6 │ *       (       )       -       _       +       =       {       } │
7 │ [       ]       |       \       :       ;       '       "       < │
8 │ >       /       Æ       ÷       O       O       ß       E       Ä │
9 │ Ñ       Ö       Ü       D       ¢       £              I         │
  └───────────────────────────────────────────────────────────────────┘
```





