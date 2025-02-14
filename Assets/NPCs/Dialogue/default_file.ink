INCLUDE globals.ink

-> greeting

== greeting ==
# portrait: npc_default # speaker: generic NPC # layout: left # audio: aris1
"Greetings traveler."
+ [Ask about location] 
    "This is a place of business." 
    -> END
+ [Request help]
    "I'm afraid I can't help with that right now."
    -> END
+ [Leave]
    "Safe journeys." 
    -> END

== idle ==
# portrait: npc_default # speaker: Quest Giver 1 # layout: left # audio: aris1
"Is there something else you needed?"
-> greeting

== farewell ==  
# portrait: npc_default # speaker: Quest Giver 1 # layout: left # audio: aris1
"Come back anytime."
-> END