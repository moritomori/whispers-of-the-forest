EXTERNAL set_flag(string, bool)
EXTERNAL recalc_world()
EXTERNAL get_world_state()

=== start ===
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
Power is within reach.

+ Cut the sacred tree
    ~ set_flag("HarmedForest", true)
    ~ recalc_world()
    -> END

+ Leave it
    -> END


=== power_path ===
#speaker:wanderer
See? Strength always wins.
-> END


=== disappointed ===
#speaker:wanderer
You chose weakness.
-> END
