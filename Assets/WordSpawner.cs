using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSpawner : MonoBehaviour {

    public GameObject wordPrefab;
    public Transform wordCanvas;
    public float spawnHeight = 7f;
    public float spawnRangeX = 5.5f;

    public WordDisplay SpawnWord()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnRangeX, spawnRangeX), 
            spawnHeight, 
            0f
        );

        GameObject wordObj = Instantiate(wordPrefab, randomPosition, Quaternion.identity, wordCanvas);
        WordDisplay wordDisplay = wordObj.GetComponent<WordDisplay>();

        return wordDisplay;
    }

    public void SpawnDisplayMessage(string message)
    {
        // Posición fija para mensajes de nivel
        Vector3 displayPosition = new Vector3(0f, 6f, 0f); 

        GameObject wordObj = Instantiate(wordPrefab, displayPosition, Quaternion.identity, wordCanvas);
        WordDisplay wordDisplay = wordObj.GetComponent<WordDisplay>();
        
        // Crear palabra de solo visualización sin almacenarla en la lista de palabras activas
        wordDisplay.SetWord(message, true);
        
        // Opcional: añadir sonido de notificación aquí
        // AudioSource.PlayClipAtPoint(levelUpSound, transform.position);
    }

    // Método para limpiar palabras que salen de pantalla
    public void CleanupOffscreenWords()
    {
        GameObject[] words = GameObject.FindGameObjectsWithTag("Word");
        foreach (GameObject word in words)
        {
            if (word.transform.position.y < -6f) // Fuera de pantalla
            {
                Destroy(word);
            }
        }
    }
}