using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TextureStruct 
{
    public static Texture2D duplicateTexture2D(Texture2D texture2D)
    {
        var tex = new Texture2D(texture2D.width, texture2D.width, TextureFormat.RGB24, false);
        tex.SetPixels(texture2D.GetPixels());
        tex.Apply();
        return tex;
    }

    public static Sprite convertTexture2DToSprite(Texture2D texture2D)
    {
        return Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
            new Vector2(texture2D.width / 2, texture2D.height / 2));
    }

    public static Texture2D convertSpriteToTexture2D(Sprite sprite)
    {
        var croppedTexture = new Texture2D( (int)sprite.rect.width, (int)sprite.rect.height );
        var pixels = sprite.texture.GetPixels(  (int)sprite.textureRect.x, 
            (int)sprite.textureRect.y, 
            (int)sprite.textureRect.width, 
            (int)sprite.textureRect.height );
        croppedTexture.SetPixels( pixels );
        croppedTexture.Apply();
        return croppedTexture;
    }
}
