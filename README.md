# Fries-MinMax

Rules:
- Both players start with <1, 1>
- Goal is to get to <0, 0>
- On your turn you can attack or split
- Attacking adds the hand you chose to attack with to the hand you attacked and takes modulo 10. IE <5, 1> + <9,> = <4, 1>
- Splitting shifts numbers among your hand. IE <1, 1> -> <2, 0>
- You can also split to win: <9, 1> -> <0, 0>, however, non-winning splits have to maintain their equality: <1, 0> -\\> <9, 2>
- You cannot swap numbers: <1, 0> -\\> <0, 1>

Each command follows a 3 part structure, (or ([as])(?:([lr])([lr])|([0-9]+)\-([0-9]+)) for precision ):
- Type: a for attacking and s for splitting
- If attacking, l for your left hand and r for the other, then l or r for the opponents hand
- If splitting, the two numbers you're splitting into, separated by a dash

Example:

<1, 1> <br>
<1, 1> <br>
Player> all <br>
<2, 1> <br>
<1, 1> <br> 
AI> s3-0 <br>
<3, 0> <br>
<1, 1> <br>
