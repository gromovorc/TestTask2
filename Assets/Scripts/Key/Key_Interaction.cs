using UnityEngine;

public class Key_Interaction : MonoBehaviour { 
    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void OnMouseDown()
    {
        _renderer.material.color = Random.ColorHSV();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Player_Controller _player = collision.gameObject.GetComponent<Player_Controller>();

        if (_player)
        {
            _player.isWithKey = true;

            Destroy(transform.parent.gameObject);
        }
    }
}
