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

    private int _xpRequired = 10;
    [SerializeField] private int startingLevel = 1;
    private int currentLevel;
    private int currentXP;
    private PlayerController _playerController;
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
        _playerController = FindObjectOfType<PlayerController>();
        currentLevel = startingLevel;
        
    }

    public XpInfo GetXPInfo()
    {
        return new XpInfo() { currentLevel = currentLevel, currentXP = currentXP, XpRequired = _xpRequired };
    }

    public void LevelUp()
    {
        currentLevel++;
        onLevelUp.Invoke();
        _xpRequired = (int)(_xpRequired * math.pow(1.1, currentLevel));
        print(_xpRequired);
        currentXP = 0;
        onXPChanged.Invoke();
        UIXpBar.Instance.UpdateBar(GetXPInfo());
        _playerController.AcquireNewCard(CardRarity.Common);
    }


    public void AddXP(int amount)
    {
        currentXP += amount;
        while (currentXP >= _xpRequired)
        {
            LevelUp();

        }
        onXPChanged.Invoke();
        UIXpBar.Instance.UpdateBar(GetXPInfo());
    }
}