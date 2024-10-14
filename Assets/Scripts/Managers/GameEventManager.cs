using System;

public static class GameEventManager
{
    public static Action<bool> SoundStateChanged;
    public static Action<bool> MusicStateChanged;
    public static Action<bool> VibraStateChanged;
    public static Action UpdateTicket;
    public static Action PurchaseAds;
    public static Action<int> FlipCard;
    public static Action<bool> Check;
}
