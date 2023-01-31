using UnityEngine;
using UnityEngine.Events;

public struct XpInfo
{
    public int currentLevel;
    public int currentXP;
    public int[] xpThresholds;
}


public class PlayerXP : MonoBehaviour
{
    private static PlayerXP instance;
    public static PlayerXP Instance { get { return instance; } }

    [SerializeField] private int[] xpThresholds = new int[] { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000 };
    [SerializeField] private int startingLevel = 1;
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

    public XpInfo GetXPInfo()
    {
        return new XpInfo() { currentLevel = currentLevel, currentXP = currentXP, xpThresholds = xpThresholds };
    }



    public void AddXP(int amount)
    {
        currentXP += amount;
        print(currentXP);
        while (currentLevel < xpThresholds.Length && currentXP >= xpThresholds[currentLevel])
        {
            currentLevel++;
            onLevelUp.Invoke();
        }
        onXPChanged.Invoke();
        UIXpBar.Instance.UpdateBar(GetXPInfo());
    }
}