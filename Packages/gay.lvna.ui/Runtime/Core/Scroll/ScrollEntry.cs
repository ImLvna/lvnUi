
using UdonSharp;
using UdonToolkit;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace gay.lvna.ui.core.scroll
{
  [HelpMessage("This component is used to represent a single entry in a scroll view. It should be a child of the content object of the scroll view. This must be a prefab to give the button component the right reference to the OnClick callback.")]
  public class ScrollEntry : UdonSharpBehaviour
  {
    [HideInInspector]
    public DataToken data;

    public void OnClick()
    {
      transform.parent.parent.parent.GetComponent<VerticalScroller>().OnClick(data);
    }

  }
}