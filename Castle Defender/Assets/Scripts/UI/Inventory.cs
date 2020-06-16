﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryGO;
    public InventorySlot[] invSlots;

    public void ShowInventory()
    {
        inventoryGO.SetActive(true);
    }

    public void HideInventory()
    {
        inventoryGO.SetActive(false);
    }

    public void UpdateSlotData(InventorySlot _slot, InventoryGearData _gearData)
    {
        _slot.UpdateData(_gearData);
    }
}

public static class InventoryEventSystem
{
    // When player clicks at hotkey (hotbar area) invGear adds to inventory and vice versa
    //WIP
    public static void CheckCursorInput()
    {
        List<RaycastResult> raycastedData = UIRaycast();
        if (raycastedData.Count == 0)
            return;

        RaycastResult raycast;

        raycast = raycastedData.FirstOrDefault(x => x.gameObject.CompareTag("Hotkey"));
        // If clicks at hotkey 
        if (raycast.gameObject != null)
        {
            Hotkey nowHK = raycast.gameObject.GetComponent<Hotkey>();

            InventorySlot emptyInvSlot = UIManager.Instance.inventory.invSlots.FirstOrDefault(x => x.invGear == null);

            // Changing first empty invSlot data taken from nowHK
            if (nowHK.invGear != null)
            {
                emptyInvSlot.invGear = nowHK.invGear;

                //Updating Data
                UIManager.Instance.inventory.UpdateSlotData(emptyInvSlot, nowHK.invGear);
                UIManager.Instance.hotbar.UpdateHotkeyData(nowHK, null);
            }

            InventoryEventSystem.ReplaceGear();

            Debug.Log("Clicked at hotkey");
            return;
        }

        raycast = raycastedData.FirstOrDefault(x => x.gameObject.CompareTag("InvSlot"));
        // If clicks at inventory slot 
        if (raycast.gameObject != null)
        {
            InventorySlot invSlot = raycast.gameObject.GetComponent<InventorySlot>();

            Hotkey emptyHK = UIManager.Instance.hotbar.hotkeys.FirstOrDefault(x => x.invGear == null);

            // Changing first empty hotkey data taken from invSlot
            if (invSlot.invGear != null)
            {
                emptyHK.invGear = invSlot.invGear;

                //Updating Data
                UIManager.Instance.hotbar.UpdateHotkeyData(emptyHK, invSlot.invGear);
                UIManager.Instance.inventory.UpdateSlotData(invSlot, null);
            }

            InventoryEventSystem.ReplaceGear();

            Debug.Log("Clicked at inventory slot");
            return;
        }
    }

    static List<RaycastResult> UIRaycast()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        return raycastResults;
    }

    public static void ReplaceGear()
    {
        PlayerController.Instance.DeleteOldGear();
        if (UIManager.Instance.hotbar.hotkeys[UIManager.Instance.hotbar.selectedKey].invGear != null)
            PlayerController.Instance.SpawnGear(UIManager.Instance.hotbar.hotkeys[UIManager.Instance.hotbar.selectedKey].invGear);
    }
    //WIP
    public static void AddGearToInventory(InventoryGearData _gearData)
    {
        Hotkey emptyHK = UIManager.Instance.hotbar.hotkeys.FirstOrDefault(x => x.invGear == null);
        UIManager.Instance.hotbar.UpdateHotkeyData(emptyHK, _gearData);
    }
}
