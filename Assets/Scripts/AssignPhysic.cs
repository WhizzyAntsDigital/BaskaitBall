using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignPhysic : MonoBehaviour
{
    public PhysicMaterial bounceMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AlmostGround"))
        {
            GetComponent<SphereCollider>().material = bounceMaterial;
        }
    }
}
