using Godot;
using GodotInk;
using WhispersOfTheForest.Core;
using WhispersOfTheForest.Dialogue;
using WhispersOfTheForest.Story;
using WhispersOfTheForest.World;

namespace WhispersOfTheForest.scripts.npc
{
    public partial class WoodcutterNpc : Area2D, IInteractable, ILocationContextConsumer
    {
        [Export] private InkStory? _sleepingDialogue;
        [Export] private InkStory? _awakeDialogue;

        private LocationContext? _locationContext; 

        public void SetLocationContext(LocationContext context)
        {
            _locationContext = context;
        }

        public void Interact(Player player)
        {
            if (_locationContext?.DialogueController is not DialogueController controller)
                return;

            if (controller.IsDialogueActive)
                return;

            StoryFlags? flags = _locationContext.StoryManager?.StoryFlags;

            if (flags is null)
            {
                GD.PushError("[WoodcutterNpc] StoryFlags not available.");
                return;
            }

            InkStory? dialogue = flags.WoodcutterAwakened
                ? _awakeDialogue
                : _sleepingDialogue;

            if (dialogue is null)
            {
                GD.PushError("[WoodcutterNpc] Dialogue not assigned.");
                return;
            }

            controller.StartDialogue(dialogue, "start");
        }
    }
}
