using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LanguageEditorWindow : EditorWindow
{

    Texture2D editLanguageSectionTexture;
    Texture2D editKeysSectionTexture;

    Rect editLanguageSection;

    Color editLanguageSectionColor = new Color(13f / 255f, 32f / 255f, 44f / 255f, 1f);

    public LanguageData generalLanguage;

    int editSelected = 0;
    int deleteSelected = 0;
    static string newLanguageName;

    [MenuItem("Window/LanguageEditor")]
    static void OpenWindow()
    {
        LanguageEditorWindow window = (LanguageEditorWindow)GetWindow(typeof(LanguageEditorWindow));
        window.minSize = new Vector2(600, 300);
        window.Show();
    }

    private void OnEnable()
    {
        InitTextures();
        InitLanguage();
    }

    private void InitTextures()
    {
        editLanguageSectionTexture = new Texture2D(1, 1);
        editLanguageSectionTexture.SetPixel(0, 0, editLanguageSectionColor);
        editLanguageSectionTexture.Apply();
    }

    private void InitLanguage()
    {
        Object[] checkLanguages = Resources.LoadAll("General");

        generalLanguage = (LanguageData)ScriptableObject.CreateInstance(typeof(LanguageData));
        if (checkLanguages.Length == 0)
        {

            generalLanguage.languageName = "GeneralLanguage";
            generalLanguage.keyvalues.Add("hello", "bye");

            string dataPath = "Assets/Resources/General/LanguageGeneral.asset";

            AssetDatabase.CreateAsset(generalLanguage, dataPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        } else
        {
            generalLanguage = (LanguageData)checkLanguages[0];
            Debug.Log(generalLanguage.keyvalues.ContainsKey("hello"));
        }
    }
    void OnGUI()
    {
        InitializeEditLanguageSection();
        DrawEditLanguageSection();

    }

    void InitializeEditLanguageSection()
    {
        editLanguageSection.x = 0;
        editLanguageSection.y = 0;
        editLanguageSection.width = Screen.width;
        editLanguageSection.height = Screen.height;
        GUI.DrawTexture(editLanguageSection, editLanguageSectionTexture);
    }

    void DrawEditLanguageSection()
    {
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.alignment = TextAnchor.MiddleLeft;
        GUILayout.BeginArea(editLanguageSection);
        GUILayout.Label("Languages: ");

        Object[] languages = Resources.LoadAll("Languages");
        List<string> languageNames = new List<string>();

        foreach (LanguageData l in languages)
        {
            languageNames.Add(l.languageName);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(l.languageName);
            EditorGUILayout.EndHorizontal();
        }

        string[] languageArray = languageNames.ToArray();

        //Add language function
        EditorGUILayout.BeginHorizontal();
        newLanguageName = EditorGUILayout.TextField(newLanguageName, GUILayout.Width(position.width/1.5f));

        if (GUILayout.Button("Add Language", buttonStyle, GUILayout.Width(position.width / 0.5f)))
        {
            AddNewLanguage(newLanguageName);
        }
        EditorGUILayout.EndHorizontal();

        //Edit Language Function
        EditorGUILayout.BeginHorizontal();

        editSelected = EditorGUILayout.Popup(editSelected, languageArray, GUILayout.Width(position.width / 1.5f));

        if(GUILayout.Button("Edit Language", buttonStyle, GUILayout.Width(position.width / 0.5f)))
        {
            EditLanguages.OpenWindow(languageArray[editSelected]);
        }
        EditorGUILayout.EndHorizontal();

        //Edit Language Function
        EditorGUILayout.BeginHorizontal();

        deleteSelected = EditorGUILayout.Popup(deleteSelected, languageArray, GUILayout.Width(position.width / 1.5f));

        if (GUILayout.Button("Delete Language", buttonStyle, GUILayout.Width(position.width / 0.5f)))
        {
            AssetDatabase.DeleteAsset("Assets/Resources/Languages/" + languageArray[deleteSelected] + ".asset");
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    void AddNewLanguage(string checkLanguageName)
    {
        LanguageData languageData = (LanguageData)ScriptableObject.CreateInstance(typeof(LanguageData));

        Object[] checkLanguages = Resources.LoadAll("Languages");

        bool isExists = false;

        foreach (LanguageData l in checkLanguages)
        {

           if(l.languageName == checkLanguageName)
            {
                isExists = true;
            }
        }

        if (!isExists)
        {
            languageData.languageName = checkLanguageName;
            languageData.keyvalues = generalLanguage.keyvalues;
            string dataPath = "Assets/Resources/Languages/" + languageData.languageName + ".asset";

            AssetDatabase.CreateAsset(languageData, dataPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        } else
        {
            Debug.Log("Language already exists!");
        }
    }
}

public class EditLanguages : EditorWindow
{
    Texture2D editKeyValueSectionTexture;

    Rect editKeyValueSection;

    Color editKeyValueSectionColor = new Color(13f / 255f, 32f / 255f, 44f / 255f, 1f);

    static LanguageData languageTemp;
    static EditLanguages window;

    static string key = "";
    static string value = "";
    static int keyEditSelected = 0;
    static int keyDeleteSelected = 0;
    public static void OpenWindow(string languageName)
    {
        Object getLanguage = Resources.Load("Languages/" + languageName);

        languageTemp = (LanguageData)getLanguage;
        SerializableDictionary<string, string>.Enumerator myEnumerator = languageTemp.keyvalues.GetEnumerator();

        if (myEnumerator.MoveNext())
        {
            value = myEnumerator.Current.Value;
        }

        window = (EditLanguages)GetWindow(typeof(EditLanguages));
        window.minSize = new Vector2(250, 200);
        window.Show();
    }

    private void OnEnable()
    {
        InitKeyValueTexture();
        InitializeKeyValueSection();
    }

    void InitializeKeyValueSection()
    {
        editKeyValueSection.x = 0;
        editKeyValueSection.y = 0;
        editKeyValueSection.width = Screen.width;
        editKeyValueSection.height = Screen.height;
        GUI.DrawTexture(editKeyValueSection, editKeyValueSectionTexture);
    }

    void InitKeyValueTexture()
    {
        editKeyValueSectionTexture = new Texture2D(1, 1);
        editKeyValueSectionTexture.SetPixel(0, 0, editKeyValueSectionColor);
        editKeyValueSectionTexture.Apply();
    }

    private void OnGUI()
    {
        DrawKeyValueSection();
    }


    void DrawKeyValueSection()
    {
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.alignment = TextAnchor.MiddleLeft;
        List<string> keyList = new List<string>();
        foreach(string k in languageTemp.keyvalues.Keys)
        {
            keyList.Add(k);
        }

        string[] keyArray = keyList.ToArray();

        GUILayout.BeginArea(editKeyValueSection);
        GUILayout.Label("Language: " + languageTemp.languageName);

        EditorGUILayout.BeginHorizontal();
        key = EditorGUILayout.TextField(key, GUILayout.Width(position.width / 1.5f));
        if (GUILayout.Button("Add Key", buttonStyle, GUILayout.Width(position.width / 0.5f)))
        {
            AddNewKey(key);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        keyDeleteSelected = EditorGUILayout.Popup(keyDeleteSelected, keyArray, GUILayout.Width(position.width / 1.5f));
        if (GUILayout.Button("Delete Key", buttonStyle, GUILayout.Width(position.width / 0.5f)))
        {
            DeleteKey(keyArray[keyDeleteSelected]);
        }
        EditorGUILayout.EndHorizontal();


        //Edit key-value
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Edit Text Values For Keys");
        EditorGUILayout.EndHorizontal();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        keyEditSelected = EditorGUILayout.Popup(keyEditSelected, keyArray, GUILayout.Width(position.width / 1.5f));
        EditorGUILayout.EndHorizontal();
        if (EditorGUI.EndChangeCheck())
        {
            value = languageTemp.keyvalues[keyArray[keyEditSelected]];
        }

        EditorGUILayout.BeginHorizontal();
        value = EditorGUILayout.TextField(value, GUILayout.Width(position.width / 1.5f));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Edit Value", buttonStyle, GUILayout.Width(position.width / 0.5f)))
        {
            EditKeys(keyArray[keyEditSelected], value);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Close", buttonStyle))
        {
            window.Close();
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();

    }

    void AddNewKey(string keyName)
    {
        if (!languageTemp.keyvalues.ContainsKey(keyName)){
            Key newKey = new Key();
            newKey.keyName = keyName;
            newKey.isAdd = true;

            Save(newKey);

            languageTemp.keyvalues.Add(keyName, "");
        } else
        {
            Debug.Log("Key already exists!");
        }
    }

    void DeleteKey(string deleteKey)
    {
        Key newKey = new Key();
        newKey.keyName = deleteKey;
        newKey.isAdd = false;

        Save(newKey);

        languageTemp.keyvalues.Remove(deleteKey);
    }

    void Save(Key k)
    {
        Object[] otherLanguages = Resources.LoadAll("Languages");

        foreach(LanguageData l in otherLanguages)
        {
            LanguageData languageEdit = (LanguageData)ScriptableObject.CreateInstance(typeof(LanguageData));
            Debug.Log(languageEdit.languageName);
            Debug.Log(l.keyvalues.ContainsKey(k.keyName));
            if (k.isAdd)
            {
               l.keyvalues.Add(k.keyName, "");
            }
            else
            {
               l.keyvalues.Remove(k.keyName);
            }

            languageEdit.languageName = l.languageName;
            languageEdit.keyvalues = l.keyvalues;

            EditorUtility.SetDirty(languageEdit);
            AssetDatabase.SaveAssets();
            AssetDatabase.ForceReserializeAssets();
        }

        Debug.Log("General");
        Object generalLanguage = Resources.Load("General/LanguageGeneral");

        LanguageData generalLanguageTemp = (LanguageData)generalLanguage;
        Debug.Log(generalLanguageTemp.keyvalues.ContainsKey(k.keyName));
        if (k.isAdd)
        {
           generalLanguageTemp.keyvalues.Add(k.keyName, "");
        }
        else
        {
           generalLanguageTemp.keyvalues.Remove(k.keyName);
        }

        LanguageData newGeneralLanguage = (LanguageData)ScriptableObject.CreateInstance(typeof(LanguageData));
        newGeneralLanguage.languageName = generalLanguageTemp.languageName;
        newGeneralLanguage.keyvalues = generalLanguageTemp.keyvalues;

        AssetDatabase.DeleteAsset("Assets/Resources/General/LanguageGeneral.asset");
        AssetDatabase.CreateAsset(newGeneralLanguage, "Assets/Resources/General/LanguageGeneral.asset");
        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();
    }

    void EditKeys(string keyName, string fieldValue)
    {
        Object getLanguage = Resources.Load("Languages/" + languageTemp.languageName);

        LanguageData temp = (LanguageData)getLanguage;
        LanguageData languageToEdit = (LanguageData)ScriptableObject.CreateInstance(typeof(LanguageData));
        languageToEdit.languageName = temp.languageName;
        languageToEdit.keyvalues = temp.keyvalues;

        Debug.Log(fieldValue);
        languageToEdit.keyvalues[keyName] = fieldValue;

        Debug.Log(languageToEdit.keyvalues[keyName]);
        EditorUtility.SetDirty(languageToEdit);
        AssetDatabase.SaveAssets();
        AssetDatabase.ForceReserializeAssets();
        AssetDatabase.Refresh();
    }

}