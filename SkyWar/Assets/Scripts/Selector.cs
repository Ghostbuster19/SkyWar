using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(GameObject))]
public class Selector : MonoBehaviour
{

    public Color WantedColor;
    public GameObject Parent;

    [SerializeField]
    private LayerMask ContactLayer;
    private Color originalColor;
    private Canvas canvas;
    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        canvas = Parent.GetComponentInChildren(typeof(Canvas)) as Canvas;
        Debug.Log(canvas.name);
        originalColor = renderer.material.color;
    }

	// Update is called once per frame
	void Update ()
	{
        if (Input.GetMouseButtonDown(0))
	    {
	        RaycastHit hit;
	        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ContactLayer))
	        {
	            Debug.Log("Got a hit: " + hit.transform.gameObject.name);
	        }
	    }
	}

    void OnMouseEnter()
    {
        renderer.material.color = WantedColor;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            canvas.gameObject.SetActive(true);
            Debug.Log("This is now selected");
        }
    }

    void OnMouseExit()
    {
        renderer.material.color = originalColor;
        canvas.gameObject.SetActive(false);
    }
}
