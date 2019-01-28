using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextLocalizer : MonoBehaviour {

    public string jsonTextName;
	// Update is called once per frame
	void Update ()
    {
	    if(SDKLoader.CheckIfEverythingLoaded())
        {
            GetComponent<Text>().text = LangDict.Instance.GetText(jsonTextName);
            GameObject.Destroy(this);
        }
	}
}
