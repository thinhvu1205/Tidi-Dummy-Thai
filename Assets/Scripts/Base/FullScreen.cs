using UnityEngine;

public class FullScreen : MonoBehaviour
{
    [SerializeField]
    bool isPortrait;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 sizeFullParent = transform.parent.GetComponent<RectTransform>().rect.size;
        Vector2 sizeFull;
        if (isPortrait)
        {
            sizeFull = new Vector2(sizeFullParent.x, sizeFullParent.y);
        }
        else
        {
            sizeFull = new Vector2(sizeFullParent.y, sizeFullParent.x);
        }

        transform.GetComponent<RectTransform>().sizeDelta = sizeFull;
    }
}
