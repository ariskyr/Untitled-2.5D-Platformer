INCLUDE globals.ink

EXTERNAL triggerQuest()

-> start

== start ==
// First check AStolenArtifact quest status
{ 
    - AStolenArtifact == "REQUIREMENTS_NOT_MET": -> stolen_requirements_not_met
    - AStolenArtifact == "CAN_START": -> stolen_can_start
    - AStolenArtifact == "IN_PROGRESS": -> stolen_in_progress
    - AStolenArtifact == "CAN_FINISH": -> stolen_can_finish
    - AStolenArtifact == "FINISHED": -> stolen_finished
}

// If requirements are not met, check CollectCoins
== stolen_requirements_not_met ==
{
    - CollectCoins == "CAN_START": -> collect_coins_can_start
    - CollectCoins == "IN_PROGRESS": -> collect_coins_in_progress
    - CollectCoins == "CAN_FINISH": -> collect_coins_can_finish
    - CollectCoins == "FINISHED": -> collect_coins_finished
}

== collect_coins_can_start ==
#portrait: npc_default #speaker: Village Chief #layout: left #audio: aris1
"You should speak to Old Thomas first - he needs help with coins."
-> END

== collect_coins_in_progress ==
#portrait: npc_default #speaker: Village Chief #layout: left #audio: aris1
"Thomas still needs those coins collected. Hurry now!"
-> END

== collect_coins_can_finish ==
#portrait: npc_default #speaker: Village Chief #layout: left #audio: aris1
"Why are you here? Go give Thomas his coins!"
-> END

== collect_coins_finished ==
#portrait: npc_default #speaker: Village Chief #layout: left #audio: aris1
"Finally someone we can trust! Now we can discuss the real problem..."
~ AStolenArtifact = "CAN_START"
-> stolen_can_start

== stolen_can_start ==
#portrait: npc_default #speaker: Village Chief #layout: left #audio: aris1
"A knight stole our sacred artifact! Will you retrieve it?"

+ [Accept quest] 
    ~ AStolenArtifact = "IN_PROGRESS"
    ~ triggerQuest()
    "Find him north of the village!" 
    -> END
+ [Ask about knight]
    "He wears black armor and guards a chest fiercely."
    -> stolen_can_start

== stolen_in_progress ==
#portrait: npc_default #speaker: Village Chief #layout: left #audio: aris1
"The artifact must be in his chest!"
-> END

== stolen_can_finish ==
#portrait: npc_default #speaker: Village Chief #layout: left #audio: aris1
"You have the artifact! Let me bless it."

+ [Hand it over]
    ~ AStolenArtifact = "FINISHED"
    ~ triggerQuest()
    "Our people are saved!" 
    -> stolen_finished
+ [Inspect artifact]
    "The ancient runes glow faintly..."
    -> stolen_can_finish

== stolen_finished ==
#portrait: npc_default #speaker: Village Chief #layout: left #audio: aris1
"With this returned, our future is secure!"
-> END