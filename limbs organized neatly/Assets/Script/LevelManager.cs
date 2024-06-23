using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public event Action OnLevelClear;

    [SerializeField] GameObject slotPrefab;
    [SerializeField] private FadeIn fadeIn;

    private List<Limb> limbList;
    private List<GameObject> slotGameObjectList;

    public IEnumerator LoadLevel(LevelSO levelSO)
    {
        CleanUpLevel();

        if (levelSO.textArray != null)
        {
            for(int i = 0; i< levelSO.textArray.Length; i++)
            {
                yield return StartCoroutine(fadeIn.FadeInText(levelSO.textArray[i], 4));
            }
        }

        levelSO.GetWidthHeight(out int width, out int height);
        Vector2 orginPosition = new Vector2(-width * Settings.slotCellSize / 2, -height * Settings.slotCellSize / 2);
        slotGrid = new CustomGrid<Slot>(width, height, Settings.slotCellSize, orginPosition, () => new Slot());
        limbList = new List<Limb>();
        slotGameObjectList = new List<GameObject>();

        foreach (Vector2Int availablePosition in levelSO.availableSlotArray)
        {
            if (!slotGrid.TryGetValue(availablePosition, out Slot slot))
                Debug.Log("level slot out of range");

            slot.isAvailable = true;

            // Create slot sprite gameObject
            GameObject slotInstantiate = Instantiate(slotPrefab, transform);
            slotInstantiate.transform.position = slotGrid.GetWorldPosition(availablePosition);
            slotInstantiate.SetActive(true);

            slotGameObjectList.Add(slotInstantiate);
        }

        foreach (LimbOrginPosition limbOrginPosition in levelSO.limbOrginPositionArray)
        {
            Limb limb = Limb.CreateLimb(limbOrginPosition.limbSO, limbOrginPosition.orginPosition);
            limbList.Add(limb);
        }
    }

    private CustomGrid<Slot> slotGrid;

    public bool CanPlace(Vector2 worldPosition, LimbSO limbSO, orientation orientation)
    {
        foreach (Vector2Int occupiedPosition in limbSO.occupiedPositionArray)
        {
            Vector2 rotatedPosition = Orientation.GetRotatedPoint((Vector2)occupiedPosition, orientation);
            Vector2 rotatedWorldPosition = rotatedPosition * Settings.slotCellSize + worldPosition;

            // If out of range
            if (!slotGrid.TryGetValue(rotatedWorldPosition, out Slot slot))
                return false;

            // If not available
            if (!slot.isAvailable)
                return false;

            // If occupied
            if (slot.isOccupied)
                return false;
        }
        return true;
    }

    private void CleanUpLevel()
    {
        if (limbList != null)
            foreach (Limb limb in limbList)
                Destroy(limb.gameObject);

        if (slotGameObjectList != null)
            foreach (GameObject gameObject in slotGameObjectList)
                Destroy(gameObject);
    }

    public bool IsAnyOccupiedPositionValid(Vector2 worldPosition, LimbSO limbSO, orientation orientation)
    {
        foreach (Vector2Int occupiedPosition in limbSO.occupiedPositionArray)
        {
            Vector2 rotatedPosition = Orientation.GetRotatedPoint((Vector2)occupiedPosition, orientation);
            Vector2 rotatedWorldPosition = rotatedPosition * Settings.slotCellSize + worldPosition;

            // If out of range
            if (!slotGrid.TryGetValue(rotatedWorldPosition, out Slot slot))
                continue;

            // If not available
            if (!slot.isAvailable)
                continue;

            return true;
        }
        return false;
    }

    public void SetOccupied(Vector2 worldPosition, LimbSO limbSO, orientation orientation, bool isOccupied)
    {
        foreach (Vector2Int occupiedPosition in limbSO.occupiedPositionArray)
        {
            Vector2 rotatedPosition = Orientation.GetRotatedPoint((Vector2)occupiedPosition, orientation);
            Vector2 rotatedWorldPosition = rotatedPosition * Settings.slotCellSize + worldPosition;

            // If out of range
            if (!slotGrid.TryGetValue(rotatedWorldPosition, out Slot slot))
                Debug.Log("Position out of range");

            // If not available
            if (!slot.isAvailable)
                Debug.Log("Slot not available");

            // if set same occupied
            if (slot.isOccupied == isOccupied)
                Debug.Log("Try set same occupied");

            slot.isOccupied = isOccupied;
        }

        if (IsLevelClear())
            OnLevelClear?.Invoke();
    }

    public Vector2Int GetGridPosition(Vector2 worldPosition) => slotGrid.GetGridPosition(worldPosition);

    public Vector2 GetWorldPosition(Vector2 worldPosition) => slotGrid.GetWorldPosition(worldPosition);

    private bool IsLevelClear()
    {
        foreach (Limb limb in limbList)
        {
            if (!limb.isPlaced)
                return false;
        }
        return true;
    }
}

public class Slot
{
    public bool isAvailable = false;
    public bool isOccupied = false;
}
