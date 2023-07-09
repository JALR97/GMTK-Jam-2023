using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMucis : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        source.Play();
    }

}
