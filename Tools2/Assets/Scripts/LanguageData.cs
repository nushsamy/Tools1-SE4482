using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Language Data", menuName = "Languages")]
public class LanguageData : ScriptableObject
{
    public string languageName;
    public Dictionary<string, string> keyvalues = new Dictionary<string, string>();
}
