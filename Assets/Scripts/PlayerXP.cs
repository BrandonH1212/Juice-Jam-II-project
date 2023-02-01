using Newtonsoft.Json;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public struct XpInfo
{
    public int currentLevel;
    public int currentXP;
    public int XpRequired;
}


public class PlayerXP : MonoBehaviour
{
    private static PlayerXP instance;
    public static PlayerXP Instance { get { return instance; } }

    private int _xpRequired = 100;
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
        return new XpInfo() { currentLevel = currentLevel, currentXP = currentXP, XpRequired = _xpRequired };
    }



    public void AddXP(int amount)
    {
        currentXP += amount;
        while (currentXP >= _xpRequired)
        {
            currentLevel++;
            onLevelUp.Invoke();
            currentXP = 0;
            _xpRequired = (int)(100 * math.pow(1.1, currentLevel));

        }
        onXPChanged.Invoke();
        UIXpBar.Instance.UpdateBar(GetXPInfo());
    }
}