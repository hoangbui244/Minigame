using System;
using UnityEngine;

public static class GameEventManager
{
    public static Action<bool> SoundStateChanged;
    public static Action<bool> MusicStateChanged;
    public static Action<bool> VibraStateChanged;
    public static Action PurchaseAds;
    public static Action<int> FlipCard;
    public static Action<bool> Check;
    public static Action<int> LoadLevel;
    public static Action ResetLevel;
    public static Action FruitCutting;
    public static Action<bool> BreakCandy;
    public static Action<bool> DefuseBomb;
    public static Action BallBreaker;
    public static Action<bool> CheckTeeth;
    public static Action FindDifference;
    public static Action<int> CutInHalf;
    public static Action<Vector3> ThrowBall;
    public static Action BreakCandyPiece;
    public static Action<float> Test;
    public static Action<int> PerfectSlices;
    public static Action PerfectSlicesReset;
    public static Action<int> NextBomb;
    public static Action PassBomb;
    public static Action<int> CatchEgg;
    public static Action<int> PencilTap;
    public static Action<int> DropCandy;
    public static Action BalanceEgg;
    public static Action<int> PetalCount;
    public static Action<bool, int> MuteOther;
    public static Action<int> UnselectCharacter;
    public static Action Play;
}
