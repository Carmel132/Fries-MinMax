# Fries-MinMax

Rules:
- Both players start with <1, 1>
- Goal is to get to <0, 0>
- On your turn you can attack or split
- Attacking adds the hand you chose to attack with to the hand you attacked and takes modulo 10. IE <5, 1> + <9,> = <4, 1>
- Splitting shifts numbers among your hand. IE <1, 1> -> <2, 0>
- You can also split to win: <9, 1> -> <0, 0>, however, non-winning splits have to maintain their equality: <1, 0> -\\> <9, 2>
- You cannot swap numbers: <1, 0> -\\> <0, 1>
