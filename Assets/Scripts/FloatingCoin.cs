using UnityEngine;

public class FloatingCoin : MonoBehaviour
{
    public float rotationSpeed = 90f; // degrees per second
    public float floatAmplitude = 0.25f; // how high it floats
    public float floatFrequency = 1f; // how fast it floats
    public int value = 1;
    public int coinSoundIndex = 0;
    public GameObject flyCoinUIPrefab;
    public RectTransform uiTarget;
    public Canvas uiCanvas;


    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Rotate the coin
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        // Float up and down
        float newY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = startPosition + new Vector3(0f, newY, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject uiCoin = Instantiate(flyCoinUIPrefab, uiCanvas.transform);
            var flyScript = uiCoin.GetComponent<CoinFlyEffect>();
            flyScript.targetUI = uiTarget;
            flyScript.StartFly(transform.position, uiCanvas);
            SoundManager.PlaySound(SoundType.COLLECTCOIN, 0.45f, false, coinSoundIndex);
            //CoinManager.Instance.AddCoin(value);
            Destroy(gameObject);
        }
    }
}
