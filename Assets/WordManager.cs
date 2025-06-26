using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordManager : MonoBehaviour {
    public int score = 0;
    public List<Word> words;
	public WordSpawner wordSpawner;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI currentKeysText;
    public Image handImage;
    public int lives = 3;
    public int errors = 0;
    public int wordsCompleted = 0;
    public int highScore = 0;
    public bool practiceMode = false; // Si es true, no hay vidas ni errores
    private float startTime;
    private float endTime;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI errorsText;
    public TextMeshProUGUI wordsCompletedText;
    public TextMeshProUGUI precisionText;
    public TextMeshProUGUI highScoreText;

	private bool hasActiveWord;
	private Word activeWord;
    private Coroutine handImageCoroutine;

    void OnEnable() {
        WordGenerator.OnLevelAdvanced += HandleLevelAdvanced;
    }

    void OnDisable() {
        WordGenerator.OnLevelAdvanced -= HandleLevelAdvanced;
    }

    void Start() {
        UpdateUI();
        UpdateHandImage(WordGenerator.GetCurrentLevel());
        ShowInitialLevelMessage();
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        startTime = Time.time;
    }

    void UpdateUI() {
        if (levelText != null) {
            levelText.text = "Nivel: " + WordGenerator.GetCurrentLevel();
        }
        if (progressText != null) {
            progressText.text = "Progreso: " + Mathf.Round(WordGenerator.GetLevelProgress() * 100) + "%";
        }
        if (currentKeysText != null) {
            currentKeysText.text = "Teclas: " + string.Join(", ", WordGenerator.GetCurrentLevelKeys());
        }
        if (livesText != null) {
            livesText.text = "Vidas: " + lives;
        }
        if (errorsText != null) {
            errorsText.text = "Errores: " + errors;
        }
        if (wordsCompletedText != null) {
            wordsCompletedText.text = "Palabras: " + wordsCompleted;
        }
        if (precisionText != null) {
            float precision = wordsCompleted + errors > 0 ? 100f * (float)wordsCompleted / (wordsCompleted + errors) : 100f;
            precisionText.text = "Precisión: " + precision.ToString("F1") + "%";
        }
        if (highScoreText != null) {
            highScoreText.text = "Récord: " + highScore;
        }
    }

    public void AddWord() {
		Word word = new Word(WordGenerator.GetRandomWord(), wordSpawner.SpawnWord());
		words.Add(word);
	}

    public void TypeLetter(char letter) {
        // palabra más abajo
        Word lowestWord = null;
        float lowestY = float.MaxValue;
        foreach (Word word in words) {
            if (word != null && word.word != null && word.display != null && word.GetNextLetter() != '\0') {
                float y = word.display.transform.position.y;
                if (y < lowestY) {
                    lowestY = y;
                    lowestWord = word;
                }
            }
        }

        if (lowestWord != null && lowestWord.GetNextLetter() != '\0') {
            if (hasActiveWord && activeWord == lowestWord && activeWord.GetNextLetter() != '\0') {
                if (activeWord.GetNextLetter() == letter) {
				activeWord.TypeLetter();
                } else {
                    RegisterError();
                }
            } else if (!hasActiveWord) {
                if (lowestWord.GetNextLetter() == letter) {
                    activeWord = lowestWord;
					hasActiveWord = true;
                    lowestWord.TypeLetter();
                } else {
                    RegisterError();
			}
		}

            if (hasActiveWord && activeWord.WordTyped()) {
            score++;
                wordsCompleted++;
                WordGenerator.WordCompleted();
                UpdateUI();
            hasActiveWord = false;
			words.Remove(activeWord);
		}
        }
    }

    private void HandleLevelAdvanced(int level, string[] keys) {
        if (wordSpawner != null) {
            wordSpawner.SpawnDisplayMessage($"¡Nivel {level} desbloqueado! Teclas: {string.Join(", ", keys)}");
        }
        UpdateHandImage(level);
        UpdateUI();
    }

    private void UpdateHandImage(int level) {
        string imageName = "";
        switch (level) {
            case 1:
                imageName = "indices";
                break;
            case 2:
                imageName = "indicesymedios";
                break;
            default:
                imageName = "indicesmediosyanulares";
                break;
        }
        Sprite newSprite = Resources.Load<Sprite>(imageName);
        if (newSprite != null && handImage != null) {
            handImage.sprite = newSprite;
            handImage.enabled = true;
            if (handImageCoroutine != null) {
                StopCoroutine(handImageCoroutine);
            }
            handImageCoroutine = StartCoroutine(HideHandImageAfterSeconds(4f));
        }
    }

    private IEnumerator HideHandImageAfterSeconds(float seconds) {
        yield return new WaitForSeconds(seconds);
        if (handImage != null) {
            handImage.enabled = false;
        }
    }

    private void ShowInitialLevelMessage() {
        if (wordSpawner != null) {
            string msg = "¡Nivel 1 desbloqueado! Teclas: " + string.Join(", ", WordGenerator.GetCurrentLevelKeys());
            wordSpawner.SpawnDisplayMessage(msg);
        }
    }

    private void RegisterError() {
        if (!practiceMode) {
            errors++;
            lives--;
            Debug.Log("¡Error! Vidas restantes: " + lives + ", Errores: " + errors);
            if (lives <= 0) {
                EndGame();
            }
        } else {
            errors++;
            Debug.Log("¡Error en práctica libre! Total errores: " + errors);
        }
    }

    private void EndGame() {
        endTime = Time.time;
        float totalTime = endTime - startTime;
        float precision = wordsCompleted > 0 ? 100f * (float)wordsCompleted / (wordsCompleted + errors) : 0f;
        Debug.Log("\n--- FIN DEL JUEGO ---");
        Debug.Log("Puntaje: " + score);
        Debug.Log("Palabras completadas: " + wordsCompleted);
        Debug.Log("Errores: " + errors);
        Debug.Log("Precisión: " + precision.ToString("F1") + "%");
        Debug.Log("Tiempo total: " + totalTime.ToString("F1") + " segundos");
        if (score > highScore) {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            Debug.Log("¡Nuevo récord! High Score: " + highScore);
        } else {
            Debug.Log("High Score: " + highScore);
        }
        // Reiniciar variables para nueva partida
        score = 0;
        lives = 3;
        errors = 0;
        wordsCompleted = 0;
        startTime = Time.time;
	}
}
