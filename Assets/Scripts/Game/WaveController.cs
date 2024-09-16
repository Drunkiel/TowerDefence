using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave
{
    public List<Phase> phases = new();
    public float nextPhaseDelay = 2; // Delay before the next phase starts
}

[Serializable]
public class Phase
{
    public List<GameObject> enemiesToSpawn = new();
    public float spawnDelay = 1;
}

public class WaveController : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    public List<Wave> waves = new(); // List of all waves
    public bool canGoToNextWave;
    public WaveUI _waveUI;

    [SerializeField] private List<GameObject> spawnedEnemies = new();

    private void Start()
    {
        // Setting the start point for waves
        startPoint = PathController.instance.pathCells[0].transform;
        StartCoroutine(StartRound());
    }

    private void FixedUpdate()
    {
        // Removing dead enemies from the list
        if (spawnedEnemies.Count > 0)
        {
            for (int i = 0; i < spawnedEnemies.Count; i++)
            {
                if (spawnedEnemies[i] == null)
                {
                    spawnedEnemies.RemoveAt(i);
                    break;
                }
            }
        }
    }

    private IEnumerator StartRound()
    {
        // Iterate through all waves
        for (int i = 0; i < waves.Count; i++)
        {
            _waveUI.waveText.text = $"Wave\n{i + 1}/{waves.Count}";

            // Start all phases in the wave
            yield return StartCoroutine(WavePhases(waves[i]));

            // After all phases are done, go to the next wave immediately
            canGoToNextWave = false;
        }

        // When all waves are finished, display a message
        Debug.Log("All waves have been completed!");
    }

    private IEnumerator WavePhases(Wave wave)
    {
        // Iterate through all phases
        for (int i = 0; i < wave.phases.Count; i++)
        {
            _waveUI.phaseText.text = $"Phase\n{i + 1}/{wave.phases.Count}";
            
            // Spawn enemies in the current phase
            yield return StartCoroutine(SpawnEnemies(wave.phases[i]));

            // Wait until all enemies from the current phase are defeated
            yield return new WaitUntil(() => spawnedEnemies.Count == 0);
            
            _waveUI.StartCoroutine(_waveUI.StartTimer(wave.nextPhaseDelay));

            // Delay before starting the next phase
            yield return new WaitForSeconds(wave.nextPhaseDelay);
        }

        // After all phases are done, allow transition to the next wave
        canGoToNextWave = true;
    }

    private IEnumerator SpawnEnemies(Phase phase)
    {
        // Spawn enemies for the current phase
        for (int i = 0; i < phase.enemiesToSpawn.Count; i++)
        {
            GameObject enemy = Instantiate(phase.enemiesToSpawn[i], startPoint.position + new Vector3(0, 0.2f, 0), Quaternion.identity);
            spawnedEnemies.Add(enemy); // Add the spawned enemy to the list of active enemies
            yield return new WaitForSeconds(phase.spawnDelay); // Delay between enemy spawns
        }
    }
}
