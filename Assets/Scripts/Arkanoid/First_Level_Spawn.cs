using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns a basic grid of rectangles for the player to defeat
public class First_Level_Spawn : MonoBehaviour
{
    public GameObject block;

    void Start()
    {
        StartCoroutine(SpawnObjectsBlock(4, 6, 5f, 2f));
    }

    // Spawn a row x col block of rectangular blocks
    IEnumerator SpawnObjectsBlock(int row, int col, float x_offset, float y_offset)
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Instantiate(block, new Vector2(transform.position.x + j * x_offset, transform.position.y - i * y_offset), Quaternion.identity);
            }
        }
        yield return new WaitForSeconds(0f);
    }
}
