using ShatterStep.Player;

namespace ShatterStep
{
    public class EventTrigger : PlayerTrigger
    {
        public delegate void PlayerEnteredDelegate(Manager player, EventTrigger trigger);
        public event PlayerEnteredDelegate PlayerEntered;

        protected override void OnPlayerEntered(Manager player)
        {
            PlayerEntered?.Invoke(player, this);
        }
    }
}