using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    public int maxHealth;
    public Slider healthBarPrefab;
    public Material bodyFadeMaterial, limbFadeMaterial;
    public float walkSpeed;

    private int health;
    private Slider healthBar;
    private Animator anim;
    private bool isDead;
    private GameObject player, topOfHead;

    // Start is called before the first frame update
    void Start()
    {
        //topOfHead = GameObject.Find("mixamorig:HeadTop_End");
        topOfHead = transform.Find("mixamorig:Hips").Find("mixamorig:Spine").Find("mixamorig:Spine1")
            .Find("mixamorig:Spine2").Find("mixamorig:Neck").Find("mixamorig:Head").Find("mixamorig:HeadTop_End").gameObject;

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

        // NEED TO COVER CASE THIS HITS WEAPONS
        RaycastHit hit;
        Physics.Raycast(topOfHead.transform.position, toPlayer, out hit);
        String hitName = hit.transform.name;

        // This would be smarter to do with tags, but this makes it easier to merge things on Github
        bool canSeePlayer = hitName.Equals("Player") || hitName.Equals("SwordCollider") 
            || hitName.Equals("GreatswordCollider") || hitName.Equals("ShieldCollider");

        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(canSeePlayer);
        } 

        if (!isDead && canSeePlayer && !anim.GetCurrentAnimatorStateInfo(0).IsName("Monster Hit"))
        {
            if (toPlayer.magnitude > 2.5f)
            {
                bool run = (player.GetComponent<PlayerController>().GetWeapon() == Weapon.BOW) && (toPlayer.magnitude > 4);

                anim.SetFloat("Movement", Mathf.SmoothStep(anim.GetFloat("Movement"), run ? 1 : 0.5f, 0.2f));
                transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime * (run ? 3 : 1));
            } 
            else
            {
                anim.SetFloat("Movement", Mathf.SmoothStep(anim.GetFloat("Movement"), 0, 0.2f));
            }

            if (Mathf.Abs(angle) > 15)
            {
                transform.Rotate(new Vector3(0, 135, 0) * Time.deltaTime * Mathf.Sign(angle));
            }
        } 
        else
        {
            anim.SetFloat("Movement", Mathf.SmoothStep(anim.GetFloat("Movement"), 0, 0.2f));
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

        yield return new WaitForSeconds(5);

        Material[] newMats = { bodyFadeMaterial, limbFadeMaterial };
        renderer.materials = newMats;

        float fade = 1f;
        while (fade >= 0.01)
        {
            fade -= 0.02f;

            foreach (Material mat in renderer.materials)
            {
                
                mat.color = new Color(1f, 1f, 1f, fade);
            }

            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);
    }
}
