// HiddenClue.cs
// Dieses Script kommt auf die versteckten Hinweise
using UnityEngine;

public class HiddenClue : MonoBehaviour
{
    [Header("Reveal Settings")]
    [SerializeField] private bool permanentReveal = false;
    [SerializeField] private float fadeSpeed = 2f;
    
    private Renderer rend;
    private Material mat;
    private Color originalColor;
    private Color targetColor;
    private bool isRevealed = false;
    private float currentAlpha = 0f;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            mat = rend.material;
            originalColor = mat.color;
            targetColor = originalColor;
            
            // Starte unsichtbar
            Color invisible = originalColor;
            invisible.a = 0f;
            mat.color = invisible;
            
            // Falls Material Emission hat, deaktiviere es
            if (mat.HasProperty("_EmissionColor"))
            {
                mat.DisableKeyword("_EMISSION");
            }
        }
    }

    public void Reveal()
    {
        if (!isRevealed || !permanentReveal)
        {
            isRevealed = true;
            
            // Aktiviere Emission falls vorhanden
            if (mat.HasProperty("_EmissionColor"))
            {
                mat.EnableKeyword("_EMISSION");
            }
        }
    }

    void Update()
    {
        if (mat == null) return;

        // Fade in wenn revealed, fade out wenn nicht
        float targetAlpha = isRevealed ? 1f : 0f;
        
        if (!permanentReveal && !isRevealed)
        {
            targetAlpha = 0f;
        }

        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * fadeSpeed);
        
        Color newColor = originalColor;
        newColor.a = currentAlpha;
        mat.color = newColor;

        // Reset revealed status wenn nicht permanent
        if (!permanentReveal && isRevealed)
        {
            isRevealed = false;
        }
    }
}