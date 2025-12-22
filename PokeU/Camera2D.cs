using Microsoft.Xna.Framework;
using SFML.Graphics;

public class Camera2D
{
    public Matrix Transform
    {
        get;
        private set;
    }

    public FloatRect ViewBound
    {
        get;
        private set;
    }

    public int Zoom { get; set; }
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public Vector2 ViewSize { get; set; }

    public Camera2D()
    {
        this.Zoom = 0;
        this.Position = Vector2.Zero;
        this.Rotation = 0;
        this.ViewSize = Vector2.Zero;
        this.Position = Vector2.Zero;

        this.Transform = Matrix.Identity;
        this.ViewBound = new FloatRect();
    }

    public float Scaling
    {
        get
        {
            if (this.Zoom >= 0)
            {
                return this.Zoom + 1;
            }
            return -1f / (this.Zoom - 1);
        }
    }

    internal void UpdateTransform()
    {
        this.Transform = 
            Matrix.CreateTranslation(-Position.X, -Position.Y, 0f) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Scaling, Scaling, 1f) *
            Matrix.CreateTranslation(ViewSize.X * 0.5f, ViewSize.Y * 0.5f, 0f);
    }

    internal void UpdateViewBound()
    {
        this.ViewBound = new FloatRect(this.Position.X - (this.ViewSize.X / 2) / this.Scaling, this.Position.Y - (this.ViewSize.Y / 2) / this.Scaling, this.ViewSize.X / this.Scaling, this.ViewSize.Y / this.Scaling);
    }
}
