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
        Coin,
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

    public enum StatType
    {
        Coin,
        Key,
        Death,
        Time,
    }

    public enum AudioType
    {
        Music,
        Sound,
    }

    public enum GameState
    {
        Gameplay,
        Cutscene,
        Paused,
    }
}