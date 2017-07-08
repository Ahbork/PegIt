using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskToggle : MonoBehaviour {

    Image maskImg;
    public Player player;

    private void Awake()
    {
        maskImg = GetComponent<Image>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Peg")
        {
            Peg peg = collision.GetComponent<Peg>();

            if (player.CheckPegCompatibility(peg))
            {
                maskImg.enabled = true;
            }
            else
            {
                maskImg.enabled = false;
            }
        }
    }
}
