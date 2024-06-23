using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Limb : MonoBehaviour
{
    public static Limb CreateLimb(LimbSO limbSO, Vector2 position)
    {
        GameObject instantiate = Instantiate(GameResources.Instance.limbPrefab);
        instantiate.transform.position = position;

        if (!instantiate.TryGetComponent(out Limb limb))
            Debug.Log("cannot get component limb fron prefab");

        limb.SetUp(limbSO, position);

        return limb;
    }

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private GameObject tileCollider;
    [SerializeField] private Transform rotateTransform;
    [SerializeField] private CompositeCollider2D compositeCollider;
    [SerializeField] private SoundEffectSO clickSoundEffectSO;
    [SerializeField] private SoundEffectSO rotateSoundEffectSO;
    [SerializeField] private SoundEffectSO dropSoundEffectSO;

    public bool isPlaced { get; private set; }
    private float lerpAmount = 25;
    private float lerpSoftRange = 0.05f;
    private Vector2 originPosition;
    private float targetAngle;
    private Vector2 targetPosition;
    private bool isDragging;
    private LimbSO limbSO;
    private orientation orientation;

    private void Update()
    {
        LerpToAngle();
        LerpToPosition();

        if (!isDragging)
            return;

        HandleHoverPosition();
    }

    private void Awake()
    {
        inputHandler.onLeftBeginDrag += InputHandler_onLeftBeginDrag;
        inputHandler.onLeftEndDrag += InputHandler_onLeftEndDrag;
        inputHandler.onPointerRightClick += InputHandler_onPointerRightClick;
    }

    private void InputHandler_onPointerRightClick()
    {
        HandleRotate();
    }

    private void InputHandler_onLeftBeginDrag()
    {
        HandleBeginDrag();
    }

    private void InputHandler_onLeftEndDrag()
    {
        HandleEndDrag();
    }

    public void SetUp(LimbSO limbSO, Vector2 position)
    {
        this.limbSO = limbSO;
        spriteRenderer.sprite = limbSO.sprite;
        isPlaced = false;
        isDragging = false;
        orientation = orientation.East;
        targetAngle = Orientation.GetOrientationAngle(orientation);
        this.targetPosition = position;
        originPosition = position;
        spriteRenderer.sprite = limbSO.sprite;

        foreach (Vector2Int occupiedPosition in limbSO.occupiedPositionArray)
        {
            GameObject instantiate = Instantiate(tileCollider, tileCollider.transform.parent);
            instantiate.transform.localPosition = (Vector2)occupiedPosition * Settings.slotCellSize;

            if (!instantiate.TryGetComponent(out BoxCollider2D boxCollider))
                Debug.Log("cannot get box collider");

            boxCollider.size = new Vector2(Settings.slotCellSize, Settings.slotCellSize);
        }
    }

    private void HandleBeginDrag()
    {
        if (isPlaced)
        {
            isPlaced = false;
            LevelManager.Instance.SetOccupied(transform.position, limbSO, orientation, false);
        }

        isDragging = true;
        SoundEffectManager.Instance.PlaySoundEffect(clickSoundEffectSO);
    }

    private void HandleEndDrag()
    {
        Vector2 gridWorldPosition = LevelManager.Instance.GetWorldPosition(transform.position);

        if (!LevelManager.Instance.CanPlace(transform.position, limbSO, orientation))
        {
            if (LevelManager.Instance.IsAnyOccupiedPositionValid(transform.position, limbSO, orientation))
                targetPosition = originPosition;
            else if (CanPlaceTemporary())
                targetPosition = transform.position;
            else
                targetPosition = originPosition;
        }
        else
        {
            transform.position = gridWorldPosition;
            targetPosition = gridWorldPosition;
            isPlaced = true;
            LevelManager.Instance.SetOccupied(transform.position, limbSO, orientation, true);
            SoundEffectManager.Instance.PlaySoundEffect(dropSoundEffectSO);
        }

        isDragging = false;
    }

    private void HandleRotate()
    {
        if (!isPlaced)
        {
            orientation = Orientation.GetNextOrientation(orientation);
            targetAngle = Orientation.GetOrientationAngle(orientation);
        }
        else
        {
            LevelManager.Instance.SetOccupied(transform.position, limbSO, orientation, false);

            if (LevelManager.Instance.CanPlace(transform.position, limbSO, Orientation.GetNextOrientation(orientation)))
            {
                orientation = Orientation.GetNextOrientation(orientation);
                targetAngle = Orientation.GetOrientationAngle(orientation);
                LevelManager.Instance.SetOccupied(transform.position, limbSO, orientation, true);
            }
            else
            {
                LevelManager.Instance.SetOccupied(transform.position, limbSO, orientation, true);
                rotateTransform.localEulerAngles = new Vector3(0, 0, Orientation.GetOrientationAngle(orientation) - 15);
            }
        }

        SoundEffectManager.Instance.PlaySoundEffect(rotateSoundEffectSO);
    }

    private void HandleHoverPosition()
    {
        Vector2 mousePosition = HelperUtils.GetMouseWorldPosition();
        Vector2 gridWorldPosition = LevelManager.Instance.GetWorldPosition(mousePosition);

        if (!LevelManager.Instance.IsAnyOccupiedPositionValid(mousePosition, limbSO, orientation))
        {
            targetPosition = mousePosition;
        }
        else
        {
            targetPosition = gridWorldPosition;
        }
    }

    public void LerpToPosition()
    {
        if ((Vector2)transform.position == targetPosition)
            return;

        if (Vector2.Distance(transform.position, targetPosition) < lerpSoftRange)
        {
            transform.position = targetPosition;
            return;
        }

        transform.position = Vector2.Lerp(transform.position, targetPosition, lerpAmount * Time.deltaTime);
    }

    public void LerpToAngle()
    {
        if (rotateTransform.localEulerAngles.z == targetAngle)
            return;

        if (Mathf.Abs(rotateTransform.localEulerAngles.z - targetAngle) < lerpSoftRange)
        {
            rotateTransform.localEulerAngles = new Vector3(0, 0, targetAngle);
            return;
        }

        rotateTransform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(rotateTransform.localEulerAngles.z, targetAngle, lerpAmount * Time.deltaTime));
    }

    public bool CanPlaceTemporary()
    {
        Collider2D[] overlapColliderArray = new Collider2D[1];
        compositeCollider.OverlapCollider(new ContactFilter2D(), overlapColliderArray);

        if (overlapColliderArray[0] != null)
            return false;

        return true;
    }
}
