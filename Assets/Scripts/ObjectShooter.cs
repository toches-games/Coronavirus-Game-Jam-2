using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectShooter : MonoBehaviour
{
    [Header("Roof Settings")]
    public float speedAttack;
    public int initHealth;

    public Slider sliderHealth;

    public GameObject[] objects;

    public Transform target;

    public Transform camera;

    AudioSource crack;

    int currentHealth;

    void Awake(){
        crack = GetComponents<AudioSource>()[0];
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        currentHealth = initHealth;
        sliderHealth.gameObject.SetActive(true);

        yield return new WaitForSeconds(8f);

        while(true){
            if(currentHealth <= initHealth){
                Object temp = Instantiate(objects[Random.Range(0, objects.Length)], transform.position, transform.rotation).GetComponent<Object>();
                temp.objectShooter = this;
                
                yield return new WaitForSeconds(speedAttack);
            }

            if(currentHealth <= 60){
                Object temp = Instantiate(objects[Random.Range(0, objects.Length)], transform.position + transform.right * 5f, transform.rotation).GetComponent<Object>();
                temp.objectShooter = this;
                
                yield return new WaitForSeconds(speedAttack);
            }

            if(currentHealth <= 40){
                Object temp = Instantiate(objects[Random.Range(0, objects.Length)], transform.position + transform.right * -5f, transform.rotation).GetComponent<Object>();
                temp.objectShooter = this;
                
                yield return new WaitForSeconds(speedAttack);
            }

            yield return new WaitForSeconds(1f);

        }
    }

    void Update(){
        transform.position = camera.position + Vector3.up * -2f;
        transform.LookAt(target.position);
    }

    public void PlayCrak(){
        crack.Play();
    }

    public void Damage(int damage){
        currentHealth -= damage;
        sliderHealth.value = currentHealth;

        if(currentHealth <= 0){
            GameManager.sharedInstance.NextLevel(6);
            Destroy(gameObject);
        }
    }
}
