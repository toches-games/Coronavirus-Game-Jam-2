using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolumenManager : MonoBehaviour
{
    //Array de pistas que vamos a manejar su volumen
    private AudioVolumenController[] audios;
    //Volumen maximo
    public float maxVolumeLevel;
    //Volumen actual
    public float currentVolumeLevel;

    // Start is called before the first frame update
    void Start()
    {
        //Asigno el array de todos los objetos de este tipo
        audios = GetComponentsInChildren<AudioVolumenController>();
        //Tambien podemos usar FindObjectsOfType<AudioVolumenController>()

        //Asigno directamente el volumen que habiamos indicado en el
        //defaultAudioLevel
        ChangeGlobalAudioVolume();
    }

    private void Update()
    {
        //DEBEMOS ELIMINAR ESTA LINEA CUANDO HAGAMOS LA PANTALLA DE
        //CONFIGURACION DE VOLUMEN
        ChangeGlobalAudioVolume();
    }

    private void ChangeGlobalAudioVolume()
    {
        //Reviso si el volumen actual es mayor que el maximo y lo acomodo
        //Para no llegar a saturar al usuario
        if(currentVolumeLevel >= maxVolumeLevel)
        {
            currentVolumeLevel = maxVolumeLevel;
        }

        //Cambiamos el nivel de volumen de cada uno de las pistas
        foreach(AudioVolumenController avc in audios)
        {
            avc.SetAudioLevel(currentVolumeLevel);
        }
    }

    public IEnumerator ProgressiveVolumeDown()
    {
        for (float i = 1; i > 0; i -= 0.01f)
        {
            currentVolumeLevel -= i;
            yield return new WaitForSecondsRealtime(0.2f);
        }
    }
}
