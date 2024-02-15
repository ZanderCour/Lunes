using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;


public class CompanionController : MonoBehaviour
{
    public float speed;
    public float distance;
    public float distanceTrehshold;
    public float slowDistance;
    public float stopDistance;
    public Transform owner;
    [SerializeField] private NavMeshAgent agent;
    float originalSpeed;

    [Header("Animation")]
    public Animator anim;
    [SerializeField] private float LocomotionanimatorFloat;
    public float acceleration;
    public float deceleration;

    private void Start()
    {
        originalSpeed = speed;
    }

    private void Update()
    {
        agent.speed = speed;

        GetDistance(owner);
        HandleAnimations();

        if (distance > distanceTrehshold) 
        {
            MoveToOwner(owner);
        }

        if (distance < stopDistance)
        {
            agent.SetDestination(this.transform.position);
        }

        if (distance < slowDistance && speed > 0)
        {
            speed -= 1.5f * Time.deltaTime;
        }
        else if(distance > slowDistance)
        {
            speed = originalSpeed;
        }
        else if(speed < 0)
        {
            speed = 0;
        }
    }

    void GetDistance(Transform target)
    {
        if (owner != null)
            distance = Vector3.Distance(this.gameObject.transform.position, target.position);
    }

    public void MoveToOwner(Transform target)
    {
        agent.SetDestination(target.position);
    }

    private void HandleAnimations()
    {
        anim.SetFloat("Movement", LocomotionanimatorFloat);

        bool isSprinting = false;
        bool isIdle = false;
        bool isWalking = false;

        isWalking = true ? distance > distanceTrehshold && !isSprinting : isWalking = false;
        isSprinting = true ? distance > distanceTrehshold + 3f : isSprinting = false;
        isIdle = true ? distance < stopDistance : isIdle = false;
        

        if (isWalking && LocomotionanimatorFloat < 0.5f)
        {
            LocomotionanimatorFloat += Time.deltaTime * acceleration;
        }

        if (isSprinting && LocomotionanimatorFloat < 1.0f)
        {
            LocomotionanimatorFloat += Time.deltaTime * acceleration;
        }

        if (!isSprinting && isWalking && LocomotionanimatorFloat > 0.5f)
        {
            LocomotionanimatorFloat -= Time.deltaTime * deceleration;
        }

        if (isIdle && LocomotionanimatorFloat > 0.0f)
        {
            LocomotionanimatorFloat -= Time.deltaTime * deceleration;
        }

        if (isWalking && LocomotionanimatorFloat < 0.0f)
        {
            LocomotionanimatorFloat = 0f;
        }
    }
}
