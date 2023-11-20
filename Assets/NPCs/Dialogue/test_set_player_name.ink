INCLUDE globals.ink

{ player_name == "": -> main | -> already_chose }

=== main ===
What's your name?
    +[bigis pipis]
        -> chosen("bigis pipis")
    +[smool pipis]
        -> chosen("smool pipis")

=== chosen(name) ===
~ player_name = name
You are {player_name}!
-> END

=== already_chose ===
You will forever be known as {player_name}!
-> END