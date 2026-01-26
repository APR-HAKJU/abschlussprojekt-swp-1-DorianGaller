// FlashlightPickup.cs
// Dieses Script kommt auf die Taschenlampe in der Welt
using UnityEngine;

public class FlashlightPickup : MonoBehaviour
{
    [SerializeField] private float pickupRange = 3f;
    [SerializeField] private KeyCode pickupKey = KeyCode.E;
    [SerializeField] private GameObject pickupPrompt; // UI Element "Drücke E zum Aufheben"
    
    private Transform player;
    private bool isInRange = false;
    private bool isPickedUp = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (pickupPrompt != null)
            pickupPrompt.SetActive(false);
    }

    void Update()
    {
        if (isPickedUp) return;

        float distance = Vector3.Distance(transform.position, player.position);
        isInRange = distance <= pickupRange;

        if (pickupPrompt != null)
            pickupPrompt.SetActive(isInRange);

        if (isInRange && Input.GetKeyDown(pickupKey))
        {
            PickupFlashlight();
        }
    }

    void PickupFlashlight()
    {
        isPickedUp = true;
        
        // Finde die FlashlightController Komponente und aktiviere sie
        FlashlightController controller = GetComponent<FlashlightController>();
        if (controller != null)
        {
            controller.enabled = true;
            controller.EquipFlashlight();
        }

        // Verstecke das Pickup Prompt
        if (pickupPrompt != null)
            pickupPrompt.SetActive(false);

        // Deaktiviere dieses Script
        this.enabled = false;
    }
}