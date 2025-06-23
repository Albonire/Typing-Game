using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Word {

	public string word;
	private int typeIndex;

	public WordDisplay display;
    public bool isDisplayOnly { get; private set; }

	public Word (string _word, WordDisplay _display, bool _isDisplayOnly = false)
	{
		word = _word;
		typeIndex = 0;
        isDisplayOnly = _isDisplayOnly;

		display = _display;
        display.SetWord(word, isDisplayOnly);
	}

	public char GetNextLetter ()
	{
        if (isDisplayOnly) return '\0'; // No letters to type for display-only words
		return word[typeIndex];
	}

	public void TypeLetter ()
	{
        if (isDisplayOnly) return; // Cannot type display-only words
		typeIndex++;
		display.RemoveLetter();
	}

	public bool WordTyped ()
	{
        if (isDisplayOnly) return false; // Display-only words are not "typed"
		bool wordTyped = (typeIndex >= word.Length);
		if (wordTyped)
        {
            display.RemoveWord();
		}
		return wordTyped;
	}

}
