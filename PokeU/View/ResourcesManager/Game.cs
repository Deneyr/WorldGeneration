using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private DynamicTextureAtlasManager _atlasManager;
    private List<SpriteInstance> _sprites;
    private Random _rand = new Random();

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _atlasManager = new DynamicTextureAtlasManager(GraphicsDevice, 2048, 2048, 2);

        Texture2D enemy = Content.Load<Texture2D>("enemy");
        Texture2D tree = Content.Load<Texture2D>("tree");

        _atlasManager.AddTexture("enemy", enemy);
        _atlasManager.AddTexture("tree", tree);

        _sprites = new List<SpriteInstance>(50000);
        for (int i = 0; i < 50000; i++)
        {
            string tex = _rand.NextDouble() < 0.5 ? "enemy" : "tree";
            _sprites.Add(new SpriteInstance(tex, new Vector2(_rand.Next(0, 800), _rand.Next(0, 600))));
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Petits mouvements aléatoires (optionnel)
        for (int i = 0; i < _sprites.Count; i++)
        {
            var s = _sprites[i];
            s.Position += new Vector2((float)(_rand.NextDouble() - 0.5f), (float)(_rand.NextDouble() - 0.5f));
            _sprites[i] = s;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

        for (int i = 0; i < _sprites.Count; i++)
        {
            var s = _sprites[i];

            if (_atlasManager.TryGetRegion(s.TextureId, out Texture2D atlas, out Rectangle rect, out int pageIdx))
            {
                _spriteBatch.Draw(
                    atlas,
                    s.Position,
                    rect,
                    s.Color,
                    s.Rotation,
                    Vector2.Zero,
                    s.Scale,
                    SpriteEffects.None,
                    0f
                );
            }
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}