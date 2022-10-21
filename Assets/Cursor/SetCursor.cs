using System;
using System.IO;
using UnityEngine;

public static class SetCursor
{
    static string holdFilePath = Environment.CurrentDirectory + @"\Assets\Cursor\Hold.png";
    public static void Hold()
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(holdFilePath))
        {
            fileData = File.ReadAllBytes(holdFilePath);
            tex = new Texture2D(1, 1);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        Cursor.SetCursor(tex, Vector2.zero, CursorMode.Auto);
    }
    public static void Default()
    {
        Cursor.SetCursor(null,Vector2.zero,CursorMode.Auto);
    }
}
