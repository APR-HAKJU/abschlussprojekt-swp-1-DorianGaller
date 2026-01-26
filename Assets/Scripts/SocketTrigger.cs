using UnityEngine;

public class SocketTrigger : MonoBehaviour
{
    public string rightobject = "Key";
    public GameObject objectToAppear;
    public Light spotLight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (objectToAppear != null)
        objectToAppear.SetActive(false);

        if (spotLight != null)
        spotLight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.name == rightobject || other.name.StartsWith("rightobject"))
        {

        if (objectToAppear != null)
            objectToAppear.SetActive(true);

        if (spotLight != null)
            spotLight.enabled = true;

        Debug.Log("Richtiges Objekt platziert. neues Objekt erscheint!");;
        }
    }
}
