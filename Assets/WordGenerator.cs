using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Necesario para Action

public class WordGenerator : MonoBehaviour {

    private static Dictionary<int, string[]> learningLevels = new Dictionary<int, string[]>() {
        {1, new string[] {"f", "j"}},
        {2, new string[] {"f", "j", "d", "k"}},
        {3, new string[] {"f", "j", "d", "k", "s", "l"}},
        {4, new string[] {"f", "j", "d", "k", "s", "l", "a", ";"}},
        {5, new string[] {"f", "j", "d", "k", "s", "l", "a", ";", "g", "h"}},
        {6, new string[] {"f", "j", "d", "k", "s", "l", "a", ";", "g", "h", "q", "p"}},
        {7, new string[] {"f", "j", "d", "k", "s", "l", "a", ";", "g", "h", "q", "p", "r", "u"}},
        {8, new string[] {"f", "j", "d", "k", "s", "l", "a", ";", "g", "h", "q", "p", "r", "u", "e", "i"}},
        {9, new string[] {"f", "j", "d", "k", "s", "l", "a", ";", "g", "h", "q", "p", "r", "u", "e", "i", "t", "y"}},
        {10, new string[] {"f", "j", "d", "k", "s", "l", "a", ";", "g", "h", "q", "p", "r", "u", "e", "i", "t", "y", "w", "o"}}
    };


    private static Dictionary<int, string[]> levelWords = new Dictionary<int, string[]>() {
        {1, new string[] {"jf", "fj", "jj", "ff", "jjf", "ffj"}},
        {2, new string[] {"jk", "fd", "kf", "dj", "jkfd", "fdjk", "jkfdj", "fdjkf"}},
        {3, new string[] {"jks", "fdl", "sljk", "slfd", "jkfdsl", "slfdjk"}},
        {4, new string[] {"jksa", "fdl;", "asljk", ";slfd", "jkfdsla", "slfdjk;"}},
        {5, new string[] {"jksagh", "fdlgh;", "ghasljk", ";slfdgh", "jkfdslag", "slfdjk;gh"}},
        {6, new string[] {"jksaghq", "fdlgh;p", "qpasljk", ";slfdghq", "jkfdslagq", "slfdjk;ghp"}},
        {7, new string[] {"jksaghqr", "fdlgh;pu", "ruqpasljk", ";slfdghqru", "jkfdslagqu", "slfdjk;ghpr"}},
        {8, new string[] {"jksaghqre", "fdlgh;pui", "eiqpasljk", ";slfdghqrei", "jkfdslagqei", "slfdjk;ghpre"}},
        {9, new string[] {"jksaghqret", "fdlgh;puiy", "tyiqpasljk", ";slfdghqrety", "jkfdslagqety", "slfdjk;ghprty"}},
        {10, new string[] {"jksaghqretw", "fdlgh;puiyo", "woyiqpasljk", ";slfdghqretwo", "jkfdslagqetwo", "slfdjk;ghprtwo"}}
    };

    private static int currentLevel = 1;
    private static int wordsCompletedInLevel = 0;
    private static int wordsNeededToAdvance = 10; 

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

    public static event Action<int, string[]> OnLevelAdvanced; 

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
