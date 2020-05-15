using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    public int maxHealth, detectionRadius;
    public Slider healthBarPrefab;
    public Material bodyFadeMaterial, limbFadeMaterial;
    public float walkSpeed;
    public bool isHeavy, useLineOfSight;

    private int health;
    private Slider healthBar;
    private Animator anim;
    private bool isDead;
    private GameObject player, topOfHead, rightHand, leftHand;

    // Start is called before the first frame update
    void Start()
    {
        if (isHeavy)
        {
            Transform spine = transform.Find("Hips").Find("Spine").Find("Spine1").Find("Spine2");
            topOfHead = spine.Find("Neck").Find("Head").Find("HeadTop_End").gameObject;
            rightHand = spine.Find("RightShoulder").Find("RightArm").Find("RightForeArm").Find("RightHand").gameObject;
            leftHand = spine.Find("LeftShoulder").Find("LeftArm").Find("LeftForeArm").Find("LeftHand").gameObject;
        }
        else
        {
            Transform spine = transform.Find("mixamorig:Hips").Find("mixamorig:Spine").Find("mixamorig:Spine1").Find("mixamorig:Spine2");
            topOfHead = spine.Find("mixamorig:Neck").Find("mixamorig:Head").Find("mixamorig:HeadTop_End").gameObject;
            rightHand = spine.Find("mixamorig:RightShoulder").Find("mixamorig:RightArm").Find("mixamorig:RightForeArm").Find("mixamorig:RightHand").gameObject;
            leftHand = spine.Find("mixamorig:LeftShoulder").Find("mixamorig:LeftArm").Find("mixamorig:LeftForeArm").Find("mixamorig:LeftHand").gameObject;
        }

        health = maxHealth;
        healthBar = Instantiate(healthBarPrefab, GameObject.Find("Canvas").transform);
        healthBar.GetComponent<HealthBarController>().target = topOfHead;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;

        anim = GetComponent<Animator>();

        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 toPlayer = player.transform.position - transform.position;
        float angle = Vector3.SignedAngle(transform.forward, toPlayer, Vector3.up);
        anim.SetFloat("distanceToPlayer", toPlayer.magnitude);

        RaycastHit hit;
        String hitName;
        bool lineOfSight = !useLineOfSight;

        if (useLineOfSight)
        {
            Physics.Raycast(topOfHead.transform.position, player.transform.position - topOfHead.transform.position, out hit);

            if (hit.transform != null)
            {
                hitName = hit.transform.name;

                // This would be smarter to do with tags, but this makes it easier to merge things on Github
                lineOfSight = hitName.Equals("Player") || hitName.Equals("SwordCollider") || hitName.Equals("KickCollider")
                    || hitName.Equals("GreatswordCollider") || hitName.Equals("ShieldCollider")
                    || hitName.Equals("Lefthand") || hitName.Equals("RightHand") || hitName.Equals("Mutant");
            }
        }

        bool canSeePlayer = transform.Find("Ch30").GetComponent<SkinnedMeshRenderer>().isVisible && toPlayer.magnitude <= detectionRadius && lineOfSight;

        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(canSeePlayer);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            leftHand.GetComponent<BoxCollider>().enabled = !anim.IsInTransition(0);
            rightHand.GetComponent<BoxCollider>().enabled = !anim.IsInTransition(0);
        }
        else
        {
            leftHand.GetComponent<BoxCollider>().enabled = false;
            rightHand.GetComponent<BoxCollider>().enabled = false;

            AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);

            if (!isDead && canSeePlayer && !animState.IsName("Monster Hit") && !animState.IsName("Monster Cower"))
            {
                if (toPlayer.magnitude > 2.5f)
                {
                    anim.SetFloat("Movement", Mathf.SmoothStep(anim.GetFloat("Movement"), 1, 0.2f));
                    transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime * (isHeavy ? 1.5f : 4));
                }
                else
                {
                    anim.SetFloat("Movement", Mathf.SmoothStep(anim.GetFloat("Movement"), 0, 0.2f));
                }

                if (Mathf.Abs(angle) > 15)
                {
                    transform.Rotate(new Vector3(0, (isHeavy ? 135 : 270), 0) * Time.deltaTime * Mathf.Sign(angle));
                }
            }
            else
            {
                anim.SetFloat("Movement", Mathf.SmoothStep(anim.GetFloat("Movement"), 0, 0.2f));
            }
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void Damage(int damageAmount)
    {
        health = Mathf.Max(0, health - damageAmount);

        if (health == 0 && !isDead)
        {
            isDead = true;
            Destroy(healthBar.gameObject);
            anim.SetTrigger("Death");
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

            IEnumerator death = DeathRoutine(transform.Find("Ch30").GetComponent<SkinnedMeshRenderer>());
            StartCoroutine(death);
        }
        else if (!isDead)
        {
            healthBar.value = health;

            // Die
            anim.CrossFade("Monster Hit", 0.2f);
        }
    }

    IEnumerator DeathRoutine(SkinnedMeshRenderer renderer)
    {
        Physics.IgnoreCollision(GetComponent<BoxCollider>(), player.GetComponent<BoxCollider>());

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject monster in enemies)
        {
            Vector3 distanceToMonster = monster.transform.position - transform.position;
            MonsterController controller = monster.GetComponent<MonsterController>();

            if (!controller.isHeavy && distanceToMonster.magnitude <= detectionRadius && monster.gameObject != gameObject && !controller.IsDead())
            {
                monster.GetComponent<Animator>().CrossFade("Monster Cower", 0.3f);
            }
        }

        yield return new WaitForSeconds(5);

        if (isHeavy)
        {
            renderer.material = bodyFadeMaterial;
        }
        else
        {
            Material[] newMats = { bodyFadeMaterial, limbFadeMaterial };
            renderer.materials = newMats;
        }

        float fade = 1f;
        while (fade >= 0.01)
        {
            fade -= 0.02f;

            if (isHeavy)
            {
                renderer.material.color = new Color(1f, 1f, 1f, fade);
            }
            else
            {
                foreach (Material mat in renderer.materials)
                {
                    mat.color = new Color(1f, 1f, 1f, fade);
                }
            }

            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
