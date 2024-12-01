using ShatterStep.Player;

namespace ShatterStep
{
    public class Spike : PlayerTrigger
    {
        private RespawnSystem _respawnSystem;

        protected override void Initialize()
        {
            base.Initialize();

            _respawnSystem = RespawnSystem.Instance;
        }

        protected override void OnPlayerEntered(Manager player)
        {
            _respawnSystem.RespawnPlayer(player.Data);
        }
    }

}