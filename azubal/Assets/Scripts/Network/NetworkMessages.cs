using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class SeedMessage : MessageBase
{
    public int seed;
    public int clientId;
}

public class BombeMessage : MessageBase
{
    public float id;
    public float x;
    public float z;
    public TYPE_BOMBE_PICKUP typeBombe;
    public int range;
    public float explosionDelay;
    public int clientId;
}

public class DetonateurMessage : MessageBase 
{
    public int clientId;
    public float bombId;
}

public class TriggerSautMessage : MessageBase {
    public float x;
    public float z;
}

// Classe custom pour les types de message
public class MyMsgType
{
    public static short Seed = MsgType.Highest + 1;
    public static short Bomb = MsgType.Highest + 2;
    public static short Detonateur = MsgType.Highest + 3;
    public static short TriggerSaut = MsgType.Highest + 4;
}