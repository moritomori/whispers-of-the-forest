EXTERNAL set_flag(string, bool)
EXTERNAL add_harmony(int)
EXTERNAL add_ruthlessness(int)
EXTERNAL recalc_world()
EXTERNAL get_world_state()

=== start ===
~ set_flag("TalkedToElder", true)
~ temp ws = get_world_state()

{
- ws == "ForestAngered":
    -> angry_world
- ws == "ForestAwakening":
    -> awakened_world
- else:
    -> neutral_world
}

=== neutral_world ===
#speaker:elder
Stranger… the forest has been quiet lately. Too quiet.
#speaker:elder
What would you advise?

+ [Calm people]
    #speaker:player
    Don’t panic. No axes, no guards. Let’s keep peace.
    ~ add_harmony(1)
    #speaker:elder
    Peace is fragile… but I’ll try.
    -> END

+ [Arm guards]
    #speaker:player
    Prepare the guards. If something moves in the woods, we strike first.
    ~ add_ruthlessness(1)
    #speaker:elder
    Harsh… but perhaps necessary.
    -> END

+ [Just leave]
    #speaker:player
    I should go.
    -> END

=== awakened_world ===
#speaker:elder
People say the wind carries voices now.
#speaker:elder
If the forest is waking… we must choose how to live beside it.

+ [Negotiate]
    #speaker:player
    We negotiate. We offer respect and stop provoking the woods.
    ~ add_harmony(2)
    #speaker:elder
    Then we must be brave in a different way.
    -> END

+ [Control it]
    #speaker:player
    We control it. If it wakes, we cage it.
    ~ add_ruthlessness(1)
    #speaker:elder
    Control often costs more than people expect.
    -> END

+ [Just leave]
    #speaker:player
    I should go.
    -> END

=== angry_world ===
#speaker:elder
The night feels heavier. Even the dogs won’t bark.
This is how tragedies begin.

+ [Repair damage]
    #speaker:player
    Stop cutting. We try to repair what we broke.
    ~ add_harmony(1)
    #speaker:elder
    If it’s still possible… do it.
    -> END

+ [Arm everyone]
    #speaker:player
    Arm everyone. We survive by force.
    ~ add_ruthlessness(2)
    #speaker:elder
    Then blood may answer blood.
    -> END

+ [Just leave]
    #speaker:player
    I should go.
    -> END