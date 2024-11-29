namespace ShatterStep
{
    public enum LocomotionState
    {
        Grounded,
        Bouncing,
        Jumping,
        Falling,
        Hanging,
        Dashing,
    }

    public enum CollectibleType
    {
        Heart,
        Key,
    }

    public enum DirectionType
    {
        None,
        Left,
        Right,
    }

    public enum ActionType
    {
        Jump,
        Dash,
        Spawn,
    }

    public enum GameState
    {
        Gameplay,
        Cutscene,
        Paused,
    }
}