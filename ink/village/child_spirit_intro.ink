EXTERNAL set_flag(string, bool)

=== start ===
~ set_flag("MetChildSpirit", true)

#speaker:child_spirit
Oh! Hello!

#speaker:child_spirit
Are you here to visit someone? Or are you a traveler?

#speaker:player
...

+ [Ask why she is alone.]
    #speaker:player
    Where is everyone? Why are you here all alone?

    #speaker:child_spirit
    Alone?

    #speaker:child_spirit
    What do you mean? Everyone is here.

    #speaker:child_spirit
    And you're talking to me, aren't you?

    #speaker:player
    ...Right.

    -> dog_request

+ [Pretend everything is normal.]
    #speaker:player
    Just passing through. I'm a researcher, actually.

    #speaker:child_spirit
    A researcher? That's interesting!

    #speaker:child_spirit
    Our village is small, but I'm sure you'll find someone to talk to.

    #speaker:player
    I'll keep that in mind.

    -> dog_request

+ [Say nothing.]
    #speaker:player
    ...

    #speaker:child_spirit
    You're not very talkative, are you?

    #speaker:child_spirit
    That's okay. You seem nice anyway.

    -> dog_request

=== dog_request ===
#speaker:child_spirit
Oh! I almost forgot!

#speaker:child_spirit
Have you seen Joan? She's my dog.

#speaker:child_spirit
She ran into the forest, and I haven't seen her since.

#speaker:child_spirit
I can't go there myself...

#speaker:child_spirit
Maybe you could look for her?

#speaker:player
I'll see what I can find.

~ set_flag("ChildAskedAboutDog", true)
-> END
