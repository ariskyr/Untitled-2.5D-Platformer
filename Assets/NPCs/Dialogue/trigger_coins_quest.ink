INCLUDE globals.ink

EXTERNAL triggerQuest()

{ CollectCoinsQuest:
 -"CAN_START": -> can_start_quest
 -"IN_PROGRESS": -> in_progress
 -"CAN_FINISH": -> can_finish_quest
 -"FINISHED": -> finished_quest
 }

==can_start_quest==
Hey #portrait: npc_default #speaker: Quest Giver 1 #layout: left #audio: aris1
Do you want a <color=\#5B81FF>quest</color>?
+ [yes]
    ~triggerQuest()
    Fetch me 5 coins! #portrait: npc_default
    -> END
+ [no]
    bye
    -> END

==in_progress==
I see you still haven't gathered them, come back when you have all 5. #speaker: Quest Giver 1 #layout: left #audio: aris1
-> END

==can_finish_quest==
Well done! #speaker: Quest Giver 1 #layout: left #audio: aris1
Now give them to me.
+ [yes]
    ~triggerQuest()
    thank you!
    -> END
+ [no]
    go away
    -> END

==finished_quest==
A friend of mine told me that he has a quest for you #speaker: Quest Giver 1 #layout: left #audio: aris1
-> END