using UnityEngine;
using System.Collections;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance { get; private set; }

    public GameObject[] StartPrefabs;

    public int Score => m_Score;
    public static bool START_GAME = false;

    private int m_Score = 0;
    private int m_killedCount = 0;
    private int m_totalEnemyCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (var prefab in StartPrefabs)
        {
            Instantiate(prefab);
        }

        PoolSystem.Create();
    }

    void Start()
    {
        MainMenuUI.Instance.Display();

    }

    public void StartGame()
    {
        START_GAME = true;
        Controller.Instance.DisplayWeapon(true);

        // 🔥 Đếm enemy trong scene dựa trên tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        m_totalEnemyCount = enemies.Length;
        Debug.Log(enemies);
        Debug.Log($"[GameSystem] Total enemies at start: {m_totalEnemyCount}");
    }

    public void TargetDestroyed(int score)
    {
        m_Score += score;
        m_killedCount++;

        GameSystemInfo.Instance.UpdateScore(m_Score);

        Debug.Log($"[GameSystem] Enemy killed. {m_killedCount}/{m_totalEnemyCount}");

        if (m_killedCount >= m_totalEnemyCount)
        {
            StartCoroutine(WinGame());
        }
    }

    IEnumerator WinGame()
    {
        yield return new WaitForSeconds(2);

        START_GAME = false;
        Controller.Instance.DisplayWeapon(false);
        Controller.Instance.DisplayCursor(true);

        FinalScoreUI.Instance.Display();
        GameSystemInfo.Instance.gameObject.SetActive(false);
        WeaponInfoUI.Instance.gameObject.SetActive(false);

        Debug.Log("[GameSystem] 🎉 YOU WIN!");
    }
}
