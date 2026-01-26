using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Light flashlightSpotlight;
    [SerializeField] private KeyCode toggleKey = KeyCode.F;
    [SerializeField] private KeyCode dropKey = KeyCode.G;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Vector3 offsetPosition = new Vector3(0.3f, -0.2f, 0.5f);
    
    [Header("Drop Settings")]
    [SerializeField] private float dropDistance = 2f;
    [SerializeField] private Vector3 dropForce = new Vector3(0, 1, 2);
    
    [Header("Hidden Clue Detection")]
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private LayerMask clueLayer;
    
    private bool isOn = false;
    private bool isEquipped = false;
    private Collider originalCollider;
    private Rigidbody originalRigidbody;

    void Start()
    {
        // Zu Beginn deaktiviert
        this.enabled = false;
        
        if (flashlightSpotlight == null)
            flashlightSpotlight = GetComponentInChildren<Light>();
        
        if (flashlightSpotlight != null)
            flashlightSpotlight.enabled = false;

        if (playerCamera == null)
            playerCamera = Camera.main.transform;
    }

    public void EquipFlashlight()
    {
        isEquipped = true;
        
        // Setze Taschenlampe als Kind der Kamera
        transform.SetParent(playerCamera);
        transform.localPosition = offsetPosition;
        transform.localRotation = Quaternion.identity;
        
        // Speichere oder deaktiviere Physics Komponenten
        originalCollider = GetComponent<Collider>();
        if (originalCollider != null)
            originalCollider.enabled = false;
        
        originalRigidbody = GetComponent<Rigidbody>();
        if (originalRigidbody != null)
            originalRigidbody.isKinematic = true;
    }

    void Update()
    {
        if (!isEquipped) return;

        // Toggle Taschenlampe an/aus
        if (Input.GetKeyDown(toggleKey))
        {
            isOn = !isOn;
            flashlightSpotlight.enabled = isOn;
        }

        // Taschenlampe ablegen
        if (Input.GetKeyDown(dropKey))
        {
            DropFlashlight();
            return;
        }

        // Prüfe ob wir auf einen versteckten Hinweis leuchten
        if (isOn)
        {
            CheckForHiddenClues();
        }
    }

    void CheckForHiddenClues()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, clueLayer))
        {
            HiddenClue clue = hit.collider.GetComponent<HiddenClue>();
            if (clue != null)
            {
                clue.Reveal();
            }
        }
    }

    void DropFlashlight()
    {
        isEquipped = false;
        
        // Schalte Licht aus
        isOn = false;
        if (flashlightSpotlight != null)
            flashlightSpotlight.enabled = false;
        
        // Löse von der Kamera
        transform.SetParent(null);
        
        // Positioniere vor dem Spieler
        Vector3 dropPosition = playerCamera.position + playerCamera.forward * dropDistance;
        transform.position = dropPosition;
        transform.rotation = playerCamera.rotation;
        
        // Reaktiviere Physics
        if (originalCollider != null)
            originalCollider.enabled = true;
        
        if (originalRigidbody != null)
        {
            originalRigidbody.isKinematic = false;
            // Gib einen kleinen Impuls
            Vector3 force = playerCamera.TransformDirection(dropForce);
            originalRigidbody.AddForce(force, ForceMode.Impulse);
        }
        
        // Reaktiviere das Pickup Script
        FlashlightPickup pickup = GetComponent<FlashlightPickup>();
        if (pickup != null)
        {
            pickup.enabled = true;
        }
        
        // Deaktiviere dieses Script
        this.enabled = false;
    }

    void OnDrawGizmos()
    {
        if (isEquipped && isOn)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, transform.forward * rayDistance);
        }
    }
}
