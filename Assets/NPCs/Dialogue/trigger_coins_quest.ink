INCLUDE globals.ink

EXTERNAL triggerQuest()

{ CollectCoinsQuest:
 -"CAN_START": -> can_start_quest
 -"IN_PROGRESS": -> in_progress
 -"CAN_FINISH": -> can_finish_quest
 -"FINISHED": -> finished_quest
 }

==can_start_quest==
Hey #speaker: Quest Giver 1 #layout: left #audio: aris1
Thes <color=\#5B81FF>quest</color>?
+ [ne]
    ~triggerQuest()
    trava mazepse 5 coins #portrait: npc_default
    -> END
+ [oxi]
    bye
    -> END

==in_progress==
akoma na ta mazepseis? #speaker: Quest Giver 1 #layout: left #audio: aris1
-> END

==can_finish_quest==
bravo re #speaker: Quest Giver 1 #layout: left #audio: aris1
dwwsta twra edw
+ [ne]
    ~triggerQuest()
    euxaristw Poly
    -> END
+ [no]
    fyge apo dw
    -> END

==finished_quest==
enas filos mou eipe oti exei ena quest gia sena #speaker: Quest Giver 1 #layout: left #audio: aris1
-> END