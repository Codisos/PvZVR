using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(IntReference))]
public class IntReferenceDataContainer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        //pripravit pozici a sirku
        Rect labelPos = new Rect(position.x, position.y, position.width, position.height);
        position = EditorGUI.PrefixLabel(labelPos, label);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        float widthSize = position.width / 2;
        float offsetSize = 10;

        Rect posPick = new Rect(position.x, position.y, widthSize - offsetSize, position.height);
        Rect posValue = new Rect(position.x + widthSize, position.y, widthSize - offsetSize, position.height);


        SerializedProperty isConstantProperty = property.FindPropertyRelative("UseConstant");
        SerializedProperty referenceTypeProperty = property.FindPropertyRelative("RefType");


        EditorGUI.PropertyField(posPick, referenceTypeProperty, GUIContent.none);

        referenceType type = (referenceType)referenceTypeProperty.enumValueIndex;

        //logika pro to co se zobrazi

        if (type == referenceType.Constant)
        {
            isConstantProperty.boolValue = true;
            SerializedProperty constantProperty = property.FindPropertyRelative("ConstantValue");
            EditorGUI.PropertyField(posValue, constantProperty, GUIContent.none);
        }
        else
        {
            isConstantProperty.boolValue = false;
            SerializedProperty variableProperty = property.FindPropertyRelative("Variable");
            EditorGUI.PropertyField(posValue, variableProperty, GUIContent.none);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight;

        return height;
    }
}
