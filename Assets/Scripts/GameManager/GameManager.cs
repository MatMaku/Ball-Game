using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public float spawnChance; // Entre 0 y 1
}

[System.Serializable]
public class SpawnPhase
{
    public float startDistance;
    public float endDistance;
    public float spawnInterval;
    public List<EnemySpawnData> enemies;
}

public class GameManager : MonoBehaviour
{
    [Header("Enemy Spawn")]
    public Transform[] spawnPoints;
    public List<SpawnPhase> spawnPhases;
    private SpawnPhase currentPhase;
    private Coroutine spawnRoutine;

    [Header("Distance Tracker")]
    public TextMeshProUGUI DistanciaUI;
    public float distanciaRecorrida = 0f;
    public float velocidad = 10f;

    private void Awake()
    {
        Time.timeScale = 1f;
    }
    private void Start()
    {
        spawnRoutine = StartCoroutine(SpawnEnemiesRoutine());
    }

    private void Update() 
    {
        distanciaRecorrida += velocidad * Time.deltaTime;
        DistanciaUI.text = Mathf.RoundToInt(distanciaRecorrida).ToString() + " KM";
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            UpdateCurrentPhase();

            if (currentPhase != null && currentPhase.enemies.Count > 0)
            {
                TrySpawnEnemy();
                yield return new WaitForSeconds(currentPhase.spawnInterval);
            }
            else
            {
                yield return null;
            }
        }
    }

    void UpdateCurrentPhase()
    {
        foreach (var phase in spawnPhases)
        {
            if (distanciaRecorrida >= phase.startDistance && distanciaRecorrida < phase.endDistance)
            {
                currentPhase = phase;
                return;
            }
        }

        currentPhase = null;
    }

    void TrySpawnEnemy()
    {
        float rand = Random.value;
        float cumulative = 0f;

        foreach (var data in currentPhase.enemies)
        {
            cumulative += data.spawnChance;
            if (rand <= cumulative)
            {
                int spawnIndex = Random.Range(0, spawnPoints.Length);
                Instantiate(data.enemyPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
                break;
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
