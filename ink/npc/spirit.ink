EXTERNAL set_flag(string, bool)
EXTERNAL add_harmony(int)
EXTERNAL add_ruthlessness(int)
EXTERNAL recalc_world()
EXTERNAL get_world_state()

=== start ===
~ set_flag("TalkedToSpirit", true)
~ temp ws = get_world_state()

{
- ws == "ForestAngered":
    -> angry_dialogue
- ws == "ForestAwakening":
    -> after_help
- else:
    -> first_meeting
}

=== first_meeting ===
#speaker:spirit
The forest remembers every step. Not with anger… with truth.

+ [I will help]
    #speaker:player
    I’ll help the forest. Tell me what you need.
    ~ set_flag("HelpedSpirit", true)
    ~ add_harmony(2)
    ~ recalc_world()
    #speaker:spirit
    Then listen: speak to the elder… and the wanderer. Return when you’re ready.
    -> END

+ [Stay out of it]
    #speaker:player
    I won’t get involved.
    ~ set_flag("IgnoredSpirit", true)
    #speaker:spirit
    Then the forest will judge you by what you do… not what you say.
    -> END

+ [Just leave]
    #speaker:player
    I should go.
    -> END

=== after_help ===
#speaker:spirit
Your choice has softened the roots. The forest is waking.
-> END

=== angry_dialogue ===
#speaker:spirit
The bark is wounded. The forest is not your tool.
-> END