using UnityEngine;

public class HardShadow : MonoBehaviour
{
    public Transform shadowObject;  // Assign the object that will be the shadow

    void LateUpdate()
    {
        if (shadowObject != null)
        {
            // Position the shadow directly beneath the shadowObject
            Vector3 shadowPos = shadowObject.position;
            shadowPos.y = 0.1f;  // Adjust this value to control the height of the shadow above ground
            transform.position = shadowPos;

            // Match rotation with shadowObject
            transform.rotation = Quaternion.Euler(90f, shadowObject.eulerAngles.y, 0f);
        }
    }
}
