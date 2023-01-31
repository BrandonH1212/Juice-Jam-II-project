using UnityEngine;

public class XPPickup : MonoBehaviour
{
    [SerializeField]
    public int xpAmount = 10;
    [SerializeField]
    public float followDuration = 1.5f;

    private bool PickedUp = false;
    private Transform playerTransform;
    private float timeElapsed = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
        }
    }

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }

    private void FixedUpdate()
    {
        if (playerTransform == null)
        {
            return;
        }

        timeElapsed += Time.deltaTime;
        Vector3 new_position = Vector3.Lerp(transform.position, playerTransform.position, timeElapsed / followDuration);
        transform.position = new Vector3(new_position.x, new_position.y, -1);

        if (timeElapsed >= followDuration && !PickedUp)
        {
            PlayerXP.Instance.AddXP(xpAmount);
            Destroy(gameObject);
            PickedUp = true;
        }
    }
}
