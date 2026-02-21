EXTERNAL set_flag(string, bool)
EXTERNAL recalc_world()
EXTERNAL get_world_state()

=== start ===
~ temp ws = get_world_state()

{ ws == "ForestAngered":
    -> angry_dialogue
    ws == "ForestAwakening":
    -> friendly_dialogue
- else:
    -> first_meeting
}

=== first_meeting ===
#speaker:spirit
You feel the forest breathing.

+ I want to help
    ~ set_flag("HelpedSpirit", true)
    ~ recalc_world()
    -> END

+ I don't care
    -> END


=== friendly_dialogue ===
#speaker:spirit
You chose wisely before.
-> END


=== angry_dialogue ===
#speaker:spirit
The forest suffers because of you.
-> END
