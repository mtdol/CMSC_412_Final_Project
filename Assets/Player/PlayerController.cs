using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Weapon : int { SWORD = 0, GREATSWORD = 1, BOW = 2 };

public class PlayerController : MonoBehaviour
{

    // represents the dungeons, can be used in the state arrays stored
    // in the player controller
    public const int FOREST_DUNGEON = 0;
    public const int DESERT_DUNGEON = 1;
    public static bool HAVE_KEY = false;

    // is set to true by the appropriate dungeon controller when the dungeon is beaten
    private static bool[] dungeonCompletion = {
        // forest
        false,
        // desert
        false
    
    };

    // the dungeons that the player has access to
    // the dungeon will block the player if the player hasn't been granted access
    private static bool[] dungeonAccess =
    {
        // forest
        false,
        // desert
        false
    };

    // the location in the overworld the player should spawn to,
    // defaults to these coordinates
    private static int playerSpawn = DEFAULT_OVERWORLD_SPAWN;
    // these are the keys that tell the Overworld controller where to spawn the player
    // these are stored in the above variable
    public const int DEFAULT_OVERWORLD_SPAWN = 0;
    public const int FOREST_DUNGEON_SPAWN = 1;
    public const int DESERT_DUNGEON_SPAWN = 2;


    public float forwardSpeed;
    public float backwardSpeed;
    public float strafeSpeed;
    public float rotationSpeed;
    public float runMult;
    public int maxHealth;
    public Slider playerHealthBar;

    private Animator anim, bowAnim;

    public GameObject sword, greatsword, shield, bow, prefabArrow, terrain;
    private GameObject currentArrow, foot;
    private Weapon weapon;

    private int health;

    private bool attack, jump, run, shieldUp, aim; // Bools for actions the player can do
    private bool switchWeapon; // Weapon switching
    private bool isDead; 

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        bowAnim = bow.GetComponent<Animator>();
        foot = GameObject.Find("KickCollider");

        attack = false;
        jump = false;
        run = false;
        shieldUp = false;
        switchWeapon = false;
        weapon = Weapon.SWORD;

        health = maxHealth;

        if (playerHealthBar != null)
        {
            playerHealthBar.maxValue = maxHealth;
            playerHealthBar.value = health;
        }

        SwordController swordController = sword.GetComponentInChildren<SwordController>();
        swordController.terrain = terrain;
        swordController.damageAmount = 1;

        SwordController greatswordController = greatsword.GetComponentInChildren<SwordController>();
        greatswordController.terrain = terrain;
        greatswordController.damageAmount = 3;

        SwordController footController = foot.GetComponentInChildren<SwordController>();
        footController.terrain = terrain;
        footController.damageAmount = 1;

        ShieldController shieldController = shield.GetComponentInChildren<ShieldController>();
        shieldController.terrain = terrain;
    }

    private void Update()
    {

        //Debug.Log(HAVE_KEY);
        // Detect if these keys are being held down or not
        run = Input.GetKey(KeyCode.LeftShift);
        shieldUp = Input.GetKey(KeyCode.Mouse1);
        aim = Input.GetKey(KeyCode.Mouse0) && (weapon == Weapon.BOW);

        // Detect if these keys have been pressed (but not necessarily held)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !jump)
        {
            attack = true;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            switchWeapon = true;
        }
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        // Use these to get the names of whatever animation/transition we're in
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
        AnimatorTransitionInfo animTrans = anim.GetAnimatorTransitionInfo(0);

        // Prioritize any attack input that we get...
        // Note that, for swords/greatsword, this first if block only run when we START attacking (not during the entire anim; see below)
        if (aim || attack)
        {
            if (aim)
            {
                // Set the animations for the player and bow to draw an arrow
                anim.SetBool("Aim", true);
                bowAnim.SetBool("Draw", true);

                // If we don't have an arrow loaded, create one
                if (currentArrow == null)
                {
                    GameObject bowStringBone = GameObject.Find("WB.string");
                    currentArrow = Instantiate(prefabArrow, bowStringBone.transform.position, bow.transform.rotation, bow.transform);

                    // Easier to do a bit of tweaking here than inside the Instantiate() above...
                    currentArrow.transform.Rotate(Vector3.forward, -85);
                    currentArrow.transform.Translate(new Vector3(0, -0.45f, 0));
                }

                // If we're aiming, we still want to be allowed to turn
                float mouseHorizontal = Input.GetAxis("Mouse X");
                if (Mathf.Abs(mouseHorizontal) > 0.1f)
                {
                    Rotate(mouseHorizontal);
                    anim.SetBool("Turn", true);
                }
                else
                {
                    anim.SetBool("Turn", false);
                }
            }
            else
            {
                // If we're not aiming, then we're using the sword or greatsword
                anim.SetTrigger("Attack");
            }

            attack = false;
        }
        else if (shieldUp)
        {
            // Set animation and "enable" shield collisions
            anim.SetBool("Shield", true);
            shield.GetComponentInChildren<ShieldController>().guarding = true;
        }
        else
        {
            // Make sure to disable any booleans that might still be on
            anim.SetBool("Shield", false);
            anim.SetBool("Aim", false);

            // Turn off any weapon-specific behavior we're not using
            if (weapon == Weapon.BOW)
            {
                bowAnim.SetBool("Draw", false);

                if (currentArrow != null)
                {
                    currentArrow.GetComponent<ArrowController>().released = true;
                    currentArrow = null;
                }
            }
            else if (weapon == Weapon.SWORD)
            {
                shield.GetComponentInChildren<ShieldController>().guarding = false;
            }

            // Jump or switch weapon if appropriate (these things're in their own if block
            // because we can still move while we're doing them)
            if (jump)
            {
                anim.SetTrigger("Jump");
                jump = false;
            }
            else if (switchWeapon)
            {
                anim.SetTrigger("SwitchWeapon");

                // Set weapons active/inactive depending on which we're switching to.
                // Cycle goes: Sword -> Greatsword -> Bow -> Sword
                if (weapon == Weapon.SWORD)
                {
                    shieldUp = false;
                    sword.SetActive(false);
                    shield.SetActive(false);
                    greatsword.SetActive(true);

                    weapon = Weapon.GREATSWORD;
                }
                else if (weapon == Weapon.GREATSWORD)
                {
                    greatsword.SetActive(false);
                    bow.SetActive(true);

                    weapon = Weapon.BOW;
                }
                else
                {
                    bow.SetActive(false);
                    sword.SetActive(true);
                    shield.SetActive(true);

                    weapon = Weapon.SWORD;
                }

                switchWeapon = false;
            }

            // Perform behavior based on whether we're attacking (with sword/greatsword) or not
            if (!animState.IsTag("Attack") && !animTrans.IsName("Movement -> Slash") && !animTrans.IsName("Greatsword Movement -> Greatsword Slash"))
            {
                // "Disable" collisions
                if (weapon == Weapon.SWORD)
                {
                    sword.GetComponentInChildren<SwordController>().attacking = false;
                }
                else if (weapon == Weapon.GREATSWORD)
                {
                    greatsword.GetComponentInChildren<SwordController>().attacking = false;
                }

                // Move!
                HandleMovement();
            }
            else
            {
                if (weapon == Weapon.SWORD)
                {
                    // If we're using a sword, our third attack in the chain is the kick. If we're doing that, enable
                    // the foot collider; otherwise, enable the sword collider
                    if (animState.IsName("Kick"))
                    {
                        sword.GetComponentInChildren<SwordController>().attacking = false;
                        foot.GetComponent<SwordController>().attacking = true;
                    }
                    else
                    {
                        sword.GetComponentInChildren<SwordController>().attacking = true;
                        foot.GetComponent<SwordController>().attacking = false;
                    }
                }
                else if (weapon == Weapon.GREATSWORD)
                {
                    greatsword.GetComponentInChildren<SwordController>().attacking = true;
                }
            }
        }
    }


    
    private void HandleMovement()
    {
        // Get input
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        float mouseHorizontal = Input.GetAxis("Mouse X");
        float forwardMotion, sideMotion;

        // Stuff based on whether we're going forward or backward
        if (vAxis > 0)
        {
            forwardMotion = forwardSpeed * (run ? runMult : 1);
            anim.SetFloat("Move", Mathf.SmoothStep(anim.GetFloat("Move"), (run ? 1 : 0.5f), 0.2f));
        }
        else if (vAxis < 0)
        {
            forwardMotion = backwardSpeed;
            anim.SetFloat("Move", Mathf.SmoothStep(anim.GetFloat("Move"), -0.5f, 0.2f));
        }
        else
        {
            forwardMotion = 0;
            anim.SetFloat("Move", Mathf.SmoothStep(anim.GetFloat("Move"), 0, 0.15f));
        }

        // Stuff based on whether we're strafing
        if (hAxis > 0 && vAxis == 0)
        {
            sideMotion = strafeSpeed;
            anim.SetFloat("Strafe", Mathf.SmoothStep(anim.GetFloat("Strafe"), (run ? 1 : 0.5f), 0.2f));
        }
        else if (hAxis < 0 && vAxis == 0)
        {
            sideMotion = strafeSpeed;
            anim.SetFloat("Strafe", Mathf.SmoothStep(anim.GetFloat("Strafe"), (run ? -1 : -0.5f), 0.2f));
        }
        else
        {
            sideMotion = 0;
            anim.SetFloat("Strafe", Mathf.SmoothStep(anim.GetFloat("Strafe"), 0, 0.2f));
        }

        // Stuff based on whether we're rotating
        if (Mathf.Abs(mouseHorizontal) > 0.1f)
        {
            Rotate(mouseHorizontal);

            if (vAxis == 0 && hAxis == 0)
            {
                anim.SetFloat("TurnSpeed", mouseHorizontal);
                anim.SetBool("Turn", true);
            }
        }
        else
        {
            anim.SetBool("Turn", false);
        }

        // Actually calculate and implement the movement
        Vector3 forwardVector = Vector3.forward * vAxis;
        Vector3 sideVector = Vector3.right * hAxis;
        Vector3 normalized = Vector3.Normalize(forwardVector + sideVector);
        normalized.z *= forwardMotion;
        normalized.x *= sideMotion;
        transform.Translate(normalized * 1.5f * Time.deltaTime);
    }


    // rotates the character in fixed update time
    private void Rotate(float rotation)
    {
        Vector3 desiredRotation = new Vector3(0, 10, 0) * Time.fixedDeltaTime * rotation * rotationSpeed;
        transform.Rotate(desiredRotation);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyWeapon"))
        {
            Damage(1);
        }
        else if (other.gameObject.CompareTag("EnemyHeavyWeapon"))
        {
            Damage(2);
        }
        else if (other.gameObject.CompareTag ("key"))
        {
            HAVE_KEY = true;
            //Debug.Log("oof");
            SceneManager.LoadScene(3);
    
        }


    }


    private void Damage(int damageAmount)
    {
        if (!shieldUp)
        {
            health = Mathf.Max(health - damageAmount, 0);
        }

        if (!isDead && health == 0)
        {
            isDead = true;

            if (playerHealthBar != null)
            {
                Destroy(playerHealthBar.gameObject);
            }

            // Die
            anim.CrossFade("Death", 0.3f);
        }
        else if (!isDead && playerHealthBar != null)
        {
            playerHealthBar.value = health;
        }
    }

    // sets the given dungeon's completion status using the dungeon codes defined above
    public void SetDungeonCompletion(int dungeon, bool v)
    {
        dungeonCompletion[dungeon] = v;
    }

    // returns the completion status of the given dungeon
    public bool GetDungeonCompletion(int dungeon)
    {
        return dungeonCompletion[dungeon];
    }

    public Weapon GetWeapon()
    {
        return weapon;
    }

    // sets the location the player should spawn to in the overworld
    public void SetPlayerSpawn(int position)
    {
        playerSpawn = position;
    }
    // gets where in the overworld the player should spawn
    public int GetPlayerSpawn()
    {
        return playerSpawn;
    }

    // sets the given dungeon's access status using the dungeon codes defined above
    public void SetDungeonAccess(int dungeon, bool v)
    {
        dungeonAccess[dungeon] = v;
    }

    // returns the access status of the given dungeon
    public bool GetDungeonAccess(int dungeon)
    {
        return dungeonAccess[dungeon];
    }
}
