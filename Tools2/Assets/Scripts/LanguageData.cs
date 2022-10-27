using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenuAttribute(fileName = "New Language Data", menuName = "Languages")]
public class LanguageData : ScriptableObject
{
    public string languageName;
    public CustomDictionary keyvalues = new CustomDictionary();
}
