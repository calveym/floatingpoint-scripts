using UnityEngine;
 
using UnityEditor;
using System.Collections;
 
public class AddParent : ScriptableObject
{
    [MenuItem ("Tools/+Add Parent")]
    static void MenuInsertParent()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel |
            SelectionMode.OnlyUserModifiable);
 
        GameObject newParent = new GameObject("_Parent");
        Transform newParentTransform = newParent.transform;
 
        if(transforms.Length == 1)
        {
            Transform originalParent = transforms[0].parent;
            transforms[0].parent = newParentTransform;
            if(originalParent)
                newParentTransform.parent = originalParent;
        }
        else
        {
            foreach(Transform transform in transforms)
                transform.parent = newParentTransform;
        }
    }
}