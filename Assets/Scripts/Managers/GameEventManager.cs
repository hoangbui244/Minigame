using System;
using UnityEngine;

public static class GameEventManager
{
    public static Action<bool> SoundStateChanged;
    public static Action<bool> MusicStateChanged;
    public static Action<bool> VibraStateChanged;
    public static Action UpdateTicket;
    public static Action PurchaseAds;
    public static Action<int> FlipCard;
    public static Action<bool> Check;
    public static Action<int> LoadLevel;
    public static Action ResetLevel;
    public static Action FruitCutting;
    public static Action<bool> BreakCandy;
    public static Action<bool> DefuseBomb;
    public static Action<bool> BallBreaker;
}
