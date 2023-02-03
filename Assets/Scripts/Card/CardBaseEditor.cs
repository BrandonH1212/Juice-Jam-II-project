using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Class)]
public class CardBehaviourAttribute : System.Attribute
{
    public static Dictionary<string, Type> CachedNameTypePair = new();
    public string CardBehaviourName { get; set; }

    public CardBehaviourAttribute(string cardBehaviourName) { CardBehaviourName = cardBehaviourName; }

    public static Type GetTypeWithBehaviourName(string name) 
    {
        CachedNameTypePair.TryGetValue(name, out Type type);
        if (type == null) 
        {
            foreach (Type typeWithAttr in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(typeof(CardBehaviourAttribute)))) 
            {
                if (typeWithAttr.GetAttribute<CardBehaviourAttribute>().CardBehaviourName == name) 
                {
                    CachedNameTypePair.Add(name, typeWithAttr);
                    return typeWithAttr;
                }
            }
        }
        return type;
    }
}

[CustomEditor(typeof(CardBase))]
public class CardBaseEditor : Editor
{
    SerializedProperty BehaviourTypeProperty;

    void OnEnable()
    {
        // BehaviourTypeProperty = serializedObject.FindProperty("BehaviourType");
    }

    public static IEnumerable<Type> GetTypesWithAttribute(Type attributeType)
    {
        return Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(attributeType));
        //foreach (Type type in assembly.GetTypes())
        //{
        //    if (type.GetCustomAttributes(attributeType, true).Length > 0)
        //    {
        //        yield return type;
        //    }
        //}
    }

    //public override void OnInspectorGUI()
    //{
    //    serializedObject.Update();
    //    // EditorGUILayout.PropertyField(lookAtPoint);
    //    base.OnInspectorGUI();

    //    CardBase obj = (serializedObject.targetObject as CardBase);
        
    //    List<Type> allTypesAvailable = new() { null };
    //    allTypesAvailable = allTypesAvailable.Concat(GetTypesWithAttribute(typeof(CardBehaviourAttribute))).ToList();

    //    EditorGUILayout.BeginHorizontal();
    //    EditorGUILayout.LabelField("Behaviour Name:");
    //    List<string> typesName = new() { "" };
    //    typesName = typesName.Concat(allTypesAvailable.ToList().Where(t => t != null).Select(t => t.Name).ToList()).ToList();
    //    obj.BehaviourTypeName = typesName.ToList()[EditorGUILayout.Popup(typesName.IndexOf(obj.BehaviourTypeName), typesName.ToArray())];
    //    EditorGUILayout.EndHorizontal();

    //    serializedObject.ApplyModifiedProperties();
    //}

    //public override void OnInspectorGUI()
    //{
    //    serializedObject.Update();
    //    EditorGUILayout.PropertyField(lookAtPoint);
    //    if (lookAtPoint.vector3Value.y > (target as LookAtPoint).transform.position.y)
    //    {
    //        EditorGUILayout.LabelField("(Above this object)");
    //    }
    //    if (lookAtPoint.vector3Value.y < (target as LookAtPoint).transform.position.y)
    //    {
    //        EditorGUILayout.LabelField("(Below this object)");
    //    }


    //    serializedObject.ApplyModifiedProperties();
    //}
}
