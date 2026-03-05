EXTERNAL set_flag(string, bool)
EXTERNAL add_harmony(int)
EXTERNAL add_ruthlessness(int)
EXTERNAL recalc_world()
EXTERNAL get_world_state()

=== start ===
~ set_flag("TalkedToWanderer", true)
~ temp ws = get_world_state()

{
- ws == "ForestAngered":
    -> power_path
- ws == "ForestAwakening":
    -> disappointed
- else:
    -> first_meeting
}

=== first_meeting ===
#speaker:wanderer
A quiet forest is a poor forest. People need wood. Tools. Fire.

+ [Cut the tree]
    #speaker:player
    I’ll cut the sacred tree. We need a fast solution.
    ~ set_flag("HarmedForest", true)
    ~ add_ruthlessness(2)
    ~ recalc_world()
    #speaker:wanderer
    Then do it quickly. The village will follow strength.
    -> END

+ [Find another way]
    #speaker:player
    No. We’ll find another way — slower, but safer.
    ~ add_harmony(1)
    #speaker:wanderer
    Safer… until winter comes.
    -> END

+ [Just leave]
    #speaker:player
    I should go.
    -> END

=== power_path ===
#speaker:wanderer
See? The forest only understands force.
#speaker:wanderer
Now the elder will beg for protection. You’ll have leverage.
-> END

=== disappointed ===
#speaker:wanderer
So you chose whispers over work.
#speaker:wanderer
Fine. But when the village freezes, don’t come to me.
-> END