using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumConst
{
    public enum BulletDirection
    {
        Left,
        Right,
        Up,
        Down
    }
    public enum GameState
    {
        MainMenu,
        Init,
        InGame,
        Paused,
        EndGame,
    }
    public enum CurrencyType
    {
        Gold,
    }

}
