using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class Scenario : ScriptableObject{
    public static readonly Dictionary<int, Type> scenarios = new();

    public int ID { get; }
    
    public string ScenarioName { get; }

    [Serializable]
    public struct ScenarioUnits {
        public UnitPreset[] Allies;

        public UnitPreset[] Enemies;

        public int UnitsEngaged;

        public UnitPreview[] PresetUnit;

    }

    [Header("Units Info")]
    [SerializeField] private ScenarioUnits UnitsProperties;

    [SerializeField] private GameObject _terrain;
    [SerializeField] private SpawnLocation[] SpawnLocations;

    public virtual IQuest[] Goals { get; }

    public virtual GameObject Terrain { get => _terrain; }

    public int UnitsEngaged => UnitsProperties.UnitsEngaged;

    public virtual IQuest NextQuest(int phase) {
        return null;
    }
    
    public virtual void OnLoad() { }

    public void Load() {
        Loader.Instance.SetupScene(Terrain, UnitsProperties.Allies, UnitsProperties.Enemies);

        EventManager.GoalAchived += ToNextQuest;
        OnLoad();
        ToNextQuest();
    }

    public void EndScenario() {
        EventManager.GoalAchived -= ToNextQuest;
        EventManager.SendScenarioFinishedEvent(this);
        Loader.Instance.LoadScene(1);
    }

    private int curQuest = 0;

    private void ToNextQuest() {
        try {
            NextQuest(curQuest).StartQuest();
            curQuest++;
        } catch (Exception) { }   
    }

    public virtual UnitPreview[] GetUnitsPreset() => UnitsProperties.PresetUnit;

    public virtual SpawnLocation[] GetSpawnLocations() => SpawnLocations;

}


#if UNITY_EDITOR
[CustomEditor(typeof(Scenario))]
public class ScenarioEditor : Editor {

    private SerializedProperty _scenarioId;
    private SerializedProperty _scenarioName;

    private List<string> _questTypeNames;
    private SerializedProperty _quests;

    [MenuItem("Assets/Scenario", priority = 0)]
    public static void CreateScenario() {
        Scenario newScenario = CreateInstance<Scenario>();

        ProjectWindowUtil.CreateAsset(newScenario, "scenario.asset");
    }

    private void OnEnable() {
        _scenarioId = serializedObject.FindProperty(nameof(Scenario.ID));
        _scenarioName = serializedObject.FindProperty(nameof(Scenario.ScenarioName));
        _quests = serializedObject.FindProperty(nameof(Scenario.Goals));

        Type lookup = typeof(Scenario);
        _questTypeNames = System.AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(lookup)).Select(type => type.Name).ToList();

    }

    public override void OnInspectorGUI() {
        DrawProperty(_scenarioId.Copy());
        DrawProperty(_scenarioName.Copy());

    }

    private void DrawProperty(SerializedProperty child) {
        int depth = child.depth;
        child.NextVisible(true);
        EditorGUILayout.LabelField("Scenario Info");
        while (child.depth > depth) {
            EditorGUILayout.PropertyField(child, true);
            child.NextVisible(false);
        }

        int choice = EditorGUILayout.Popup("Add new Quest", -1, _questTypeNames.ToArray());
        if(choice != -1) {
            ScriptableObject quest = ScriptableObject.CreateInstance(_questTypeNames[choice]);
            AssetDatabase.AddObjectToAsset(quest, target);

            _quests.InsertArrayElementAtIndex(_quests.arraySize);
            _quests.GetArrayElementAtIndex(_quests.arraySize - 1).objectReferenceValue = quest;
        }
        Editor ed = null;
        int toDelete = -1;
        for (int i = 0; i < _quests.arraySize; i++) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            SerializedProperty item = _quests.GetArrayElementAtIndex(i);
            SerializedObject option = new SerializedObject(item.objectReferenceValue);
            CreateCachedEditor(item.objectReferenceValue, null, ref ed);
            ed.OnInspectorGUI();
            EditorGUILayout.EndVertical();
            if (GUILayout.Button("-", GUILayout.Width(32))) {
                toDelete = i;
            }
            EditorGUILayout.EndHorizontal();
        }

        

        if(toDelete != 1) {
            UnityEngine.Object item = _quests.GetArrayElementAtIndex(toDelete).objectReferenceValue;
            DestroyImmediate(item);

            _quests.DeleteArrayElementAtIndex(toDelete);
            _quests.DeleteArrayElementAtIndex(toDelete);
        }

        serializedObject.ApplyModifiedProperties();
    }


}
#endif



