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
    public int z = 0;

    void Awake()
    {
        pc = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        mask = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpriteSlice(x.transform.position, y.transform.position);
        }
    }

    // Creates copy of object and uses SliceSpriteUp and SliceSpriteDown to cut both objects
    void SpriteSlice(Vector2 start, Vector2 end)
    {
        // Create copy of object
        //StartCoroutine(Instantiate(gameObject, transform.position, Quaternion.identity).GetComponent<Sprite_Slice>().SliceSpriteUp(start, end));
        StartCoroutine(SliceSpriteDown(start, end));
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

        GameObject new_mask = Instantiate(mask, new Vector3((transform.position.y - intercept) / slope, transform.position.y), Quaternion.identity, transform);
        new_mask.SetActive(true);
        new_mask.transform.rotation = Quaternion.Euler(0, 0, angle + 180);

        // Case 1: Starting point is the rightmost point 
        if (start.x > end.x)
        {
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
                            if (start_point.y < (transform.position.y + col_points[i].y))
                            { col_points[i] = start_point - (Vector2)transform.position; }
                            else if (start_point.x < (transform.position.y + col_points[i].y))
                            { col_points[i] = start_point - (Vector2)transform.position; }
                            else
                            { col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x; }
                        }
                        else
                        {
                            Debug.Log("hi");
                            if (start_point.y < (transform.position.y + col_points[i].y))
                            { col_points[i] = start_point - (Vector2)transform.position; }
                            else
                            { col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x; }
                        }
                    }
                    else // If point is closer to ending intersection point
                    {
                        if (end_point.y > start_point.y)
                        {
                            if (end_point.y > (transform.position.y + col_points[i].y))
                            { col_points[i] = end_point - (Vector2)transform.position; }
                            else
                            { col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x; }
                        }
                        else
                        {
                            if (end_point.y > (transform.position.y + col_points[i].y))
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
                            if (start_point.y < (transform.position.y + col_points[i].y))
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
        // Get points of PolygonCollider2D
        Vector2[] col_points = pc.points;

        // Calculate slope, y-intercept, and angle between two points
        float slope = (start.y - end.y) / (start.x - end.x);
        float intercept = start.y - slope * start.x;
        float angle = Mathf.Rad2Deg * Mathf.Atan2((start.y - end.y), (start.x - end.x));

        // Get two farmost intersection points
        Vector2 start_point = Physics2D.Raycast(start, end - start, LayerMask.GetMask("Cuttable")).point;
        Vector2 end_point = Physics2D.Raycast(end, start - end, LayerMask.GetMask("Cuttable")).point;

        GameObject new_mask;

        if ((start.y - end.y) >= -0.1f && (start.y - end.y) <= 0.01f) // Horizontal Cut
        {
            new_mask = Instantiate(mask, new Vector3(transform.position.x, start.y), Quaternion.identity, transform);
        }
        else if ((start.x - end.x) >= -0.01f && (start.x - end.x) <= 0.01f) // Vertical Cut
        {
            new_mask = Instantiate(mask, new Vector3(transform.position.x + start.x, transform.position.y), Quaternion.identity, transform);
        }
        else
        {
            new_mask = Instantiate(mask, new Vector3((transform.position.y - intercept) / slope, transform.position.y - start.y), Quaternion.identity, transform);
        }

        new_mask.SetActive(true);
        new_mask.transform.rotation = Quaternion.Euler(0, 0, angle);

        x.transform.position = start_point;
        y.transform.position = end_point;

        // Case 1: Starting point is the rightmost point 
        if (start.x > end.x)
        {
            for (int i = 0; i < col_points.Length; i++)
            {
                // Move all points under line along line or at start_point / end_point
                if ((transform.position.x + col_points[i].x) > ((transform.position.y + col_points[i].y - intercept) / slope))
                {
                    // Calculate how far each point is to the points of intersection to check whether to put along line or intersection point
                    float start_dist = Vector2.Distance(start_point, (Vector2)transform.position + col_points[i]);
                    float end_dist = Vector2.Distance(end_point, (Vector2)transform.position + col_points[i]);

                    // If point is closer to starting intersection point
                    if (start_dist < end_dist)
                    {
                        if (start_point.y > end_point.y)
                        {
                            Debug.Log("hi");
                            if (start_point.y > (transform.position.y + col_points[i].y))
                                { col_points[i] = start_point - (Vector2) transform.position; }
                            else if (start_point.x < (transform.position.x + col_points[i].x))
                                { col_points[i] = start_point - (Vector2)transform.position; }
                            else
                                { col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x; }
                        }
                        else
                        {
                            Debug.Log("h");
                            if (start_point.y > (transform.position.y + col_points[i].y))
                                { col_points[i] = start_point - (Vector2)transform.position; }
                            else
                                { col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x; }
                        }
                    }
                    else // If point is closer to ending intersection point
                    {
                        Debug.Log("hi");
                        if (end_point.y < start_point.y)
                        {
                            if (end_point.y > (transform.position.y + col_points[i].y))
                                { col_points[i] = end_point - (Vector2)transform.position; }
                            else
                                { col_points[i].x = ((transform.position.y + col_points[i].y - intercept) / slope) - transform.position.x; }
                        }
                        else
                        {
                            if (end_point.y > (transform.position.y + col_points[i].y))
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

        pc.points = col_points;
        rb.gravityScale = 1;
        pc.isTrigger = false;
        
        yield return new WaitForSeconds(0f);
    }
}
