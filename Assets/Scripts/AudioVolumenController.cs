using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolumenController : MonoBehaviour
{
    //Referencia al audio source del objeto
    private AudioSource audioSource;
    //Nivel de volumen actual
    private float currentAudioLevel;
    //Se va a tomar como el volumen maximo que alcanzará esta pista
    public float defaultAudioLevel;

    // Start is called before the first frame update
    void Start()
    {
        //Inicializamos la variable
        audioSource = GetComponent<AudioSource>();
    }

    public void SetAudioLevel(float newVolumen)
    {
        //Como medida de seguridad porque puede suceder que al ser
        //Una pista muy pesada no alcance a subirse en el Start()
        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        //Calculamos el nuevo volumen
        currentAudioLevel = defaultAudioLevel * newVolumen;
        //Modificamos el volumen de la pista
        audioSource.volume = currentAudioLevel;
    }
}
