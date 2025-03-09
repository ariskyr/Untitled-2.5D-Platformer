INCLUDE globals.ink

{ player_name == "": -> no_name | -> greet }

=== no_name ===
poios eisai? #layout: right
-> END


=== greet ===
sup {player_name}. #speaker: {player_name} #portrait: link_default #layout: left
-> END