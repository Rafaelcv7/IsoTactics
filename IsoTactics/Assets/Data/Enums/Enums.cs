namespace IsoTactics.Enums
{
    //DataTiles types that dictates the different configuration of each tile.
    public enum TileTypes
    {
        Traversable,
        NonTraversable,
        Effect,
    }

    public enum Stats
    {
    Strength,        //Affects damage dealt with PHYSICAL attacks.
    Vitality,        //Affects DEFENSE against attacks & Health pool.
    Accuracy,        //Affects CHANCES of landing successful attack.
    Agility,         //Affects CHANCES of avoiding a attack and the POSITION on the turn manager.
    Intelligence,    //Affects the strength of OFFENSIVE magic.
    Wisdom,          //Affects CHANCES of offensive and defensive magic & your resistance to it.
    Resistance,      //Affects global resistance to effects and elemental attacks.
    ActionPoints,    //How many actions can this character do per turn. 
    MovementPoints,  //How many spaces may this character move per turn.
    }
    
    public enum Operation
    {
        Add,
        Minus,
        Multiply,
        Divide,
        AddByPercentage,
        MinusByPercentage
    }

    //The behaviours that dictates the logic each Character Ai will follow.
    public enum Behaviours
    {
        CloseRange,
        LongRange,
        LongRangeExecutor
    }
}