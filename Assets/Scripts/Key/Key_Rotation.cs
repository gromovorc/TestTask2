using UnityEngine;

public class Key_Rotation : MonoBehaviour
{
    private void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, 30 * Time.deltaTime);
    }
}
