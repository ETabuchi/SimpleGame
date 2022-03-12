using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player's melee attack script that when
// hitting a cuttable object triggers its 
// Sprite_Slice script to cut it into two pieces

public class Player_Attack : MonoBehaviour
{
    // Cursor position
    private GameObject reticle;

    // Attack range
    private float attack_range; // how far the attack will reach 
    private int attack_width;   // how wide of a check to see if there is viable object (odd number)

    void Start()
    {
        reticle = transform.GetChild(0).gameObject;
        attack_range = 2f;
        attack_width = 3;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SliceDash()); 
        }
    }

    // Player will dash towards target location up to the attack_range
    // and slice in half any cuttable objects in their way
    IEnumerator SliceDash()
    {
        // Check using Raycasts if there is any cuttable objects in the way (wide effect)
        RaycastHit2D[] hit = null;
        bool target_hit = false;
        for (int i = 0; i < attack_width; i++)
        {
            if (i % 2 == 0)
            {
                hit = Physics2D.RaycastAll(transform.position, reticle.transform.position - transform.position + (0.5f * i * Vector3.down), attack_range, LayerMask.GetMask("Cuttable"));
            }
            else
            {
                hit = Physics2D.RaycastAll(transform.position, reticle.transform.position - transform.position - (0.5f * i * Vector3.down), attack_range, LayerMask.GetMask("Cuttable"));
            }
            
            if (hit != null)
            {
                target_hit = true;
                break;
            }
        }

        // If a cuttable target(s) was found, access its Sprite_Slice script to cut it in half
        if (target_hit)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                //StartCoroutine(hit[i].collider.GetComponent<Sprite_Slice>().SliceSprite(transform.position, reticle.transform.position - transform.position));
            }
        }
        yield return new WaitForSeconds(0f);
    }
}
