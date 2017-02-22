using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using MyLib.Parsing.XML;
using MyLib.Parsing.SimpleParser;
using UnityUIBuilder;

[ExecuteInEditMode]
[RequireComponent(typeof(Canvas))]
public class UIBuilder : MonoBehaviour
{
    [SerializeField]
    string uiName;

    void Start()
    {
        while (transform.childCount != 0)
            DestroyImmediate(transform.GetChild(0).gameObject);

        new XMLApplication().Perform(uiName, transform);
    }
}

