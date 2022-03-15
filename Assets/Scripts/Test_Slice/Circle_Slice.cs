using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Slice a 2D circle in half 
public class Circle_Slice : MonoBehaviour
{
    private GameObject mask;      // sprite mask to hide away cuts
    private PolygonCollider2D pc; // manipulates collider points
    private Rigidbody2D rb;       // rigidbody for physics

    void Awake()
    {
        mask = transform.GetChild(0).gameObject;
        pc = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //StartCoroutine(Instantiate(gameObject, transform.position, Quaternion.identity).GetComponent<Circle_Slice>().Cut(x.transform.position, y.transform.position, 0));
            //StartCoroutine(Cut(x.transform.position, y.transform.position, 1));
        }
    }

    // Cuts an object in half 
    // Mode: 0 - Cut up and leave everything up / left visible
    // Mode: 1 - Cut down and leave everything down / right visible
    public IEnumerator Cut(Vector2 start, Vector2 end, int mode)
    {
        // Points of intersection
        Vector2 start_point = Physics2D.Raycast(start, end - start, LayerMask.GetMask("Cuttable")).point;
        Vector2 end_point = Physics2D.Raycast(end, start - end, LayerMask.GetMask("Cuttable")).point;

        // Instantiate a new mask to hide cut part
        GameObject cut_mask = Instantiate(mask, mask.transform.position, Quaternion.identity, this.transform);
        cut_mask.SetActive(true);

        // Calculate line statistics between start and end points (y = mx + b, angle between points)
        float slope = (start.y - end.y) / (start.x - end.x);
        float intercept = start.y - slope * start.x;
        float angle = Mathf.Rad2Deg * Mathf.Atan2((start.y - end.y), (start.x - end.x));

        // Get points of PolygonCollider2D
        Vector2[] col_points = pc.points;

        // Check the type of line created from stats
        if ((start.y - end.y) >= -0.1f && (start.y - end.y) <= 0.01f) // Case 1: Horizontal Cut
        {
            // Change position and rotate
            cut_mask.transform.position = new Vector2(transform.position.x, start.y);
            cut_mask.transform.rotation = Quaternion.Euler(0, 0, mode * 180);

            // Check all points along collider
            for (int i = 0; i < col_points.Length; i++)
            {
                // Move all points to left / right of line along the line or at start / end point
                switch (mode)
                {
                    // Cut up
                    case 0:
                        if (((transform.position.y + col_points[i].y) < start_point.y))
                        {
                            col_points[i].y = start_point.y - transform.position.y;
                        }
                        break;
                    // Cut down
                    case 1:
                        if (((transform.position.y + col_points[i].y) >= start_point.y))
                        {
                            col_points[i].y = start_point.y - transform.position.y;
                        }
                        break;
                    // Default: cut down
                    default:
                        if (((transform.position.y + col_points[i].y) >= start_point.y))
                        {
                            col_points[i].y = start_point.y - transform.position.y;
                        }
                        break;
                }
            }
        }
        else if ((start.x - end.x) >= -0.01f && (start.x - end.x) <= 0.01f) // Case 2: Vertical Cut
        {
            // Change position and rotate
            cut_mask.transform.position = new Vector2(start.x, transform.position.y);
            cut_mask.transform.rotation = Quaternion.Euler(0, 0, 180 * mode + 90); // 0 - up, 1 - down

            // Check all points along collider
            for (int i = 0; i < col_points.Length; i++)
            {
                // Move all points to left / right of line along the line or at start / end point
                switch (mode)
                {
                    // Cut up
                    case 0:
                        if (((transform.position.x + col_points[i].x) >= start_point.x))
                        {
                            col_points[i].x = start_point.x - transform.position.x;
                        }
                        break;
                    // Cut down
                    case 1:
                        if (((transform.position.x + col_points[i].x) <= start_point.x))
                        {
                            col_points[i].x = start_point.x - transform.position.x;
                        }
                        break;
                    // Default: cut down
                    default:
                        if (((transform.position.x + col_points[i].x) <= start_point.x))
                        {
                            col_points[i].x = start_point.x - transform.position.x;
                        }
                        break;
                }
            }
        }
        else // Case 3, angular cut
        {
            // Change position and rotate
            cut_mask.transform.position = new Vector2((start_point.x + end_point.x) / 2, (start_point.y + end_point.y) / 2);

            if (start.x < end.x)
            {
                cut_mask.transform.rotation = Quaternion.Euler(0, 0, angle + 180 * (mode - 1));
            }
            else
            {
                cut_mask.transform.rotation = Quaternion.Euler(0, 0, angle + 180 * mode);
            }

            // Dictionary stores all points in circle that are viable to manipulate (depending on cut mode)
            Dictionary<int, float> points = new Dictionary<int, float>();
            int index = 0;

            if (mode == 0) // Cut up
            {
                for (int i = 0; i < col_points.Length; i++)
                {
                    if ((transform.position.y + col_points[i].y) < (transform.position.x + col_points[i].x) * slope + intercept)
                    {
                        points.Add(i, Vector2.Distance((Vector2)transform.position + col_points[i], (Vector2)end_point));
                    }
                }
            }
            else // Cut down
            {
                for (int i = 0; i < col_points.Length; i++)
                {
                    if ((transform.position.y + col_points[i].y) > (transform.position.x + col_points[i].x) * slope + intercept)
                    {
                        points.Add(i, Vector2.Distance((Vector2)transform.position + col_points[i], (Vector2)end_point));
                    }
                }
            }

            foreach (KeyValuePair<int, float> point in points.OrderBy(key => key.Value))
            {
                if (index == 0) // Set first point closest to end point to its position
                {
                    col_points[point.Key] = end_point - (Vector2)transform.position;
                }
                else if (index == points.Count - 1)
                {
                    col_points[point.Key] = start_point - (Vector2)transform.position;
                }
                else
                {
                    col_points[point.Key] = (start_point + end_point) / 2 - (Vector2)transform.position;
                    end_point = (start_point + end_point) / 2;
                }

                index++;
            }
        }

        pc.points = col_points;
        rb.gravityScale = 1;
        pc.isTrigger = false;

        yield return new WaitForSeconds(0f);
    }
}
