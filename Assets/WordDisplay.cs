using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordDisplay : MonoBehaviour {

	public TextMeshProUGUI text;
	public float fallSpeed = 1f;
    public float displayDuration = 5f; // Duración para palabras de solo mostrar

    private bool isDisplayOnlyWord = false;

    public void SetWord (string word, bool isDisplayOnly = false)
	{
		text.text = word;
        isDisplayOnlyWord = isDisplayOnly;

        // Alternar automáticamente entre blanco y amarillo claro para contraste
        Color chalkWhite = Color.white;
        Color chalkYellow = new Color(1f, 0.95f, 0.6f); // Amarillo claro tipo tiza
        text.color = (Random.value > 0.5f) ? chalkWhite : chalkYellow;

        if (isDisplayOnlyWord)
        {
            text.fontSize = 45;
            StartCoroutine(FadeAndRemove());
        }
        else
        {
            text.fontSize = 48;
        }
    }

	public void RemoveLetter ()
	{
        if (isDisplayOnlyWord) return; // No se remueven letras de palabras de solo mostrar
		text.text = text.text.Remove(0, 1);
		text.color = Color.red;
	}

	public void RemoveWord ()
	{
		Destroy(gameObject);
	}

	private void Update()
	{
		transform.Translate(0f, -fallSpeed * Time.deltaTime, 0f);
	}

    private IEnumerator FadeAndRemove()
    {
        yield return new WaitForSeconds(displayDuration); // Esperar la duración

        // Opcional: Fade out
        float fadeTime = 0.5f; // Tiempo para desvanecer
        Color originalColor = text.color;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime)
        {
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(originalColor.a, 0, t));
            text.color = newColor;
            yield return null;
        }

        Destroy(gameObject);
    }

}
