using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwapper : MonoBehaviour
{
    [Header("Idle Mask")]
    public GameObject MaskIdle;
    public Sprite idleDefault;
    public Sprite idleAlternate;

    [Header("Walk Mask")]
    public GameObject MaskWalk;
    public Sprite walkDefault;
    public Sprite walkAlternate;

    private SpriteRenderer idleRenderer;
    private SpriteRenderer walkRenderer;

    private bool isAlternate = false;

    void Start()
    {
        idleRenderer = MaskIdle.GetComponent<SpriteRenderer>();
        walkRenderer = MaskWalk.GetComponent<SpriteRenderer>();

        idleRenderer.sprite = idleDefault;
        walkRenderer.sprite = walkDefault;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ToggleSprites();
            SoundEffectManager.Play("MaskSwap");
        }
    }

    void ToggleSprites()
    {
        isAlternate = !isAlternate;

        idleRenderer.sprite = isAlternate ? idleAlternate : idleDefault;
        walkRenderer.sprite = isAlternate ? walkAlternate : walkDefault;
    }
}