using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShooter : MonoBehaviour
{
    public float speedAttack;

    public GameObject[] objects;

    public Transform target;

    public Transform camera;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);

        while(true){
            Instantiate(objects[Random.Range(0, objects.Length)], transform.position, transform.rotation);
            yield return new WaitForSeconds(speedAttack);
        }
    }

    void Update(){
        transform.position = camera.position + Vector3.up * -2f;
        transform.LookAt(target.position);
    }
}
