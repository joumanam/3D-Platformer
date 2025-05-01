using TMPro;
using UnityEngine;
public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int coinCount = 0;
    public TMP_Text coinText;
    public RectTransform targetUI;
    public float duration = 0.5f;

    private RectTransform rectTransform;
    private Vector3 worldStartPos;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateCoinUI();
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        // I want to do the fly to UI logic here
        UpdateCoinUI();
    }

    void UpdateCoinUI()
    {
        if (coinText != null)
            coinText.text = "Coins: " + coinCount;
    }

}
