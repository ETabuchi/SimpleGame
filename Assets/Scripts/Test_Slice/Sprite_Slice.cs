using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Cuts a 2D sprite into two halves based off two points
// Point 1: Starting point (i.e. player / sword / melee)
// Point 2: End point (i.e. where you are aiming)
public class Sprite_Slice : MonoBehaviour
{
    // GameObject Component(s)
    private PolygonCollider2D pc; // Collider to be manipulated / sliced
    private Rigidbody2D rb;
    private GameObject mask;      // Mask to be instantiated to make sliced parts not appear

    public GameObject x;
    public GameObject y;

    void Start()
    {
        pc = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        mask = transform.GetChild(0).gameObject;
        StartCoroutine(SliceSpriteDown(x.transform.position, y.transform.position));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //StartCoroutine(SliceSprite(x.transform.position, y.transform.position));
        }
    }

    void SpriteSlice(Vector2 start, Vector2 end)
    {
        // Create copy of object
        GameObject copy = Instantiate(gameObject, transform.position, Quaternion.identity);
        PolygonCollider2D pc_copy = copy.GetComponent<PolygonCollider2D>();
    }

    // Cuts everything and leves only upward half
    public IEnumerator SliceSpriteUp(Vector2 start, Vector2 end)
    {
        // Get points of PolygonCollider2D
        Vector2[] col_points = pc.points;

        // Calculate slope, y-intercept, and angle between two points
        float slope = (start.y - end.y) / (start.x - end.x);
        float intercept = start.y - slope * start.x;
        float angle = Mathf.Rad2Deg * Mathf.Atan2((start.y - end.y), (start.x - end.x));

        // Get two farmost intersection points
        RaycastHit2D[] int_points = Physics2D.RaycastAll(start, end - start, LayerMask.GetMask("Cuttable"));
        Vector2 start_point = Vector2.zero;
        Vector2 end_point = Vector2.zero;

        for (int i = 0; i < int_points.Length; i++)
        {
            if (int_points[i].collider.gameObject == this.gameObject)
            {
                if (start_point == Vector2.zero)
                {
                    start_point = int_points[i].point;
                }
                else
                {
                    end_point = int_points[i].point;
                }
            }
        }

        GameObject new_mask = Instantiate(mask, new Vector3((start.x + end.x) / 2, (start.y + end.y) / 2, 0), Quaternion.identity, transform);
        new_mask.SetActive(true);
        new_mask.transform.rotation = Quaternion.Euler(0, 0, angle + 180);

        // Case 1: Starting point is the rightmost point 
        if (start.x > end.x)
        {
            new_mask.transform.rotation = Quaternion.Euler(0, 0, angle);
            for (int i = 0; i < col_points.Length; i++)
            {
                // Move all points to right of line along line or at start_point / end_point
                if ((transform.position.x + col_points[i].x) < ((transform.position.y + col_points[i].y - intercept) / slope))
                {
                    // Calculate how far each point is to the points of intersection to check whether to put along line or intersection point
                    float start_dist = Vector2.Distance(start_point, (Vector2)transform.position + col_points[i]);
                    float end_dist = Vector2.Distance(end_point, (Vector2)transform.position + col_points[i]);

                    // If point is closer to starting intersection point
                    if (start_dist <= end_dist)
                    {
                        if (start_point.y > end_point.y)
                        {
                            if (start_point.y > (transform.position.y + col_points[i].y))
                            { col_points[i] = start_point - (Vector2)transform.position; }
                            else
                            { col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x; }
                        }
                        else
                        {
                            Debug.Log("hi2");
                            if (start_point.y < (transform.position.y + col_points[i].y))
                            { col_points[i] = start_point - (Vector2)transform.position; }
                            else
                            { col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x; }
                        }
                    }
                    else // If point is closer to ending intersection point
                    {
                        if (end_point.y < start_point.y)
                        {
                            if (end_point.y > (transform.position.y + col_points[i].y))
                            { col_points[i] = end_point - (Vector2)transform.position; }
                            else
                            { col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x; }
                        }
                        else
                        {
                            Debug.Log("hi");
                            if (end_point.y < (transform.position.y + col_points[i].y))
                            { col_points[i] = end_point - (Vector2)transform.position; }
                            else
                            { col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x; }
                        }
                    }
                }
            }
        }
        else if (start.y >= end.y) // Case 2: Starting point is above end point regardless of x-position
        {
            for (int i = 0; i < col_points.Length; i++)
            {
                // Move all points to left of line along line or at start_point / end_point
                if ((transform.position.x + col_points[i].x) <= ((transform.position.y + col_points[i].y - intercept) / slope))
                {
                    // Calculate how far each point is to the points of intersection to check whether to put along line or intersection point
                    float start_dist = Vector2.Distance(start_point, (Vector2)transform.position + col_points[i]);
                    float end_dist = Vector2.Distance(end_point, (Vector2)transform.position + col_points[i]);

                    // If point is closer to starting intersection point
                    if (start_dist <= end_dist)
                    {
                        if (start_point.y < end_point.y)
                        {
                            if (start_point.y > (transform.position.y + col_points[i].y))
                            {
                                col_points[i] = start_point - (Vector2)transform.position;
                            }
                            else
                            {
                                col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x;
                            }
                        }
                        else
                        {
                            if (start_point.y > (transform.position.y + col_points[i].y))
                            {
                                col_points[i] = start_point - (Vector2)transform.position;
                            }
                            else
                            {
                                col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x;
                            }
                        }
                    }
                    else // If point is closer to ending intersection point
                    {
                        if (end_point.y > start_point.y)
                        {
                            if (end_point.y < (transform.position.y + col_points[i].y))
                            {
                                col_points[i] = end_point - (Vector2)transform.position;
                            }
                            else
                            {
                                col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x;
                            }
                        }
                        else
                        {
                            if (end_point.y > (transform.position.y + col_points[i].y))
                            {
                                col_points[i] = end_point - (Vector2)transform.position;
                            }
                            else
                            {
                                col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x;
                            }
                        }
                    }
                }
            }
        }
        else if (start.y < end.y) // Case 3: Starting point is below end point regardless of x-position
        {
            for (int i = 0; i < col_points.Length; i++)
            {
                // Move all points to right of line along line or at start_point / end_point
                if ((transform.position.x + col_points[i].x) > ((transform.position.y + col_points[i].y - intercept) / slope))
                {
                    // Calculate how far each point is to the points of intersection to check whether to put along line or intersection point
                    float start_dist = Vector2.Distance(start_point, (Vector2)transform.position + col_points[i]);
                    float end_dist = Vector2.Distance(end_point, (Vector2)transform.position + col_points[i]);

                    // If point is closer to starting intersection point
                    if (start_dist <= end_dist)
                    {
                        if (start_point.y < end_point.y)
                        {
                            if (start_point.y > (transform.position.y + col_points[i].y))
                            {
                                col_points[i] = start_point - (Vector2)transform.position;
                            }
                            else
                            {
                                col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x;
                            }
                        }
                        else
                        {
                            if (start_point.y < (transform.position.y + col_points[i].y))
                            {
                                col_points[i] = start_point - (Vector2)transform.position;
                            }
                            else
                            {
                                col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x;
                            }
                        }
                    }
                    else // If point is closer to ending intersection point
                    {
                        if (end_point.y > start_point.y)
                        {
                            if (end_point.y >= (transform.position.y + col_points[i].y))
                            {
                                col_points[i] = end_point - (Vector2)transform.position;
                            }
                            else
                            {
                                col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x;
                            }
                        }
                        else
                        {
                            if (end_point.y > (transform.position.y + col_points[i].y))
                            {
                                col_points[i] = end_point - (Vector2)transform.position;
                            }
                            else
                            {
                                col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x;
                            }
                        }
                    }
                }
            }
        }

        pc.points = col_points;
        rb.gravityScale = 1;
        pc.isTrigger = false;
        yield return new WaitForSeconds(0f);
    }


    // Cuts everything and leaves only the downward half
    public IEnumerator SliceSpriteDown(Vector2 start, Vector2 end)
    {
        // Create copy of object
        // GameObject copy = Instantiate(gameObject, transform.position, Quaternion.identity);
        // PolygonCollider2D pc_copy = copy.GetComponent<PolygonCollider2D>();

        // Get points of PolygonCollider2D
        Vector2[] col_points = pc.points;
        //Vector2[] col_points_copy = pc_copy.points;
        //GameObject copy_mask = copy.transform.GetChild(0).gameObject;

        // Calculate slope, y-intercept, and angle between two points
        float slope = (start.y - end.y) / (start.x - end.x);
        float intercept = start.y - slope * start.x;
        float angle = Mathf.Rad2Deg * Mathf.Atan2((start.y - end.y), (start.x - end.x));

        // Get two farmost intersection points
        RaycastHit2D[] int_points = Physics2D.RaycastAll(start, end - start, LayerMask.GetMask("Cuttable"));
        Vector2 start_point = Vector2.zero;
        Vector2 end_point = Vector2.zero;

        for (int i = 0; i < int_points.Length; i++)
        {
            if (int_points[i].collider.gameObject == this.gameObject)
            {
                if (start_point == Vector2.zero)
                {
                    start_point = int_points[i].point; 
                }
                else
                {
                    end_point = int_points[i].point;
                }
            }
        }

        GameObject new_mask = Instantiate(mask, new Vector3((start.x + end.x) / 2, (start.y + end.y) / 2, 0), Quaternion.identity, transform);
        new_mask.SetActive(true);
        new_mask.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Case 1: Starting point is the rightmost point 
        if (start.x > end.x)
        {
            for (int i = 0; i < col_points.Length; i++)
            {
                // Move all points under line along line or at start_point / end_point
                if ((transform.position.x + col_points[i].x) <= ((transform.position.y + col_points[i].y - intercept) / slope))
                {
                    // Calculate how far each point is to the points of intersection to check whether to put along line or intersection point
                    float start_dist = Vector2.Distance(start_point, (Vector2)transform.position + col_points[i]);
                    float end_dist = Vector2.Distance(end_point, (Vector2)transform.position + col_points[i]);

                    // If point is closer to starting intersection point
                    if (start_dist <= end_dist)
                    {
                        if (start_point.y > end_point.y)
                        {
                            Debug.Log("hi");
                            if (start_point.y < (transform.position.y + col_points[i].y))
                                { col_points[i] = start_point - (Vector2) transform.position; }
                            else
                                { col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x; }
                        }
                        else
                        {
                            Debug.Log("hi2");
                            if (start_point.y > (transform.position.y + col_points[i].y))
                                { col_points[i] = start_point - (Vector2)transform.position; }
                            else
                                { col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x; }
                        }
                    }
                    else // If point is closer to ending intersection point
                    {
                        if (end_point.y < start_point.y)
                        {
                            Debug.Log("hi3");
                            if (end_point.y < (transform.position.y + col_points[i].y))
                                { col_points[i] = end_point - (Vector2)transform.position; }
                            else
                                { col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x; }
                        }
                        else
                        {
                            Debug.Log("hi4");
                            if (end_point.y < (transform.position.y + col_points[i].y))
                                { col_points[i] = end_point - (Vector2)transform.position; }
                            else
                            { col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x; }
                        }
                    }
                }
            }
        }
        else if (start.y >= end.y) // Case 2: Starting point is above end point regardless of x-position
        {
            for (int i = 0; i < col_points.Length; i++)
            {
                // Move all points to right of line along line or at start_point / end_point
                if ((transform.position.x + col_points[i].x) > ((transform.position.y + col_points[i].y - intercept) / slope))
                {
                    // Calculate how far each point is to the points of intersection to check whether to put along line or intersection point
                    float start_dist = Vector2.Distance(start_point, (Vector2)transform.position + col_points[i]);
                    float end_dist = Vector2.Distance(end_point, (Vector2)transform.position + col_points[i]);

                    // If point is closer to starting intersection point
                    if (start_dist <= end_dist)
                    {
                        if (start_point.y < end_point.y)
                        {
                            if (start_point.y > (transform.position.y + col_points[i].y))
                            {
                                col_points[i] = start_point - (Vector2)transform.position;
                            }
                            else
                            {
                                col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x;
                            }
                        }
                        else
                        {
                            if (start_point.y <= (transform.position.y + col_points[i].y))
                            {
                                col_points[i] = start_point - (Vector2)transform.position;
                            }
                            else
                            {
                                col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x;
                            }
                        }
                    }
                    else // If point is closer to ending intersection point
                    {
                        if (end_point.y > start_point.y)
                        {
                            if (end_point.y > (transform.position.y + col_points[i].y))
                            {
                                col_points[i] = end_point - (Vector2)transform.position;
                            }
                            else
                            {
                                col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x;
                            }
                        }
                        else
                        {
                            if (end_point.y < (transform.position.y + col_points[i].y))
                            {
                                col_points[i] = end_point - (Vector2)transform.position;
                            }
                            else
                            {
                                col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x;
                            }
                        }
                    }
                }
            }
        }
        else if (start.y < end.y) // Case 3: Starting point is below end point regardless of x-position
        {
            for (int i = 0; i < col_points.Length; i++)
            {
                // Move all points to right of line along line or at start_point / end_point
                if ((transform.position.x + col_points[i].x) <= ((transform.position.y + col_points[i].y - intercept) / slope))
                {
                    // Calculate how far each point is to the points of intersection to check whether to put along line or intersection point
                    float start_dist = Vector2.Distance(start_point, (Vector2)transform.position + col_points[i]);
                    float end_dist = Vector2.Distance(end_point, (Vector2)transform.position + col_points[i]);

                    // If point is closer to starting intersection point
                    if (start_dist <= end_dist)
                    {
                        if (start_point.y < end_point.y)
                        {
                            if (start_point.y > (transform.position.y + col_points[i].y))
                            {
                                col_points[i] = start_point - (Vector2)transform.position;
                            }
                            else
                            {
                                col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x;
                            }
                        }
                        else
                        {
                            if (start_point.y <= (transform.position.y + col_points[i].y))
                            {
                                col_points[i] = start_point - (Vector2)transform.position;
                            }
                            else
                            {
                                col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x;
                            }
                        }
                    }
                    else // If point is closer to ending intersection point
                    {
                        if (end_point.y > start_point.y)
                        {
                            if (end_point.y >= (transform.position.y + col_points[i].y))
                            {
                                col_points[i] = end_point - (Vector2)transform.position;
                            }
                            else
                            {
                                col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x;
                            }
                        }
                        else
                        {
                            if (end_point.y < (transform.position.y + col_points[i].y))
                            {
                                col_points[i] = end_point - (Vector2)transform.position;
                            }
                            else
                            {
                                col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x;
                            }
                        }
                    }
                }
            }
        }

        /*
        // Compare all collider points with calculated line
        for (int i = 0; i < col_points.Length; i++)
        {
            // Move sprite mask to appropriate position
            mask.transform.position = new Vector2((transform.position.y - intercept) / slope, transform.position.y);

            
            if (angle > 80 && angle < 110)
            {
                // Move all points to right of line along the line
                if ((transform.position.x + col_points[i].x) > ((transform.position.y + col_points[i].y - intercept) / slope))
                {
                    col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x;
                }
            }
            else // Move all points above line along line
            {
                if (col_points[i].y > (slope * col_points[i].x + intercept))
                {
                    col_points[i].y = slope * col_points[i].x + intercept;
                }
            }
           
        }

        // Repeat process for copied object
        for (int j = 0; j < col_points_copy.Length; j++)
        {
            copy_mask.transform.position = new Vector2((transform.position.y - intercept) / slope, transform.position.y);

            if (angle > 80 && angle < 110)
            {
                // Move all points to left of line along the line (for the copy)
                if ((transform.position.x + col_points_copy[j].x) < ((transform.position.y + col_points_copy[j].y - intercept) / slope))
                {
                    col_points_copy[j].x = ((transform.position.y + col_points_copy[j].y - intercept) / slope) - transform.position.x;
                }
            }
            else
            {
                if (col_points_copy[j].y < (slope * col_points_copy[j].x + intercept))
                {
                    col_points_copy[j].y = slope * col_points_copy[j].x + intercept;
                }
            }
        }
        */

        /*
        // Assign new points to collider
        pc.points = col_points;
        pc_copy.points = col_points_copy;

        // Set sprite mask active
        mask.SetActive(true);
        copy_mask.SetActive(true);

        // Rotate sprite mask appropriately
        mask.GetComponent<Rigidbody2D>().rotation = angle;
        copy_mask.GetComponent<Rigidbody2D>().rotation = angle + 180;
        */
        pc.points = col_points;
        rb.gravityScale = 1;
        pc.isTrigger = false;
        
        yield return new WaitForSeconds(0f);
    }
}
