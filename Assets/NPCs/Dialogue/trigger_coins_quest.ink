INCLUDE globals.ink

EXTERNAL triggerQuest()

{ CollectCoinsQuest:
 -"CAN_START": -> can_start_quest
 -"IN_PROGRESS": -> in_progress
 -"CAN_FINISH": -> can_finish_quest
 -"FINISHED": -> finished_quest
 }

==can_start_quest==
Hey #speaker: Liolios #portrait: liolios_default #layout: left
Thes <color=\#5B81FF>quest</color>?
+ [ne]
    ~triggerQuest()
    trava mazepse 5 coins #portrait: liolios_ee
    -> END
+ [oxi]
    bye
    -> END

==in_progress==
akoma na ta mazepseis? #speaker: Liolios #portrait: liolios_default #layout: left
-> END

==can_finish_quest==
bravo re maga #speaker: Liolios #portrait: liolios_default #layout: left
dwwsta twra edw
+ [ne]
    ~triggerQuest()
    euxaristw Poly
    -> END
+ [no]
    re fyge apo dw
    -> END

==finished_quest==
trava vres allou quest
-> END