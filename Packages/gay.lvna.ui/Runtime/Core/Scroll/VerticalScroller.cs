
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

        public ScrollEntry[] entries
        {
            get
            {
                ScrollEntry[] entries = new ScrollEntry[0];
                for (int i = 0; i < content.transform.childCount; i++)
                {
                    Transform child = content.transform.GetChild(i);
                    if (child.gameObject == template)
                    {
                        continue;
                    }
                    ScrollEntry entry = child.GetComponent<ScrollEntry>();
                    if (entry == null || entry.isDestroyed)
                    {
                        continue;
                    }
                    entries = entries.Add(entry);
                }
                return entries;
            }
        }

        public ScrollEntry AddEntry(DataToken data)
        {
            GameObject entry = Instantiate(template, content.transform);
            entry.SetActive(true);
            entry.GetComponent<ScrollEntry>().data = data;
            Layout();
            return entry.GetComponent<ScrollEntry>();
        }

        public void RemoveEntry(GameObject go)
        {
            ScrollEntry entry = go.GetComponent<ScrollEntry>();
            if (entry != null)
            {
                RemoveEntry(entry);
            }
        }
        public void RemoveEntry(DataToken data)
        {
            ScrollEntry entry = GetEntry(data);
            if (entry != null)
            {
                RemoveEntry(entry);
            }
        }
        public void RemoveEntry(ScrollEntry entry)
        {
            entry.isDestroyed = true;
            Destroy(entry.gameObject);
            Layout();
        }

        public ScrollEntry GetEntry(DataToken data)
        {
            foreach (ScrollEntry entry in entries)
            {
                if (entry.data == data)
                {
                    return entry;
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

            foreach (ScrollEntry entry in entries)
            {
                if (entry == null || entry.isDestroyed)
                {
                    continue;
                }
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
