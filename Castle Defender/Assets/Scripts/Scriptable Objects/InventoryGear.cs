﻿using UnityEngine;

[CreateAssetMenu (fileName = "InventoryItem", menuName = "Scriptable Objects/Inventory Gear")]
public class InventoryGear : ScriptableObject
{
    public GameObject prefab;
    public Vector3 spawnPosition;
}