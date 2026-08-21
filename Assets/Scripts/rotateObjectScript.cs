using UnityEngine;

public class rotateObjectScript : MonoBehaviour
{
    public void flipObject(float rotationAngle)
    {
        transform.Rotate(0f, 0f, rotationAngle);
    }
    
}

