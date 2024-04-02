INCLUDE globals.ink

EXTERNAL triggerQuest()

{ VisitBoxesQuest:
-"CAN_START": -> can_start_quest
-"IN_PROGRESS": -> in_progress
-"CAN_FINISH": -> can_finish_quest
-"FINISHED": -> finished_quest
}

==can_start_quest==
Twra pou ekanes to prwto quest, thes kiallo?
+ [ne]
    ~triggerQuest()
    pigaine episkepsou ta 3 koutia me ti swsti seira
    -> END
+ [oxi]
    krima
    -> END

==in_progress==
eisai sigouros oti ta episkeftikes ola me ti swsti seira?
-> END

==can_finish_quest==
~triggerQuest()
bravo
-> END

==finished_quest==
paei to quest mou
-> END
