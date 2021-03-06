using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Microsoft.CSharp;

public class LocalizedText : MonoBehaviour {

	public string key;

	private Text text;
	private TextMesh textMesh;
	[SerializeField] private bool isTextMesh = false;

	[Header("References")]
	[SerializeField]private LocalizationManager localizationManager;



	void OnEnable(){

	
		if (isTextMesh) {
			textMesh = GetComponent<TextMesh> ();
			if (textMesh == null) 
				Debug.LogWarning ("Cannot get LocalizedText TextMesh Component");

		}else {
			text = GetComponent<Text> ();
			if (text == null) 
				Debug.LogWarning ("Cannot get LocalizedText Text Component");
			
		}
			
		localizationManager.registeredLocalizedTexts.Add (this);
        SetTextLocalizedKey(key);
	}

	public void OnDisable(){

		localizationManager.registeredLocalizedTexts.Remove (this);
	}



	public void OnUpdate () 
	{

		if (isTextMesh) {
			textMesh.text = localizationManager.GetLocalizedValue (key);
			textMesh.text = textMesh.text.Replace ("\\n", "\n");

		} else {
			text.text =  localizationManager.GetLocalizedValue (key);
			text.text = text.text.Replace("\\n", "\n");
		}

	}
    public void SetTextLocalizedKey(string nKey)
    {

        key = nKey;
        OnUpdate();


    }
    public void RemoveLocalizedKey()
    {

        key = string.Empty;
        OnUpdate();


    }
    private bool isFirstTime = true;

	IEnumerator SetText(){

		while (! localizationManager.GetIsReady ())
			yield return null;

	if(isTextMesh)
			textMesh.text =  localizationManager.GetLocalizedValue (key);
	else
			text.text =  localizationManager.GetLocalizedValue (key);
		

	}


}
