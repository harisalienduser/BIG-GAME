﻿using UnityEngine;
using UnityEngine.UI;
using System;
using Ink.Runtime;
using System.Collections.Generic;
using TMPro;
public enum characters
{
	Dr_Baciu,
	Maj_Janik,
	Dr_Kim
}
// This is a super bare bones example of how to play and display a ink story in Unity.
public class BasicInkExample : MonoBehaviour {
	public static event Action<Story> OnCreateStory;
	private const string PORTRAIT_TAG = "portrait";
	private const string SPEAKER_TAG = "speaker";
	public TextMeshProUGUI displayNameText;
	public Image portraitImg;
	public Sprite[] characterPortraits;
	
	void Awake() {
		// Remove the default message
		RemoveChildren();
		StartStory();
	}

	// Creates a new Story object with the compiled story which we can then play!
	void StartStory() {
		story = new Story(inkJSONAsset.text);
		if (OnCreateStory != null) OnCreateStory(story);
		RefreshView();
	}

	// This is the main function called every time the story changes. It does a few things:
	// Destroys all the old content and choices.
	// Continues over all the lines of text, then displays all the choices. If there are no choices, the story is finished!
	void RefreshView() {
		// Remove all the UI on screen
		RemoveChildren();

		// Read all the content until we can't continue any more
		while (story.canContinue) {
			// Continue gets the next line of the story
			string text = story.Continue();
			// This removes any white space from the text.
			text = text.Trim();
			// Display the text on screen!
			CreateContentView(text);
			
		}

		// Display all the choices, if there are any!
		if (story.currentChoices.Count > 0) {
			for (int i = 0; i < story.currentChoices.Count; i++) {
				Choice choice = story.currentChoices[i];
				Button button = CreateChoiceView(choice.text.Trim());
				// Tell the button what to do when we press it
				button.onClick.AddListener(delegate {
					OnClickChoiceButton(choice);
				});
			}
		}
		// If we've read all the content and there's no choices, the story is finished!
		else {
			Button choice = CreateChoiceView("End of story.\nRestart?");
			choice.onClick.AddListener(delegate {
				StartStory();
			});
		}
	}
	private void HandleTags(List<string> currentTags)
	{
		// loop through each tag and handle it accordingly
		foreach (string tag in currentTags)
		{
			// parse the tag
			string[] splitTag = tag.Split(':');
			if (splitTag.Length != 2)
			{
				Debug.LogError("Tag could not be appropriately parsed: " + tag);
			}
			string tagKey = splitTag[0].Trim();
			string tagValue = splitTag[1].Trim();

			// handle the tag
			switch (tagKey)
			{
				case SPEAKER_TAG:
					displayNameText.text = tagValue;
					break;
				case PORTRAIT_TAG:
					portraitImg.sprite = characterPortraits[GetCharacterIndex(tagValue)];
					break;

				default:
					Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
					break;
			}
		}
	}
	private int GetCharacterIndex(string value)
	{
		foreach (characters charValue in Enum.GetValues(typeof(characters)))
		{
			if (charValue.ToString() == value)
			{
				return (int)charValue;
				break;
			}
		}
		return 0;
	}

	// When we click the choice button, tell the story to choose that choice!
	void OnClickChoiceButton (Choice choice) {
		story.ChooseChoiceIndex (choice.index);
		RefreshView();
	}

	// Creates a textbox showing the the line of text
	void CreateContentView (string text) {
		Text storyText = Instantiate (textPrefab) as Text;
		storyText.text = text;
		storyText.transform.SetParent (canvas.transform, false);
		HandleTags(story.currentTags);
	}

	// Creates a button showing the choice text
	Button CreateChoiceView (string text) {
		// Creates the button from a prefab
		Button choice = Instantiate (buttonPrefab) as Button;
		choice.transform.SetParent (canvas.transform, false);
		
		// Gets the text from the button prefab
		Text choiceText = choice.GetComponentInChildren<Text> ();
		choiceText.text = text;

		// Make the button expand to fit the text
		HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
		layoutGroup.childForceExpandHeight = false;

		return choice;
	}

	// Destroys all the children of this gameobject (all the UI)
	void RemoveChildren () {
		int childCount = canvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i) {
			GameObject.Destroy (canvas.transform.GetChild (i).gameObject);
		}
	}

	[SerializeField]
	private TextAsset inkJSONAsset = null;
	public Story story;

	[SerializeField]
	private Canvas canvas = null;

	// UI Prefabs
	[SerializeField]
	private Text textPrefab = null;
	[SerializeField]
	private Button buttonPrefab = null;
}
