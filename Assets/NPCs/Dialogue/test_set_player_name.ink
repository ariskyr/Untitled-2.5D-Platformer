INCLUDE globals.ink

{ player_name == "": -> main | -> already_chose }

=== main ===
What's your name? #speaker: Npc 1 #portrait: npc_default #layout: left #audio: aris
    +[Link]
        -> chosen("Link")
    +[Zelda]
        -> chosen("Zelda")

=== chosen(name) ===
~ player_name = name
You are {player_name}!
-> END

=== already_chose ===
You will forever be known as {player_name}! #speaker: Npc 1 #portrait: npc_default #layout: right #audio: aris
-> END