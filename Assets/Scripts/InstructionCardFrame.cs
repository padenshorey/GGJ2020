﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionCardFrame : MonoBehaviour
{
    public GameObject activeObject;
    public GameObject disabledObject;
    public Transform repairRow;
    public Image backgroundFill;

    private int _successfulRepairs = 0;
    public InstructionCard instructionCard;

    private bool _inUse = false;
    public bool InUse { get { return _inUse; } }

    public void ActivateCard(InstructionCard card)
    { 
        _inUse = true;
        activeObject.SetActive(true);
        disabledObject.SetActive(false);
        instructionCard = card;

        PopulateCard(card);
    }

    public void PopulateCard(InstructionCard card)
    {
        foreach(Repair r in card.Repairs)
        {
            GameObject buttonIconResource = Resources.Load("ButtonIcon") as GameObject;
            GameObject buttonIcon = GameObject.Instantiate(buttonIconResource, repairRow);
            ButtonIcon bi = buttonIcon.GetComponent<ButtonIcon>();
            foreach(KeyValuePair<string, Enums.RepairType> keyValuePair in r.repairRequirements)
            {
                bi.SetImage(keyValuePair.Key);
            }
        }
    }

    public void IncreaseStep()
    {
        _successfulRepairs++;
        backgroundFill.fillAmount = (float)_successfulRepairs / (float)instructionCard.Repairs.Count;
    }

    public void ResetProgress()
    {
        _successfulRepairs = 0;
        backgroundFill.fillAmount = 0;
    }

    public void SetAsComplete()
    {

    }

    public void DisableCard()
    {
        ResetProgress();

        foreach (Transform child in repairRow)
        {
            GameObject.Destroy(child.gameObject);
        }

        _inUse = false;
        activeObject.SetActive(false);
        disabledObject.SetActive(true);
        instructionCard = null;
    }

}
