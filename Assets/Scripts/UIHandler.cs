using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : Singleton<UIHandler>
{
    [SerializeField]
    AudioSource audioSource;
    [Header("Animations")]
    [SerializeField]
    AudioClip buttonHighlightSound;

    void SFX_PlayButtonHighlight() => audioSource.PlayOneShot(buttonHighlightSound);
}
