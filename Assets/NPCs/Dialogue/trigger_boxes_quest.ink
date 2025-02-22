// INCLUDE globals.ink

// EXTERNAL triggerQuest()

// { VisitBoxesQuest:
// -"CAN_START": -> can_start_quest
// -"IN_PROGRESS": -> in_progress
// -"CAN_FINISH": -> can_finish_quest
// -"FINISHED": -> finished_quest
// }

// ==can_start_quest==
// Twra pou ekanes to prwto quest, thes kiallo? #audio: aris1
// + [ne]
//     ~triggerQuest()
//     pigaine episkepsou ta 3 koutia me ti swsti seira #audio: aris1
//     -> END
// + [oxi]
//     krima #audio: aris1
//     -> END

// ==in_progress==
// eisai sigouros oti ta episkeftikes ola me ti swsti seira? #audio: aris1
// -> END

// ==can_finish_quest==
// ~triggerQuest()
// bravo #audio: aris1
// -> END

// ==finished_quest==
// paei to quest mou #audio: aris1
// -> END
