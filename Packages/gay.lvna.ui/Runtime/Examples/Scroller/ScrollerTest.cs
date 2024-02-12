
using gay.lvna.ui.core.scroll;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace gay.lvna.ui.examples
{
    [RequireComponent(typeof(VerticalScroller))]
    public class ScrollerTest : UdonSharpBehaviour
    {
        [FieldChangeCallback("onClickCallbackSet")]
        public string onClickCallback;
        public string onClickCallbackSet
        {
            set
            {
                Debug.Log("onClickCallback: " + value);
            }
        }

        public void Start()
        {
            VerticalScroller scroller = GetComponent<VerticalScroller>();
            scroller.onClickCallback = this;
            scroller.onClickCallbackVar = "onClickCallback";

            scroller.AddEntry("Entry 1").GetComponentInChildren<TextMeshProUGUI>().text = "Entry 1";
            scroller.AddEntry("Entry 2").GetComponentInChildren<TextMeshProUGUI>().text = "Entry 2";
            scroller.AddEntry("Entry 3").GetComponentInChildren<TextMeshProUGUI>().text = "Entry 3";
        }
    }
}