using UnityEngine;

public class MoveFromInput : MonoBehaviour
{
    public float speed;
    public GameObject relativeToObject;
    private Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 worldSpaceRelativeVector = relativeToObject.transform.TransformVector(new Vector3(moveHorizontal, 0, moveVertical)).normalized;
        rigidBody.AddForce(worldSpaceRelativeVector * speed);
    }
}