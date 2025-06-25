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

    public void UpdateHP(GameObject hpBar, float currHP, float maxHP)
    {
        RectTransform rt = hpBar.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(maxHP * 2, rt.sizeDelta.y); // adjust max health bar width

        Slider slider = hpBar.GetComponent<Slider>();
        slider.value = currHP; // adjust current health bar fill
    }

    public void UpdateInventory(Dictionary<GameObject, int> inventory)
    {
        inventoryText.text = "";
        foreach (GameObject item in inventory.Keys)
        {
            inventoryText.text += $"{item.name}s: {inventory[item]}\n";
        }
    }
}
