using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Array con las pistas de audio de las diferentes escenas
    //Es importante llevar un orden como si de un cd se tratara
    public AudioSource[] audioTracks;
    //Pista actual en reproduccion
    public int currentTrack;
    //Variable que me indica si debe o no reproducirse la pista 
    public bool audioCanBePlayed;

    // Update is called once per frame
    void Update()
    {
        if (audioCanBePlayed)
        {
            if (!audioTracks[currentTrack].isPlaying)
            {
                audioTracks[currentTrack].Play();
            }
        }
    }
    //Metodo que me cambia de pista
    public void PlayNewTrack(int newTrack)
    {
        audioTracks[currentTrack].Stop();
        currentTrack = newTrack;
        audioTracks[currentTrack].Play();
    }

    public void StopSound()
    {
        audioTracks[currentTrack].Stop();

    }
}
