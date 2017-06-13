using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyPatrol : MonoBehaviour {

    NavMeshAgent nmAgent;

    public GameObject currentTarget;
    public GameObject[] path;
    int pathIndex;
    float distanceToNext;
    public bool patrolling;
    float maxPlayerDistance;
    float minPlayerDistance;

    public bool attack;

    Animator anim;

    float health;
    Canvas healthBar;
    public RectTransform healthBarImage;

    public AudioClip creepy;
    public AudioClip grunt1;
    public AudioClip grunt2;

    public GameObject[] collectibles;

	// Use this for initialization
	void Start () {

        nmAgent = GetComponent<NavMeshAgent>();
        if (!nmAgent)
        {
            Debug.LogWarning("NavMeshAgent not found on " + this.name);
            nmAgent = gameObject.AddComponent<NavMeshAgent>();
            nmAgent.baseOffset = 1;
            nmAgent.height = 3;
            nmAgent.speed = 5;
        }

        if (path.Length > 0)
            targetNextNode();

        distanceToNext = 2;
        patrolling = true;
        maxPlayerDistance = 50;
        minPlayerDistance = 2;

        anim = GetComponentInChildren<Animator>();
        if (!anim)
        {
            Debug.LogWarning("Animator not found on " + this.name);
        }

        health = 100;
        healthBar = GetComponentInChildren<Canvas>();
        if (!healthBar)
            Debug.LogWarning("HealthBar Canvas of " + this.name + " not found");

        //healthBarImage = transform.FindChild("HealthBar Canvas")
        //    .transform.FindChild("Background")
        //    .transform.FindChild("Foreground").GetComponent<RectTransform>();
        //if (!healthBarImage)
        //    Debug.LogWarning("Health Bar Image " + this.name + " not found");

	}
	
	// Update is called once per frame
	void Update () {

        healthBar.transform.LookAt(Camera.main.transform);

        if (health > 0)
        {
            anim.SetFloat("Speed", Mathf.Abs(nmAgent.velocity.x) + Mathf.Abs(nmAgent.velocity.z));
            anim.SetBool("Attack", attack);

            if (patrolling)
            {
                if (Vector3.Distance(currentTarget.transform.position, this.transform.position) < distanceToNext)
                {
                    ++pathIndex;
                    pathIndex %= path.Length;
                    targetNextNode();
                }
            }
            else
            {
                if (Vector3.Distance(currentTarget.transform.position, this.transform.position) > maxPlayerDistance)
                {
                    targetNextNode();
                    patrolling = true;
                }
                else if (Vector3.Distance(currentTarget.transform.position, this.transform.position) <= minPlayerDistance)
                {
                    //transform.LookAt(currentTarget.transform.FindChild("LookAt").transform.position);
                    attack = true;
                    nmAgent.velocity = Vector3.zero;
                    SoundManager.instance.playSFX(grunt1);
                }
                else
                {
                    attack = false;
                    nmAgent.SetDestination(currentTarget.transform.position);
                }
            }
        }
        else
        {
            StartCoroutine(deathSequence());
        }
	}

    void targetNextNode()
    {
        nmAgent.speed = 5;
        nmAgent.stoppingDistance = 1;
        currentTarget = path[pathIndex];

        if (currentTarget)
            nmAgent.SetDestination(currentTarget.transform.position);
    }

    void targetPlayer()
    {
        SoundManager.instance.playSFX(creepy);
        patrolling = false;
        nmAgent.speed = 10;
        nmAgent.stoppingDistance = 3;
    }

    private IEnumerator deathSequence()
    {
        anim.SetTrigger("Death");
        nmAgent.Stop();
        currentTarget = null;
        patrolling = false;
        healthBar.gameObject.SetActive(false);
        yield return new WaitForSeconds(4.5f);      // Length of death animation
        int rand = Random.Range(0, 1);
        GameObject globe = Instantiate(collectibles[rand], transform.position + Vector3.up, transform.rotation) as GameObject;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider c)
    {
        //Debug.Log(gameObject.name + " entered trigger: " + c.gameObject.name);
        if (c.gameObject.tag == "Player")
        {
            if (patrolling)
            {
                currentTarget = c.gameObject;
                targetPlayer();
            }
        }
        else if (c.gameObject.tag == "IceAttack")
        {
            health -= 50;
            healthBarImage.sizeDelta = new Vector2(health, healthBarImage.sizeDelta.y);
        }
        //else if (c.gameObject.name == "LeftHand" || c.gameObject.name == "RightHand" || c.gameObject.tag == "PlayerPunchAttack")
        //{
        //    health -= 20;
        //    healthBarImage.sizeDelta = new Vector2(health, healthBarImage.sizeDelta.y);
        //}
    }
}
