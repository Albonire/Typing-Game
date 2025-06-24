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
    }

    void UpdateUI() {
        if (levelText != null) {
            levelText.text = $"Nivel: {WordGenerator.GetCurrentLevel()}";
        }
        if (progressText != null) {
            progressText.text = $"Progreso: {Mathf.Round(WordGenerator.GetLevelProgress() * 100)}%";
        }
        if (currentKeysText != null) {
            currentKeysText.text = $"Teclas: {string.Join(", ", WordGenerator.GetCurrentLevelKeys())}";
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
                }
            } else if (!hasActiveWord) {
                if (lowestWord.GetNextLetter() == letter) {
                    activeWord = lowestWord;
                    hasActiveWord = true;
                    lowestWord.TypeLetter();
                }
            }

            if (hasActiveWord && activeWord.WordTyped()) {
                score++;
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
}
