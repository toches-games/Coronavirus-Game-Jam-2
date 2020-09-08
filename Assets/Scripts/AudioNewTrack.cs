using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioNewTrack : MonoBehaviour
{
    //Referencia al audio manager
    private AudioManager manager;
    //Nueva pista a reproducir
    public int newTrackID;
    //Booleano que me indica si debo empezar reproduciendo la pista
    public bool playOnStart;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<AudioManager>();
        if (playOnStart)
        {
            manager.PlayNewTrack(newTrackID);
        }
    }

    //Si entra en la zona cambiamos a la pista
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si el player es quien entra
        if (collision.gameObject.tag.Equals("Player"))
        {
            //Cambiamos la pista
            manager.PlayNewTrack(newTrackID);
            //Desactivamos la zona para que no siga activandose
            gameObject.SetActive(false);
        }
    }

    public void ChangeTrack(int newTrackID)
    {
        manager.PlayNewTrack(newTrackID);
    }

    public void StopSound()
    {
        manager.StopSound();
    }

}
