using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody rb;

    private int lifePoints;
    private bool isHavingADetonator;
    private GameObject lastDroppedBomb;
    private float hauteurDeDepart;
    private TYPE_BOMBE_PICKUP[] bombesSpeciales;
    private int nombreBombe;
    private float energie = 1f;
    private bool isJumping;
    private bool doitMonter;
    private const float SPRINT_SPEED = 2f;
    private Vector3 velocite = Vector3.zero;

    [Header("Paramètres")]
    public float movementSpeed = 1f;
    private float altMovementSpeed = 0.2f;
    public bool estRalenti = false;
    public float cameraSpeed;
    public int nombreBombeMax = 1;
    public int rangeExplosion = 2;
    [Space(10)]

    [Header("Bombes")]
    public GameObject bombe;
    public GameObject superBombe;
    public GameObject bombeMur;
    public GameObject bombeGlace;
    public GameObject bombeGlue;

    // 1.0f -> aucun effet
    // 0.01f -> grande réduction de friction
    private const float FRICTION_INIT = 0.4f;
    private float friction;

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        gameManager.ToggleHudPlayer(true);
        rb = GetComponent<Rigidbody>();
        nombreBombe = nombreBombeMax;
        bombesSpeciales = new TYPE_BOMBE_PICKUP[2];
        hauteurDeDepart = transform.position.y;
        isJumping = false;
        isHavingADetonator = true;
        lifePoints = 10;
        UpdateUI();
        friction = FRICTION_INIT;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey("escape")) {

        }
        //Mouvement du joueur en fonction de la souris
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * cameraSpeed);

        // Si le joueur doit être en saut
        if (isJumping)
        {
            Sauter();
        }
        // Sinon il se déplace
        else
        {
            Deplacer();

            //Gestion posage de bombes
            if (nombreBombe > 0) {
                Vector3 spawnpoint = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y - 0.05f, Mathf.RoundToInt(transform.position.z));
                Collider[] hitColliders = Physics.OverlapSphere(spawnpoint, 0, 5);

                if (hitColliders.Length == 0 || hitColliders[0].CompareTag("Player")) {
                    if (Input.GetKeyDown(KeyCode.Space))
                        DropABomb(TYPE_BOMBE_PICKUP.BOMBE);
                    else if (Input.GetKeyDown(KeyCode.Mouse0) && bombesSpeciales[0] != TYPE_BOMBE_PICKUP.BOMBE)
                        DropABomb(bombesSpeciales[0]);
                    else if (Input.GetKeyDown(KeyCode.Mouse1) && bombesSpeciales[1] != TYPE_BOMBE_PICKUP.BOMBE)
                        DropABomb(bombesSpeciales[1]);
                }
            }
        }

        //Detonator
        if (Input.GetKey(KeyCode.Mouse2) && lastDroppedBomb != null && isHavingADetonator)
        {
            DetonateLastBomb();
            isHavingADetonator = false;
        }

        //Empêche le rebond du joueur sur les murs
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        UpdateUI();
    }

    public void SetJumping() {
        isJumping = true;
        doitMonter = true;
    }

    private void Sauter()
    {
        Vector3 vecteurChute;
        if (doitMonter) //Monte jusqu'à une certaine ou hauteur ou un LShift press
        {
            vecteurChute = new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, this.transform.position.z);
        }
        else
        {
            vecteurChute = new Vector3(this.transform.position.x, this.transform.position.y - 0.15f, this.transform.position.z);
        }

        this.transform.position = vecteurChute;

        if (transform.position.y > 15 || Input.GetKeyDown(KeyCode.LeftShift))
            doitMonter = false;
        else if (this.transform.position.y < (hauteurDeDepart)) //Arrête le saut une fois revenur à la position de base
            isJumping = false;
    }

    private void Deplacer()
    {
        //Gestion déplacement du joueur
        Vector3 vec = new Vector3(transform.rotation.x, 0, transform.rotation.z);

        //ZQSD & WASD
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W))
            vec += transform.forward * 0.03f;
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            vec += transform.forward * -0.03f;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A))
            vec += transform.right * -0.03f;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            vec += transform.right * 0.03f;

        //Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (energie > 0.04f)
            {
                vec *= SPRINT_SPEED;
                energie -= 0.02f;
            }
        }
        else if (energie < 1f)
        {
            energie += 0.005f;
        }

        // appliquer la vitesse de déplacement
        vec *= movementSpeed;

        // friction
        vec = (vec * friction) + (velocite * (1 - friction));

        //On change la position du joueur en fonction de tout ce qui s'est passé plus haut
        transform.position = new Vector3(transform.position.x + vec.x, hauteurDeDepart, transform.position.z + vec.z);
        if (transform.rotation.x != 0 || transform.rotation.z != 0)
        {
            transform.rotation = Quaternion.LookRotation(transform.forward);
        }
        velocite = new Vector3(vec.x, hauteurDeDepart, vec.z);
    }
    
    private void DropABomb(TYPE_BOMBE_PICKUP bombType) {
        GameObject spawnedBomb = SpawnBomb(transform.position.x, transform.position.z, rangeExplosion, bombType);
        Bombe spawnedBombController = spawnedBomb.GetComponent<Bombe>();
        spawnedBombController.id = Random.value;

        lastDroppedBomb = spawnedBomb;
        spawnedBombController.player = this;
        nombreBombe--;

        GameObject.FindGameObjectWithTag("Network").GetComponent<ServerManager>().EnvoyerMessageBombe(spawnedBombController);
    }

    public GameObject SpawnBomb(float x, float z, int range, TYPE_BOMBE_PICKUP bombType) {
        GameObject newBomb = null;
        switch (bombType) {
            case TYPE_BOMBE_PICKUP.BOMBE:
                newBomb = bombe;
                break;
            case TYPE_BOMBE_PICKUP.BOMBE_MUR:
                newBomb = bombeMur;
                break;
            case TYPE_BOMBE_PICKUP.BOMBE_GLACE:
                newBomb = bombeGlace;
                break;
            case TYPE_BOMBE_PICKUP.SUPER_BOMBE:
                newBomb = superBombe;
                break;
            case TYPE_BOMBE_PICKUP.BOMBE_GLUANTE:
                newBomb = bombeGlue;
                break;
        }

        GameObject spawnedBomb = null;
        if (newBomb) {
            spawnedBomb = Instantiate(
                    newBomb,
                    new Vector3(Mathf.RoundToInt(x), transform.position.y - 0.12f, Mathf.RoundToInt(z)),
                    Quaternion.Euler(new Vector3(-90, 0, 0)));
            spawnedBomb.GetComponent<Bombe>().range = range;
        }

        return spawnedBomb;
    }

    public GameObject SpawnBomb(float x, float z, int range, TYPE_BOMBE_PICKUP bombType, float id) {
        GameObject bombe = SpawnBomb(x, z, range, bombType);
        bombe.GetComponent<Bombe>().id = id;
        return bombe;
    }

    public void addBombeSpeciale(TYPE_BOMBE_PICKUP typeBombeSpeciale)
    {
        if (bombesSpeciales[0] != typeBombeSpeciale && bombesSpeciales[1] != typeBombeSpeciale) {
            bombesSpeciales[1] = bombesSpeciales[0];
            bombesSpeciales[0] = typeBombeSpeciale;
        }
    }

    public void IncrementerNbrBombeMax()
    {
        this.nombreBombeMax++;
        IncrementerNbrBombe();
    }

    public void IncrementerNbrBombe()
    {
        this.nombreBombe++;
    }

    public void AugmenterVitesse()
    {
        this.movementSpeed += 0.2f;
    }

    public void AugmenterRange()
    {
        this.rangeExplosion++;
    }

    public void ReduceLife(int value)
    {
        lifePoints -= value;
        if (lifePoints <= 0) { 
            lifePoints = 0;
            KillPlayer();
        }
    }
    
    void KillPlayer()
    {
        gameManager.ToggleDeathPanel(true);
        Destroy(gameObject);
    }

    void UpdateUI()
    {
        gameManager.updateUI(nombreBombe, rangeExplosion, lifePoints);
        gameManager.UpdateBombUI(bombesSpeciales);
        gameManager.ToggleDetonator(isHavingADetonator);
    }

    void OnGUI()
    {
        RectTransform boussole = GameObject.FindGameObjectWithTag("GUI_Boussole").GetComponent<RectTransform>();
        RectTransform playerTransform = this.GetComponent<RectTransform>();
        int direction = (int)(playerTransform.rotation.eulerAngles.y);
        
        boussole.rotation = Quaternion.Euler(0.0f, 0.0f, direction);

        Image sprintMeter = GameObject.FindGameObjectWithTag("GUI_SprintMeter").GetComponent<Image>();

        if (energie >= 0.04f)
            sprintMeter.fillAmount = energie;
        else
            sprintMeter.fillAmount = 0;
    }

    public void SetFrictionTemp(float friction) {
        this.friction = friction;
        CancelInvoke("RestaurerFriction");
        Invoke("RestaurerFriction", 0.1f);
    }

    void RestaurerFriction() {
        friction = FRICTION_INIT;
    }

    private void DetonateLastBomb()
    {
        Bombe bombeManager = lastDroppedBomb.GetComponent<Bombe>();

        bombeManager.Explode();
        GameObject.FindGameObjectWithTag("Network").GetComponent<ServerManager>().EnvoyerMessageDetonateur(bombeManager.id);
    }

    public void ReduceMovementSpeed() {
        if (! estRalenti) {
            estRalenti = true;
            movementSpeed *= altMovementSpeed;
            CancelInvoke("ResumeMovementSpeed");
            Invoke("ResumeMovementSpeed", 0.25f);
        }
    }

    public void ResumeMovementSpeed() {
        movementSpeed /= altMovementSpeed;
        estRalenti = false;
    }
    
}
