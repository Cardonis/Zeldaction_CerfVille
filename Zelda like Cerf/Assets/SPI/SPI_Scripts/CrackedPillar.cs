using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedPillar : MonoBehaviour
{
    public float wallDamagePoints;
    public int wallMaxPoints;
    public Vector2 buttonSize;
    int detectedObjectMass;
    float detectedObjectProjectionLevel;
    [HideInInspector] public bool isOpen;
    Collider2D pillarCollider;

    Animator animator;
    
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        pillarCollider = GetComponentInChildren<Collider2D>();
        isOpen = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Elements_Controller element = collision.collider.GetComponentInParent<Elements_Controller>();
        
        if (element != null)
        {
            detectedObjectMass = element.mass;
            detectedObjectProjectionLevel = element.levelProjected;
            wallDamagePoints += detectedObjectMass * detectedObjectProjectionLevel;
            
        }
        if (wallDamagePoints >= wallMaxPoints)
        {
            isOpen = true;
            animator.SetBool("isOpen", isOpen);
            pillarCollider.enabled = !isOpen;
        }
    }
}
