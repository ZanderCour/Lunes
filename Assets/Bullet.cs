using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public int Damage;
    [SerializeField] private Vector3 BoxSize;
    public LayerMask layerMask;

    private void FixedUpdate()
    {
        Vector3 direction = transform.forward;
        RaycastHit hit;

        if (Physics.BoxCast(transform.position, BoxSize / 2, direction, out hit, Quaternion.identity, Mathf.Infinity, layerMask))
        {
            if (hit.transform.GetComponent<Health>()) {
                bool dealtDamage = false;
                Health hitHealthSystem = hit.transform.GetComponent<Health>();

                if(!dealtDamage)
                    hitHealthSystem.TakeDamage(Damage);

                dealtDamage = true;
            }

            Destroy(this.gameObject, 0.25f);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + transform.forward * (BoxSize.z / 2), BoxSize);
    }

}
