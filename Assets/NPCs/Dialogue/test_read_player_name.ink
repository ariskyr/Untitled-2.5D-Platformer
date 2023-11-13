INCLUDE globals.ink

{ player_name == "": -> no_name | -> greet }

=== no_name ===
Who de fuck are you? #layout: right
-> END


=== greet ===
sup bitch boy link #speaker: {player_name} #portrait: link_default #layout: left
-> END