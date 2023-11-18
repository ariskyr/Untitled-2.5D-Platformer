INCLUDE globals.ink

{ player_name == "": -> no_name | -> greet }

=== no_name ===
poios nomizeis oti eisai? #layout: right
-> END


=== greet ===
sup bitch boy {player_name}. #speaker: {player_name} #portrait: link_default #layout: left
-> END