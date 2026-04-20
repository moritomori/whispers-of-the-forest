EXTERNAL set_flag(string, bool)
EXTERNAL add_ruthlessness(int)

=== start ===
~ set_flag("MetWoodcutter", true)
~ set_flag("SpokeToWoodcutterFirst", true)
~ add_ruthlessness(1)

#speaker:woodcutter
You are not from here.

#speaker:player
No. And I can already tell something is wrong with this village.

#speaker:woodcutter
Wrong?

#speaker:woodcutter
No. It is simply unfinished.

#speaker:woodcutter
This place should have faded long ago.

#speaker:player
What does that mean?

#speaker:woodcutter
It means something here refused to end.

#speaker:woodcutter
And that is rarely a blessing.
-> END