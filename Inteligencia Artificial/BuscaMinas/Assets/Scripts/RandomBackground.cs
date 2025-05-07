using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomBackground : MonoBehaviour
{
    public List<Sprite> backgrounds;
    private Image img;

    void Start()
    {
        img = GetComponent<Image>();

        Sprite selectedBackground = backgrounds[Random.Range(0, backgrounds.Count)];
        img.sprite = selectedBackground;
    }
}
