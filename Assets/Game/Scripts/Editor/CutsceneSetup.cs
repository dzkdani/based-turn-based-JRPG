#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEditor;
using UnityEditor.SceneManagement;
using Fungus;

public class CutsceneSetup
{
    private const string CUTSCENE_MANAGER_NAME = "CutsceneManager";
    private const string PLAYABLE_DIRECTOR_NAME = "PlayableDirector";
    private const string FLOWCHART_NAME = "DialogueFlowchart";
    private const string CUTSCENE_SEQUENCE_NAME = "CutsceneSequence";

    [MenuItem("Tools/Cutscene/Setup Scene")]
    public static void SetupCutsceneScene()
    {
        var scene = EditorSceneManager.GetActiveScene();
        if (!scene.isLoaded)
        {
            EditorUtility.DisplayDialog("Error", "No scene is currently open.", "OK");
            return;
        }

        GameObject managerGO = CreateOrFindGameObject(CUTSCENE_MANAGER_NAME);
        var manager = managerGO.GetComponent<CutsceneManager>();
        if (manager == null)
        {
            manager = managerGO.AddComponent<CutsceneManager>();
        }
        managerGO.transform.SetAsFirstSibling();
        AddScenePlayerLockable(manager);

        GameObject directorGO = CreateOrFindGameObject(PLAYABLE_DIRECTOR_NAME);
        var director = directorGO.GetComponent<PlayableDirector>();
        if (director == null)
        {
            director = directorGO.AddComponent<PlayableDirector>();
        }
        director.playOnAwake = false;

        GameObject flowchartGO = CreateOrFindGameObject(FLOWCHART_NAME);
        var flowchart = flowchartGO.GetComponent<Flowchart>();
        if (flowchart == null)
        {
            flowchart = flowchartGO.AddComponent<Flowchart>();
        }

        GameObject sequenceGO = CreateOrFindGameObject(CUTSCENE_SEQUENCE_NAME);
        if (sequenceGO.GetComponent<CutsceneSequence>() == null)
        {
            sequenceGO.AddComponent<CutsceneSequence>();
        }

        EditorUtility.SetDirty(managerGO);
        EditorUtility.SetDirty(directorGO);
        EditorUtility.SetDirty(flowchartGO);
        EditorUtility.SetDirty(sequenceGO);
        EditorSceneManager.MarkSceneDirty(scene);

        EditorUtility.DisplayDialog(
            "Success",
            $"Cutscene scene setup complete!\n\n" +
            $"Created:\n" +
            $"- {CUTSCENE_MANAGER_NAME}\n" +
            $"- {PLAYABLE_DIRECTOR_NAME}\n" +
            $"- {FLOWCHART_NAME}\n" +
            $"- {CUTSCENE_SEQUENCE_NAME}\n\n" +
            $"Next steps:\n" +
            $"1. Create a Timeline asset and cutscene step assets\n" +
            $"2. Assign it to the PlayableDirector\n" +
            $"3. Add the step assets to CutsceneSequence\n" +
            $"4. Call CutsceneSequence.Run() from your trigger",
            "OK"
        );
    }

    private static GameObject CreateOrFindGameObject(string name)
    {
        var existingGO = GameObject.Find(name);
        if (existingGO != null)
        {
            return existingGO;
        }

        var newGO = new GameObject(name);
        return newGO;
    }

    private static void AddScenePlayerLockable(CutsceneManager manager)
    {
        var player = Object.FindAnyObjectByType<PlayerController>();
        if (player == null)
            return;

        var serializedManager = new SerializedObject(manager);
        var lockables = serializedManager.FindProperty("lockables");
        for (int i = 0; i < lockables.arraySize; i++)
        {
            if (lockables.GetArrayElementAtIndex(i).objectReferenceValue == player)
                return;
        }

        lockables.InsertArrayElementAtIndex(lockables.arraySize);
        lockables.GetArrayElementAtIndex(lockables.arraySize - 1).objectReferenceValue = player;
        serializedManager.ApplyModifiedProperties();
    }

    [MenuItem("Tools/Cutscene/Create Timeline Asset")]
    public static void CreateTimelineAsset()
    {
        var path = EditorUtility.SaveFilePanelInProject(
            "Create Timeline",
            "NewCutscene",
            "playable",
            "Save your Timeline asset"
        );

        if (string.IsNullOrEmpty(path))
            return;

        var timeline = ScriptableObject.CreateInstance<TimelineAsset>();
        AssetDatabase.CreateAsset(timeline, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Success", $"Timeline created at:\n{path}", "OK");
    }
}
#endif
