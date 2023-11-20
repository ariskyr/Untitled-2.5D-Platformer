-> main

=== main ===
Choose your fighter!
    + [1 2]
        -> chosen("liolios")
    + [spatali oksygonou]
        -> chosen("bouziotios")
    + [re maga]
        -> chosen("paliniotis")

=== chosen(fighter) ===
You chose {fighter}!
    +[go agane?]
        -> main
    +[puss?]
        bye
        -> END