INCLUDE globals.ink

EXTERNAL triggerQuest()

{ CollectCoins:
 -"CAN_START": -> can_start_quest
 -"IN_PROGRESS": -> in_progress
 -"CAN_FINISH": -> can_finish_quest
 -"FINISHED": -> finished_quest
 }

==can_start_quest==
Hey #portrait: npc_default #speaker: Thomas #layout: left #audio: aris1
I seem to have misplaced some of my collectable <color=\#5B81FF>coins</color>.
Can you please help me find them?
+ [yes]
    ~triggerQuest()
    Thank you! There should be 5 in total. #portrait: npc_default
    -> END
+ [no]
    That's a shame, have a good day.
    -> END

==in_progress==
I see you still haven't gathered them, come back when you have all 5. #speaker: Thomas #layout: left #audio: aris1
-> END

==can_finish_quest==
Well done! #portrait: npc_default #speaker: Thomas #layout: left #audio: aris1
Now give them to me.
+ [yes]
    ~triggerQuest()
    thank you!
    -> END
+ [no]
    They are very important to me!
    -> END

==finished_quest==
Since you helped me with those coins, I put in a word in to the villager leader. #portrait: npc_default #speaker: Thomas #layout: left #audio: aris1
Go speak to him.
-> END