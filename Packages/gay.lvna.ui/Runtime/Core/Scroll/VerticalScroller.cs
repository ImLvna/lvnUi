
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UdonToolkit;
using UnityEngine.Events;
using gay.lvna.common.udon.extensions;
using TMPro;
using VRC.SDK3.Data;





#if !COMPILER_UDONSHARP && UNITY_EDITOR
using UnityEditor;
#endif

namespace gay.lvna.ui.core.scroll
{
    public class VerticalScroller : UdonSharpBehaviour
    {

        [HelpBox("Link your own UdonBehaviour here to handle the click event. The callback will edit the variable below with the data of the clicked entry.")]
        public UdonSharpBehaviour onClickCallback;
        public string onClickCallbackVar = "data";


        [OnValueChanged("OnRowsChanged")]
        [RangeSlider(1, 10)]
        public int Rows = 1;

#if !COMPILER_UDONSHARP && UNITY_EDITOR
        public void OnRowsChanged(SerializedProperty value)
        {
            Rows = value.intValue;
            Layout();
        }
#endif

        GameObject content
        {
            get
            {
                return transform.Find("Viewport").Find("Content").gameObject;
            }
        }

        GameObject template
        {
            get
            {
                return content.transform.GetChild(0).gameObject;
            }
        }

        GameObject[] entries
        {
            get
            {
                GameObject[] entries = new GameObject[0];
                for (int i = 0; i < content.transform.childCount; i++)
                {
                    if (content.transform.GetChild(i).gameObject == template)
                    {
                        continue;
                    }
                    entries = entries.Add(content.transform.GetChild(i).gameObject);
                }
                return entries;
            }
        }

        public GameObject AddEntry(DataToken data)
        {
            GameObject entry = Instantiate(template, content.transform);
            entry.SetActive(true);
            entry.GetComponent<ScrollEntry>().data = data;
            Layout();
            return entry;
        }

        public void RemoveEntry(GameObject entry)
        {
            Destroy(entry);
            Layout();
        }

        public ScrollEntry GetEntry(DataToken data)
        {
            foreach (GameObject entry in entries)
            {
                if (entry.GetComponent<ScrollEntry>().data == data)
                {
                    return entry.GetComponent<ScrollEntry>();
                }
            }
            return null;
        }


        public void Layout()
        {
            RectTransform rect = content.GetComponent<RectTransform>();
            float maxWidth = rect.rect.width;
            float stepHeight = template.GetComponent<RectTransform>().rect.height;
            float stepWidth = maxWidth / Rows;
            float top = 0;
            float left = 0;

            if (template != null)
            {
                template.GetComponent<RectTransform>().anchoredPosition = new Vector2(left, -top);
                template.GetComponent<RectTransform>().sizeDelta = new Vector2(stepWidth, stepHeight);
            }

            foreach (GameObject entry in entries)
            {
                RectTransform entryRect = entry.GetComponent<RectTransform>();
                entryRect.anchoredPosition = new Vector2(left, -top);
                entryRect.sizeDelta = new Vector2(stepWidth, stepHeight);

                if (left + stepWidth < maxWidth)
                {
                    left += stepWidth;
                }
                else
                {
                    Debug.Log(left);
                    Debug.Log(stepWidth);
                    Debug.Log(maxWidth);
                    left = 0;
                    top += stepHeight;
                }
            }
            rect.sizeDelta = new Vector2(maxWidth, top + stepHeight);
        }


        public void OnClick(DataToken data)
        {
            Debug.Log(onClickCallback);
            if (onClickCallback != null)
            {
                onClickCallback.SetProgramVariable(onClickCallbackVar, data);
            }
        }

        void Start()
        {
            template.SetActive(false);
        }
    }
}
