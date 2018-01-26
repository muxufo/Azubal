using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Manager : NetworkManager
{
    /*
    private int seed;
    private NetworkClient myClient;

    // Classe custom de message
    public class SeedMessage : MessageBase
    {
        public int seed;
    };

    public class BombeMessage : MessageBase 
    {
        public float x;
        public float z;
        public TYPE_BOMBE_PICKUP typeBombe;
        public int range;
        public float explosionDelay;
    };

    // Classe custom pour les types de message
    public class MyMsgType
    {
        public static short Seed = MsgType.Highest + 1;
        public static short Bombe = MsgType.Highest + 2;
    };

    // Quand le joueur se connecte
    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Je suis connecté");
    }

    // Quand le joueur reçoit un seed du host
    public void OnSeed(NetworkMessage netMsg)
    {
        SeedMessage msg = netMsg.ReadMessage<SeedMessage>();
        Debug.Log("Réception de la seed : " + msg.seed);
        this.seed = msg.seed;

        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().genererTerrain(seed);
    }

    public void OnBombe(NetworkMessage netMsg) 
    {
        BombeMessage msg = netMsg.ReadMessage<BombeMessage>();

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SpawnBomb(msg.x, msg.z, msg.range, msg.typeBombe);
    }

    // Quand le joueur commence à host une partie
    public override void OnStartHost()
    {
        seed = (int)System.DateTime.Now.Ticks;
        Debug.Log("Start server ! " + seed);
        NetworkServer.RegisterHandler(MyMsgType.Bombe, OnBombe);

        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().genererTerrain(seed);
    }

    // Quand un joueur se connecte, cette fonction est déclenchée sur le host
    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("Un client s'est connecté !" + conn.connectionId);
        Debug.Log("Envoi de la seed ! " + seed);
        SeedMessage msg = new SeedMessage();
        msg.seed = this.seed;
        NetworkServer.SendToClient(conn.connectionId, MyMsgType.Seed, msg);
    }

    // Quand un client se connecte au host, cette fonction se déclenche sur le client
    public override void OnClientConnect(NetworkConnection conn)
    {
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.RegisterHandler(MyMsgType.Seed, OnSeed);
        myClient.Connect(this.networkAddress, this.networkPort);

        Debug.Log("Tentative de connexion sur " + this.networkAddress);
    }

    public int getSeed() { return seed; }

    public void EnvoyerMessageBombe(Bombe bombe) 
    {
        BombeMessage msg = new BombeMessage();
        msg.x = bombe.transform.position.x;
        msg.z = bombe.transform.position.z;
        switch (bombe.GetType().ToString()) {
            case "Bombe":
                msg.typeBombe = TYPE_BOMBE_PICKUP.BOMBE;
                break;
            case "SuperBombe":
                msg.typeBombe = TYPE_BOMBE_PICKUP.SUPER_BOMBE;
                break;
            case "BombeMur":
                msg.typeBombe = TYPE_BOMBE_PICKUP.BOMBE_MUR;
                break;
            case "BombeGlace":
                msg.typeBombe = TYPE_BOMBE_PICKUP.BOMBE_GLACE;
                break;
        }
        msg.range = bombe.range;
        msg.explosionDelay = Bombe.EXPLOSION_DELAY;
        
        myClient.Send(MyMsgType.Bombe, msg);
    }
    */
}
