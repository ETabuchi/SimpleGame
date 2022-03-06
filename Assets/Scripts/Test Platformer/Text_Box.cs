using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script pops up a text box and has all of its
// dialogue pop up. Text box will disappear if you click
// on it or press any key once text is fully finished.
public class Text_Box : MonoBehaviour
{
    // GameObject Components
    private Image background;
    private Text message;

    // Message to animate
    public string text;
    public float char_gap;

    void Start()
    {
        background = transform.GetChild(0).GetComponent<Image>();
        message = transform.GetChild(1).GetComponent<Text>();
        char_gap = 0.075f;

        StartCoroutine(animateMessage());
    }

    void FixedUpdate()
    {
        // Animate text
    }

    IEnumerator animateMessage()
    {
        int i = 0;

        while (message.text != text && i < text.Length)
        {
            if (text.Length != i + 1)
            {
                if (text[i] == '\\' && text[i + 1] == 'n')
                {
                    message.text += '\n';
                    i++;
                }
                else
                {
                    message.text += text[i];
                }
            }
            else
            {
                message.text += text[i];
            }

            if (text[i] != ' ')
            {
                yield return new WaitForSeconds(char_gap);
            }

            i++;
        }
    }
}
