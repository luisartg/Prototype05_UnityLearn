using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int pointValue = 1;
    public int livesLostIfMiss = 0;
    public ParticleSystem explosionPS;

    private GameManager gameManager;
    private Rigidbody targetRb;
    private float minSpeed = 14;
    private float maxSpeed = 18;
    private float torqueRange = 10;
    private float xRange = 4;
    private float ySpawnPos = -6;


    // Start is called before the first frame update
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ApplyInitialForce();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void ApplyInitialForce()
    {
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        transform.position = RandomSpawnPos();
    }

    private Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    private float RandomTorque()
    {
        return Random.Range(-torqueRange, torqueRange);
    }

    private Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }

    private void OnMouseOver()
    {
        if (gameManager.IsGameActive() && Input.GetMouseButton(0))
        {
            Destroy(gameObject);
            Instantiate(explosionPS, transform.position, explosionPS.transform.rotation);
            gameManager.AddToScore(pointValue);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.RemoveLifeBy(livesLostIfMiss);
        Destroy(gameObject);
    }
}
