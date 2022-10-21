using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class tradeGUI : MonoBehaviour
{
    private Texture2D CurrentCursor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BuildButtonClick(Texture2D CursorImage)
    {
        if (CurrentCursor == CursorImage)
            return;
        CursorImage.width = (int)(CursorImage.width* 5);
        CursorImage.height = (int)(CursorImage.height * 5);
        CurrentCursor = CursorImage;
        Cursor.SetCursor(CurrentCursor, Vector2.zero, CursorMode.Auto);
    }
}
