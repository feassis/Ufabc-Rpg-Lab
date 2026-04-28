using UnityEngine;
using UnityEngine.U2D.Animation;

public class FixSpriteSkin : MonoBehaviour
{
    SpriteSkin[] skins;

    void Awake()
    {
        skins = GetComponentsInChildren<SpriteSkin>();
    }

    void LateUpdate()
    {
        foreach (var skin in skins)
        {
            skin.enabled = false;
            skin.enabled = true;
        }
    }
}