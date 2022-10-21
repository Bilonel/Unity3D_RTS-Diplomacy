using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public Sprite Active;
    public Sprite Normal;
    public Texture2D HandCursor;
    public Texture2D ArrowCursor;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(ArrowCursor, Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseOver()
    {
        GetComponent<SpriteRenderer>().sprite = Active;
        Cursor.SetCursor(HandCursor, Vector2.zero, CursorMode.Auto);
    }
    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().sprite = Normal;
        Cursor.SetCursor(ArrowCursor, Vector2.zero, CursorMode.Auto);
    }
}
