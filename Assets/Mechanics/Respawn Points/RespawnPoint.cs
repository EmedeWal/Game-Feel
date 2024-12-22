using ShatterStep.Player;

namespace ShatterStep
{
    public class RespawnPoint : PlayerTrigger
    {
        protected override void OnPlayerEntered(Manager player)
        {
            RespawnSystem.Instance.RegisterRespawnPoint(this);
        }
    }
}