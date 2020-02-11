using UnityEngine;

public class DestroyTimeout : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
