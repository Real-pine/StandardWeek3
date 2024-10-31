using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Resource,
    Equipable,
    Consumable
}

public enum ConsumableType
{
    Hunger,
    Health,
    Mana
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Information")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")] 
    public bool canStack;
    public int maxStackAmount;
    
    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
    
    [Header("Equipment")]
    public GameObject equipPrefab;
}
