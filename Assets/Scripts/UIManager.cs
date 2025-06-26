using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI inventoryText;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateStat(GameObject statBar, float currStat, float maxStat)
    {
        RectTransform rt = statBar.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(maxStat * 2, rt.sizeDelta.y); // adjust max health bar width

        Slider slider = statBar.GetComponent<Slider>();
        slider.maxValue = maxStat; // adjust max health bar fill
        slider.value = currStat; // adjust current health bar fill
    }

    public void UpdateInventory(Item[] inventory)
    {
        inventoryText.text = "";
        foreach (GameObject item in inventory.Keys)
        {
            inventoryText.text += $"{item.name}s: {inventory[item]}\n";
        }
    }
}
