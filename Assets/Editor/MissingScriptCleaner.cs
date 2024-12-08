using UnityEditor;
using UnityEngine;

public class MissingScriptCleaner : MonoBehaviour {
    [MenuItem("Tools/Clean Missing Scripts")]
    static void CleanMissingScripts() {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        int count = 0;

        foreach (GameObject obj in allObjects) {
            SerializedObject so = new SerializedObject(obj);
            SerializedProperty sp = so.FindProperty("m_Component");

            if (sp != null) {
                for (int i = sp.arraySize - 1; i >= 0; i--) {
                    SerializedProperty component = sp.GetArrayElementAtIndex(i);
                    if (component.objectReferenceValue == null) {
                        sp.DeleteArrayElementAtIndex(i);
                        count++;
                    }
                }
                so.ApplyModifiedProperties();
            }
        }

        Debug.Log($"Removed {count} missing scripts from GameObjects.");
    }
}
