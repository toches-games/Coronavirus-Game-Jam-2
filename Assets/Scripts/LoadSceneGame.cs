using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneGame : MonoBehaviour
{
    void OnEnable(){
        SceneManager.LoadScene(1);
    }
}
