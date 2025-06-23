using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Necesario para Action

public class WordGenerator : MonoBehaviour {
    // Niveles de aprendizaje con sus respectivas teclas
    private static Dictionary<int, string[]> learningLevels = new Dictionary<int, string[]>() {
        {1, new string[] {"j", "f"}}, // Nivel 1: j y f
        {2, new string[] {"j", "f", "k", "d"}}, // Nivel 2: añade k y d
        {3, new string[] {"j", "f", "k", "d", "g", "h"}}, // Nivel 3: añade g y h
        {4, new string[] {"j", "f", "k", "d", "g", "h", "s", "l"}}, // Nivel 4: añade s y l
        {5, new string[] {"j", "f", "k", "d", "g", "h", "s", "l", "a", ";"}}, // Nivel 5: añade a y ;
    };

    // Palabras de ejemplo para cada nivel
    private static Dictionary<int, string[]> levelWords = new Dictionary<int, string[]>() {
        {1, new string[] {"jf", "fj", "jj", "ff"}},
        {2, new string[] {"jk", "fd", "kf", "dj", "jkfd", "fdjk"}},
        {3, new string[] {"jkg", "fdh", "ghj", "hfk", "jkfdgh", "ghfdjk"}},
        {4, new string[] {"jksl", "fdsl", "sljk", "slfd", "jkfdsl", "slfdjk"}},
        {5, new string[] {"jksla", "fdsl;", "asljk", ";slfd", "jkfdsla", "slfdjk;"}}
    };

    private static int currentLevel = 1;
    private static int wordsCompletedInLevel = 0;
    private static int wordsNeededToAdvance = 10; // Número de palabras necesarias para avanzar de nivel

    public static string GetRandomWord() {
        // Obtener palabras del nivel actual
        string[] availableWords = levelWords[currentLevel];
        int randomIndex = UnityEngine.Random.Range(0, availableWords.Length);
        return availableWords[randomIndex];
    }

    public static void WordCompleted() {
        wordsCompletedInLevel++;
        if (wordsCompletedInLevel >= wordsNeededToAdvance && currentLevel < learningLevels.Count) {
            AdvanceLevel();
        }
    }

    public static event Action<int, string[]> OnLevelAdvanced; // Evento que se dispara al avanzar de nivel

    private static void AdvanceLevel() {
        currentLevel++;
        wordsCompletedInLevel = 0;
        // En lugar de Debug.Log, disparamos el evento
        OnLevelAdvanced?.Invoke(currentLevel, learningLevels[currentLevel]);
    }

    public static int GetCurrentLevel() {
        return currentLevel;
    }

    public static string[] GetCurrentLevelKeys() {
        return learningLevels[currentLevel];
    }

    public static float GetLevelProgress() {
        return (float)wordsCompletedInLevel / wordsNeededToAdvance;
	}
}
