using UnityEngine;
using UnityEngine.Events;

public class PlayerXP : MonoBehaviour
{
    private static PlayerXP instance;
    public static PlayerXP Instance { get { return instance; } }

    [SerializeField] int[] xpThresholds = new int[] { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };
    [SerializeField] int startingLevel;
    private int currentLevel;
    private int currentXP;

    public UnityEvent onXPChanged;
    public UnityEvent onLevelUp;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        currentLevel = startingLevel;
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        while (currentLevel < xpThresholds.Length && currentXP >= xpThresholds[currentLevel])
        {
            currentLevel++;
            onLevelUp.Invoke();
        }
        onXPChanged.Invoke();
    }
}