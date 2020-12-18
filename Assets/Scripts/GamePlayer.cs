using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    private int state = 0;

    public int State
    {
        get { return state; }
        set { state = value; }
    }

    public void JumpPlayer()
    {
        if(state == 0)
        {
            state = 1;
            StartCoroutine(Jump());
        }
    }

    private IEnumerator Jump()
    {
        float jumpPow = 20;
        while(true)
        {
            this.transform.localPosition += new Vector3(0f, jumpPow, 0f);
            jumpPow -= 0.6f;
            if (this.transform.localPosition.y < -210)
            {
                this.transform.localPosition = new Vector3(-464f, -210f, 0f);
                state = 0;
                break;
            }
            yield return null;
        }

        yield return null;
    }
}
