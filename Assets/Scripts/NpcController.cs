using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;


public class NpcController : MonoBehaviour
{
    public DialogSO NpcDialog;

    public string name;
    public float offset;
    private float speed;

    public GameObject player;
    private MultiAimConstraint aimConstraint;
    [SerializeField] private float value;
    public Transform NPCRootBones;
    private Health NpcHealth;

    public enum AnimationState
    {
        Idle,
        StandingTalking,
        SittingTalking,
        SittingIdle
    };

    public AnimationState state = new AnimationState();


    private void Start()
    {
        aimConstraint = GetComponentInChildren<MultiAimConstraint>();
        NpcHealth = GetComponent<Health>();
        Revive();
    }

    private void Awake()
    {
        name = NpcDialog.characterName;
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger(state.ToString());

        speed = Random.Range(0.75f, 1);
        offset = Random.Range(0.5f, 2);

        anim.speed = speed;
        anim.SetFloat("Offset", offset);

        if(state.ToString() == "StandingTalking")
        {
            int number = Random.Range(1, 3);
            anim.SetInteger("Random", number);
        }
    }
    

    private void FixedUpdate()
    {
        aimConstraint.weight = value;

        if (aimConstraint != null)
        {
            Vector3 dir = (transform.position - player.transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, dir);
            
            if (Vector3.Distance(this.transform.position, player.transform.position) < 3 && dot < 0)
            {
                if(value < 0.75f)
                {
                    value += 0.05f;
                }
            }
            else
            {
                if(value > 0)
                {
                    value -= 0.05f;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
            Revive();

    }

    public void Die()
    {
        Collider[] collider = GetComponentsInChildren<Collider>();
        foreach (Collider col in collider)
        {
            col.enabled = true;
        }

        Rigidbody[] rb = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody r in rb)
        {
            r.isKinematic = false;
        }

        Animator anim = GetComponent<Animator>();
        Collider collilder = GetComponent<Collider>();

        anim.enabled = false;
        collilder.enabled = false;
    }

    public void Revive()
    {
        Collider[] collider = GetComponentsInChildren<Collider>();
        foreach (Collider col in collider)
        {
            col.enabled = false;
        }

        Rigidbody[] rb = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody r in rb)
        {
            r.isKinematic = true;
        }

        Animator anim = GetComponent<Animator>();
        Collider collilder = GetComponent<Collider>();

        anim.enabled = true;
        collilder.enabled = true;
        NpcHealth.Heal();
    }

}
