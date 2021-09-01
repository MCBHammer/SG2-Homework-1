using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    [SerializeField] Player player;
    Text text;
    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void FixedUpdate()
    {
        text.text = "Treasure: " + player.TreasureAmount;
    }
}
