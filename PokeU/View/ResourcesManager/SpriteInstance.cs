using Microsoft.Xna.Framework;

public struct SpriteInstance
{
    public string TextureId;
    public Vector2 Position;
    public float Rotation;
    public Vector2 Scale;
    public Color Color;

    public SpriteInstance(string id, Vector2 pos)
    {
        TextureId = id;
        Position = pos;
        Rotation = 0f;
        Scale = Vector2.One;
        Color = Color.White;
    }
}