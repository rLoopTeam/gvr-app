﻿///Author: Michael House 
///Date: 1/27/2016

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PartLabel : MonoBehaviour, IPointerClickHandler
{

    public string partText = "";
    public Sprite imageToShow;
    public Text partLabelTextObject;
    public Image locationToShowImage;

    void Start()
    {
        if (string.IsNullOrEmpty(partText))
            partText = this.gameObject.name;
        if (partLabelTextObject == null)
        { 
            Debug.Log("You must set a target Text object for text to be displayed!");
            this.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ShowText();
    }

    public void ShowText()
    {
        partLabelTextObject.text = partText;
        locationToShowImage.sprite = imageToShow;
    }
}
