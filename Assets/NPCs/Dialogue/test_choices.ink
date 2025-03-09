-> main

=== main ===
Choose your fighter!
    + [choice 1]
        -> chosen("Name 1")
    + [choice 2]
        -> chosen("Name 2")
    + [choice 3]
        -> chosen("Name 3")

=== chosen(fighter) ===
You chose {fighter}!
    +[pali?]
        -> main
    +[bye?]
        bye
        -> END