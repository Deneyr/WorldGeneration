using Microsoft.Xna.Framework;
using SFML.Graphics;

public class Camera2D
{
    public Camera2D()
    {
        this.Zoom = 0;
        this.Position = Vector2.Zero;
        this.Rotation = 0;
        this.ViewSize = Vector2.Zero;
        this.Position = Vector2.Zero;
    }

    public int Zoom { get; set; }

    public float Scaling
    {
        get
        {
            if(this.Zoom >= 0)
            {
                return this.Zoom + 1;
            }
            return -1f / (this.Zoom - 1);
        }
    }
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public Vector2 ViewSize { get; set; }

    public FloatRect ViewBound
    {
        get
        {
            return new FloatRect(this.Position.X - (this.ViewSize.X / 2) / this.Scaling, this.Position.Y - (this.ViewSize.Y / 2) / this.Scaling, this.ViewSize.X / this.Scaling, this.ViewSize.Y / this.Scaling);
        }
    }

    public void Move(Vector2 direction)
    {
        Position += direction;
    }

    public Matrix GetTransform()
    {
        var translationMatrix = Matrix.CreateTranslation(new Vector3((int) (-this.Position.X), (int) (-this.Position.Y), 0));
        var rotationMatrix = Matrix.CreateRotationZ(this.Rotation);
        var scaleMatrix = Matrix.CreateScale(new Vector3(this.Scaling, this.Scaling, 1));
        var originMatrix = Matrix.CreateTranslation(new Vector3((int) (this.ViewSize.X / 2), (int) (this.ViewSize.Y / 2), 0));

        return translationMatrix * rotationMatrix * scaleMatrix * originMatrix;
    }
}
