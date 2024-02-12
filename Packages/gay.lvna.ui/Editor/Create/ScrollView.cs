using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace gay.lvna.ui.editor.create
{
    public class ScrollView : BaseCreate
    {
        [MenuItem("GameObject/UI/(Luna) Scroll View", false, 10)]
        public static void CreateScrollView(MenuCommand menuCommand)
        {
            Create("Scroller", menuCommand);
        }
    }
}
