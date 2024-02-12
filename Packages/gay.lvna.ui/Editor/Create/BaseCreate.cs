using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace gay.lvna.ui.editor.create
{
    public class BaseCreate : MonoBehaviour
    {
        public static string basePrefabPath = "Packages/gay.lvna.ui/Prefabs/";
        public static GameObject Create(string name, MenuCommand menuCommand)
        {
            // Take a name and a parent, create and unpack a prefab from the path
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(basePrefabPath + name + ".prefab");
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            instance.transform.SetParent(((GameObject)menuCommand.context).transform, false);

            PrefabUtility.UnpackPrefabInstance(instance, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
            return instance;
        }
    }
}
