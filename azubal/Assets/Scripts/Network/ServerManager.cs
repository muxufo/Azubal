using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ServerManager : NetworkManager
{
    private int seed;

    // Quand le joueur commence à host une partie
    public override void OnStartHost()
    {
        this.seed = (int)System.DateTime.Now.Ticks;
        Debug.Log("Start server ! " + this.seed);
        NetworkServer.RegisterHandler(MyMsgType.Bomb, OnClientSpawnedBomb);
        NetworkServer.RegisterHandler(MyMsgType.Detonateur, OnClientDetonateur);

        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().genererTerrain(seed);
    }

    // Quand un joueur se connecte, cette fonction est déclenchée sur le host
    public override void OnServerConnect(NetworkConnection conn)
    {
        Debug.Log("Un client s'est connecté !" + conn.connectionId);
        Debug.Log("Envoi de la seed ! " + this.seed);
        SeedMessage msg = new SeedMessage();
        msg.seed = this.seed;
        msg.clientId = NetworkServer.connections.Count;
        NetworkServer.SendToClient(conn.connectionId, MyMsgType.Seed, msg);
    }
    
    private void OnClientSpawnedBomb(NetworkMessage netMsg) {
        BombeMessage msg = netMsg.ReadMessage<BombeMessage>();
        NetworkServer.SendToAll(MyMsgType.Bomb, msg);
    }

    private void OnClientDetonateur(NetworkMessage netMsg) {
        DetonateurMessage msg = netMsg.ReadMessage<DetonateurMessage>();
        NetworkServer.SendToAll(MyMsgType.Detonateur, msg);
    }






    /*****************
     *  CODE CLIENT
     ****************/
    private int clientId;

    // Quand un client se connecte au host, cette fonction se déclenche sur le client
    public override void OnClientConnect(NetworkConnection conn) {
        this.client.RegisterHandler(MyMsgType.Seed, OnSeed);
        this.client.RegisterHandler(MyMsgType.Bomb, OnBombSpawned);
        this.client.RegisterHandler(MyMsgType.Detonateur, OnDetonateur);

        Debug.Log("Je suis connecté");
    }

    // Quand le joueur reçoit un seed du host
    public void OnSeed(NetworkMessage netMsg) {
        SeedMessage msg = netMsg.ReadMessage<SeedMessage>();
        Debug.Log("Réception de la seed: " + msg.seed);
        Debug.Log("clientId assigné: "+ msg.clientId);
        this.seed = msg.seed;
        this.clientId = msg.clientId;

        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().genererTerrain(seed);
    }

    private void OnBombSpawned(NetworkMessage netMsg) {
        BombeMessage msg = netMsg.ReadMessage<BombeMessage>();

        if (msg.clientId != this.clientId)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().SpawnBomb(msg.x, msg.z, msg.range, msg.typeBombe, msg.id);
        }
    }

    private void OnDetonateur(NetworkMessage netMsg) {
        DetonateurMessage msg = netMsg.ReadMessage<DetonateurMessage>();

        if (msg.clientId != this.clientId) {
            Bombe[] listeBombe = GameObject.FindObjectsOfType<Bombe>();

            foreach (Bombe bombe in listeBombe) {
                if (bombe.id == msg.bombId) {
                    bombe.Explode();
                }
            }
        }
    }
    


    public void EnvoyerMessageBombe(Bombe bomb)
    {
        BombeMessage msg = new BombeMessage();

        msg.id = bomb.id;
        msg.x = bomb.transform.position.x;
        msg.z = bomb.transform.position.z;
        switch (bomb.GetType().ToString())
        {
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

        msg.range = bomb.range;
        msg.explosionDelay = Bombe.EXPLOSION_DELAY;
        msg.clientId = this.clientId;
        
        this.client.Send(MyMsgType.Bomb, msg);
    }

    public void EnvoyerMessageDetonateur(float bombId) {
        DetonateurMessage msg = new DetonateurMessage();
        msg.clientId = this.clientId;
        msg.bombId = bombId;

        this.client.Send(MyMsgType.Detonateur, msg);
    }
}
