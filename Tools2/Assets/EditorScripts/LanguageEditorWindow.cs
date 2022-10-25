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

    static LanguageData languageData;
    static LanguageData generalLanguage;

    public static LanguageData LanguageInfo { get { return languageData; } }

    int editSelected = 0;
    int deleteSelected = 0;

    [MenuItem("Window/LanguageEditor")]
    static void OpenWindow()
    {
        LanguageEditorWindow window = (LanguageEditorWindow)GetWindow(typeof(LanguageEditorWindow));
        window.minSize = new Vector2(600, 300);
        window.Show();
    }

    private void OnEnable()
    {
        InitData();
        InitTextures();
        InitLanguage();
    }

    public static void InitData()
    {
        languageData = (LanguageData)ScriptableObject.CreateInstance(typeof(LanguageData));
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

        if(checkLanguages.Length == 0)
        {

            generalLanguage = (LanguageData)ScriptableObject.CreateInstance(typeof(LanguageData));
            generalLanguage.languageName = "GeneralLanguage";
            generalLanguage.keyvalues.Add("hello", "bye");

            string dataPath = "Assets/Resources/General/LanguageGeneral.asset";

            AssetDatabase.CreateAsset(generalLanguage, dataPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        } else
        {
            generalLanguage = (LanguageData)checkLanguages[0];
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
            languageNames.Add(l.name);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(l.languageName);
            EditorGUILayout.EndHorizontal();
        }

        string[] languageArray = languageNames.ToArray();

        //Add language function
        EditorGUILayout.BeginHorizontal();
        LanguageInfo.languageName = EditorGUILayout.TextField(LanguageInfo.languageName, GUILayout.Width(position.width/1.5f));

        if (GUILayout.Button("Add Language", buttonStyle, GUILayout.Width(position.width / 0.5f)))
        {
            AddNewLanguage();
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

    void AddNewLanguage()
    {

        Object[] checkLanguages = Resources.LoadAll("Languages");

        bool isExists = false;

        foreach (LanguageData l in checkLanguages)
        {

           if(l.languageName == LanguageInfo.languageName)
            {
                isExists = true;
            }
        }

        if (!isExists)
        {
            LanguageInfo.keyvalues = generalLanguage.keyvalues;

            string dataPath = "Assets/Resources/Languages/" + LanguageInfo.languageName + ".asset";

            AssetDatabase.CreateAsset(LanguageInfo, dataPath);
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

    static LanguageData language;
    static EditLanguages window;
    public static void OpenWindow(string languageName)
    {
        Object getLanguage = Resources.Load("Languages/" + languageName);
        language = (LanguageData)getLanguage;
        window = (EditLanguages)GetWindow(typeof(EditLanguages));
        window.minSize = new Vector2(250, 200);
        window.Show();
        Debug.Log(language.languageName);
    }
}