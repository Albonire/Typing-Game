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

	private bool hasActiveWord;
	private Word activeWord;

    void OnEnable() {
        WordGenerator.OnLevelAdvanced += HandleLevelAdvanced;
    }

    void OnDisable() {
        WordGenerator.OnLevelAdvanced -= HandleLevelAdvanced;
    }

    void Start() {
        UpdateUI();
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
        if (hasActiveWord) {
            if (activeWord.GetNextLetter() == letter) {
				activeWord.TypeLetter();
			}
        } else {
            foreach(Word word in words) {
                if (word.GetNextLetter() == letter) {
					activeWord = word;
					hasActiveWord = true;
					word.TypeLetter();
					break;
				}
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

    private void HandleLevelAdvanced(int level, string[] keys) {
        if (wordSpawner != null) {
            wordSpawner.SpawnDisplayMessage($"¡Nivel {level} desbloqueado! Teclas: {string.Join(", ", keys)}");
        }
        UpdateUI();
    }
}
