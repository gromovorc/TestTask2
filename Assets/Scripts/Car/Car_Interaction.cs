using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car_Interaction : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Player_Controller _player = 
            collision.gameObject.GetComponent<Player_Controller>();
        if (_player && _player.isWithKey) SceneManager.LoadScene("Loading");
    }
}
