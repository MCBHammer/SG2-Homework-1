using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleHelper
{
    public static ParticleSystem particlePlay(ParticleSystem p)
    {
        //create
        GameObject particleObject = new GameObject("ParticleSpawn");
        ParticleSystem particleSource = particleObject.AddComponent<ParticleSystem>();
        //configure
        particleSource = p;
        //activate
        particleSource.Play();
        Object.Destroy(particleObject, p.main.duration);
        //return in case other things need it
        return particleSource;
    }
}
