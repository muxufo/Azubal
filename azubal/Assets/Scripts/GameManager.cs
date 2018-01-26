using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum TYPE_SOL
{
    Normal,
    Glace,
    Slime,
    Saut
}

public enum TYPE_OBJET
{
    Vide,
    Mur,
    Rocher,
    Pickup
}

public class GameManager : MonoBehaviour
{
    private GameObject[,] terrainPhysiqueJeu;
    private TYPE_SOL[,] terrainLogiqueJeu;
    private GameObject[,] objetsPhysiquesJeu;
    private TYPE_OBJET[,] objetsLogiquesJeu;

    private GameObject networkManager;
    private bool isTerrainGenere;

    [Header("Paramètres")]
    [Range(7, 21)]
    public int taille;
    [Range(0, 100)]
    public int tauxRochers;
    [Space(10)]

    [Header("Types de sol")]
    public GameObject sol;
    public GameObject glace;
    public GameObject slime;
    public GameObject saut;
    [Space(10)]

    [Header("Obstacles")]
    public GameObject mur;
    public GameObject rocher;
    [Space(10)]

    [Header("Pickups")]
    public GameObject pickupVitesse;
    public GameObject pickupBombe;
    public GameObject pickupBombeMur;
    public GameObject pickupSuperBombe;
    public GameObject pickupBombeGlace;
    public GameObject pickupBombeGlue;
    public GameObject pickupRange;
    [Space(10)]

    [Header("UI")]
    public Text txtNombreBombes;
    public Text txtRange;
    public Text txtPtsVie;
    public Image slotBomb1;
    public Image slotBomb2;
    public Image detonator;
    public Image heart;
    [Space(10)]

    [Header("HUD")]
    public GameObject canvas;

    [Header("Autres")]
    public GameObject spawnPoint;

    // Use this for initialization
    void Start()
    {
        networkManager = GameObject.FindGameObjectWithTag("Network");
        if (tauxRochers < 0)
            tauxRochers = 0;
        isTerrainGenere = false;
    }

    public void updateUI(int nbrBombes, int range, int ptsVie)
    {
        txtNombreBombes.text = "x" + nbrBombes;
        txtRange.text = "x" + range;
        txtPtsVie.text = ptsVie.ToString();
        if (ptsVie < 10)
        {
            heart.sprite = detonator.sprite;
        }
        
    }
    
    public void UpdateBombUI(TYPE_BOMBE_PICKUP[] playerBombes)
    {
        slotBomb1.sprite = GetComponent<BombUIManager>().GetSpriteBomb(playerBombes[0]);
        slotBomb2.sprite = GetComponent<BombUIManager>().GetSpriteBomb(playerBombes[1]);
    }
    
    public void genererTerrain(int seed)
    {
        if (!isTerrainGenere)
        {
            //Debug.Log("Seed : " + seed);
            Random.InitState(seed);

            terrainPhysiqueJeu = new GameObject[taille, taille];
            terrainLogiqueJeu = new TYPE_SOL[taille, taille];

            objetsPhysiquesJeu = new GameObject[taille, taille];
            objetsLogiquesJeu = new TYPE_OBJET[taille, taille];

            for (int x = 0; x < taille; x++)
            {
                for (int y = 0; y < taille; y++)
                {
                    // GESTION DU SOL
                    terrainPhysiqueJeu[x, y] = Instantiate(sol, this.transform);
                    terrainPhysiqueJeu[x, y].transform.position = new Vector3(x, 0, y);
                    terrainPhysiqueJeu[x, y].name = "Sol X:" + x + " Y:" + y;
                    terrainLogiqueJeu[x, y] = TYPE_SOL.Normal;

                    // La case est vide par défaut, puis si un objet est placé on override la variable
                    objetsLogiquesJeu[x, y] = TYPE_OBJET.Vide;

                    // GESTION DE L'APPARITION DES MURS
                    if (x == 0 || y == 0 || x == taille - 1 || y == taille - 1 || (x % 2 == 0 && y % 2 == 0))
                    {
                        objetsPhysiquesJeu[x, y] = Instantiate(mur, this.transform);
                        objetsPhysiquesJeu[x, y].transform.position = new Vector3(x, 0, y);
                        objetsPhysiquesJeu[x, y].name = "Mur X:" + x + " Y:" + y;

                        objetsLogiquesJeu[x, y] = TYPE_OBJET.Mur;
                    }
                    // GESTION DE L'APPARITION DES JOUEURS
                    else if (x == 1 && y == 1 || x == 1 && y == taille - 2 ||
                        x == taille - 2 && y == 1 || x == taille - 2 && y == taille - 2)
                    {
                        spawnPoint.transform.position = new Vector3(x, 0.4f, y);
                        Instantiate(spawnPoint, networkManager.transform);
                    }
                    // GESTION DE L'APPARITION DES ROCHERS
                    else {
                        if (Random.Range(0, 100) <= tauxRochers && !isLibreObligatoire(x, y, taille) || 
                            (x == 1 || x == 3 || x == taille - 2 || x == taille - 4) &&
                            (y == 1 || y == 3 || y == taille - 2 || y == taille - 4)) {
                            placerRocher(x, y);
                        }
                        // GESTION DE L'APPARITION DES PICKUPS
                        if (Random.value < 0.1f) {
                            Pickup.SpawnRandomPickup(x, y);
                        }
                    }
                }
            }

            // GESTION DES CASES DE SAUT
            if (taille > 10)
            {
                for (int i = 0; i < 4; i++)
                {
                    int randX;
                    int randY;

                    do
                    {
                        randX = Random.Range(3, taille - 4);
                        randY = Random.Range(3, taille - 4);
                    } while (objetsLogiquesJeu[randX, randY] != TYPE_OBJET.Vide && (randX % 2 != 1 && randY % 2 != 1));

                    Destroy(terrainPhysiqueJeu[randX, randY]);
                    terrainPhysiqueJeu[randX, randY] = Instantiate(saut, this.transform);
                    terrainLogiqueJeu[randX, randY] = TYPE_SOL.Saut;

                    terrainPhysiqueJeu[randX, randY].transform.position = new Vector3(randX, 0, randY);
                    terrainPhysiqueJeu[randX, randY].name = "Saut X:" + randX + " Y:" + randY;
                }
            }
            
            isTerrainGenere = true;
        }
    }
    
    public void placerRocher(int x, int y)
    {
        GameObject[] joueurs = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < joueurs.Length; i++)
        {
            if (Mathf.Floor(joueurs[i].transform.position.x) == x && Mathf.Floor(joueurs[i].transform.position.z) == y)
                joueurs[i].GetComponent<PlayerController>().ReduceLife(1000);
        }

        objetsPhysiquesJeu[x, y] = Instantiate(rocher, this.transform);
        objetsPhysiquesJeu[x, y].transform.position = new Vector3(x, 0, y);
        objetsPhysiquesJeu[x, y].name = "Rocher X:" + x + " Y:" + y;

        objetsLogiquesJeu[x, y] = TYPE_OBJET.Rocher;
    }

    public void RetirerObjet(int x, int y) {
        objetsPhysiquesJeu[x, y] = null;
        objetsLogiquesJeu[x, y] = TYPE_OBJET.Vide;
    }

    // Vérifie si la case doit obligatoire être libre ou non
    private bool isLibreObligatoire(int x, int y, int taille)
    {
        bool isLibreObligatoire = false;

        if (x == 1 || x == 2 || x == taille - 3 || x == taille - 2)
        {
            if (y == 1 || y == 2 || y == taille - 3 || y == taille - 2)
            {
                isLibreObligatoire = true;
            }
        }

        return isLibreObligatoire;
    }

    public TYPE_OBJET getTypeObjet(int x, int y)
    {
        return objetsLogiquesJeu[x, y];
    }

    public void ToggleDetonator(bool toggle)
    {
        switch (!toggle != true) {
            case true: 
                {
                    detonator.enabled = !false;
                    break;
                }
            case !true: 
                {
                    detonator.enabled = false;
                    break;
                }
        }
    }
    
    public void ToggleDeathPanel(bool toggle)
    {
        ToggleChildrenWithTag(toggle, "DeathPanel");
        ToggleChildrenWithTag(!toggle, "PlayerHud");
    }

    public void ToggleHudPlayer(bool toggle)
    {
        //ToggleChildrenWithTag(toggle, "DeathPanel");
    }

    void ToggleChildrenWithTag(bool toggle, string nom)
    {
        Transform[] childrenList = canvas.GetComponentsInChildren<Transform>(true);
        foreach(var child in childrenList) {
            if (child.name == nom) {
                child.gameObject.SetActive(toggle);
            }
        }
        /*for (int i = 0; i < childrenList.Length; i++)
        {
            if (childrenList[i].CompareTag(tag))
            {
                childrenList[i].gameObject.SetActive(toggle);
            }
        }*/
    }
}