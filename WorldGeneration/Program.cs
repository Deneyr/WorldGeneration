using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks.PerlinNoise;
using WorldGeneration.WorldGenerating;

namespace WorldGeneration
{
    class Program
    {
        static void Main(string[] args)
        {

            //IntRect test1 = new IntRect(0, 0, 1, 3);
            //IntRect test2 = new IntRect(1, 0, 1, 1);
            //bool boo1 = test2.Intersects(test1);
            //bool boo2 = test1.Intersects(test2);

            //Console.WriteLine(boo1 + " " + boo2);

            WorldMonitor worldMonitor = new WorldMonitor(16, 0, 123456789);

            worldMonitor.WorldGenerator.ConstructWorldGenerator();

            //PerlinDataChunkLayer perlinDataChunkLayer = new PerlinDataChunkLayer("landscape", 16, 8);
            //worldMonitor.WorldGenerator.AddDataLayerToGenerator(perlinDataChunkLayer);

            //ChunksMonitor chunksMonitor = new ChunksMonitor(0);

            //chunksMonitor.UpdateChunksArea(new IntRect(-3, -2, 3, 3));

            //chunksMonitor.UpdateChunksArea(new IntRect(-1, -1, 2, 2));

            //chunksMonitor.UpdateChunksArea(new IntRect(-3, -2, 3, 3));

            IntRect area = new IntRect((int)-800 / 2, (int)-800 / 2, 800, 600);
            area.Left /= 16;
            area.Top /= 16;
            area.Width /= 16;
            area.Height /= 16;

            worldMonitor.WorldArea = area;

            worldMonitor.UpdateWorld(Time.Zero);

            //Console.Read();

            Clock clock = new Clock();

            var mode = new SFML.Window.VideoMode(800, 600);
            //RenderWindow window = new RenderWindow(SFML.Window.VideoMode.DesktopMode, "WorldTestGenerator", Styles.Fullscreen);
            RenderWindow window = new SFML.Graphics.RenderWindow(mode, "WorldTestGenerator");
            window.SetVerticalSyncEnabled(true);

            RectangleShape rectangle = new RectangleShape(new Vector2f(16, 16));
            rectangle.FillColor = new Color(128, 128, 128);

            rectangle.Position = new Vector2f(0, 0);
            View view = window.DefaultView;
            view.Center = new Vector2f(0, 0);
            window.SetView(view);

            // Start the game loop
            while (window.IsOpen)
            {
                Time deltaTime = clock.Restart();

                // Process events
                window.DispatchEvents();

                worldMonitor.UpdateWorld(deltaTime);

                // Draw window
                window.Clear();

                window.Draw(rectangle);

                // Finally, display the rendered frame on screen
                window.Display();
            }
        }
    }
}
