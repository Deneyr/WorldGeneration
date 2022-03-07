using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ChunksMonitoring;
using WorldGeneration.DataChunks;
using WorldGeneration.DataChunks.DSNoise;
using WorldGeneration.DataChunks.DSNoise.BiomeDSNoise;
using WorldGeneration.DataChunks.PerlinNoise;
using WorldGeneration.DataChunks.VoronoiNoise;
using WorldGeneration.ViewTest;
using WorldGeneration.WorldGenerating;

namespace WorldGeneration
{
    class Program
    {
        private static RenderWindow window;

        static void Main(string[] args)
        {

            //IntRect test1 = new IntRect(0, 0, 1, 3);
            //IntRect test2 = new IntRect(1, 0, 1, 1);
            //bool boo1 = test2.Intersects(test1);
            //bool boo2 = test1.Intersects(test2);

            //Console.WriteLine(boo1 + " " + boo2);




            //DSDataChunkLayer dsDataChunkLayer = new DSDataChunkLayer("biome", 7);
            //DataChunkLayersMonitor dataChunkLayerMonitor = new DataChunkLayersMonitor(123);
            //dsDataChunkLayer.DataChunksMonitor = dataChunkLayerMonitor;

            //BiomeDSDataChunkLayer biomeDSDataChunkLayer = new BiomeDSDataChunkLayer("biome", 7, 4);
            //biomeDSDataChunkLayer.DataChunksMonitor = dataChunkLayerMonitor;

            //VoronoiDataChunkLayer voronoiDataChunkLayer = new VoronoiDataChunkLayer("biome", 2, 256, 1024);
            //voronoiDataChunkLayer.DataChunksMonitor = dataChunkLayerMonitor;

            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            //sw.Start();

            ////dsDataChunkLayer.UpdateLayerArea(new IntRect(0, 0, 2, 2));
            //voronoiDataChunkLayer.UpdateLayerArea(new IntRect(0, 0, 2, 2));
            ////biomeDSDataChunkLayer.UpdateLayerArea(new IntRect(0, 0, 2, 2));

            //sw.Stop();

            //Console.WriteLine(sw.ElapsedMilliseconds / 1000f);




            WorldMonitor worldMonitor = new WorldMonitor(32, 0, 123456789);
            worldMonitor.WorldGenerator.ConstructWorldGenerator2();

            //PerlinDataChunkLayer perlinDataChunkLayer = new PerlinDataChunkLayer("landscape", 16, 8);
            //worldMonitor.WorldGenerator.AddDataLayerToGenerator(perlinDataChunkLayer);

            //ChunksMonitor chunksMonitor = new ChunksMonitor(0);

            //chunksMonitor.UpdateChunksArea(new IntRect(-3, -2, 3, 3));

            //chunksMonitor.UpdateChunksArea(new IntRect(-1, -1, 2, 2));

            //chunksMonitor.UpdateChunksArea(new IntRect(-3, -2, 3, 3));

            //IntRect area = new IntRect((int)-1920 , (int)-1080 , 1920 * 2, 1080 * 2);
            //area.Left /= 16;
            //area.Top /= 16;
            //area.Width /= 16;
            //area.Height /= 16;

            //worldMonitor.WorldArea = area;

            //worldMonitor.UpdateWorld(Time.Zero);

            //Console.Read();

            Clock clock = new Clock();

            var mode = new SFML.Window.VideoMode(1920, 1080);
            //RenderWindow window = new RenderWindow(SFML.Window.VideoMode.DesktopMode, "WorldTestGenerator", Styles.Fullscreen);
            window = new SFML.Graphics.RenderWindow(mode, "WorldTestGenerator");
            window.SetVerticalSyncEnabled(true);

            //RectangleShape rectangle = new RectangleShape(new Vector2f(16, 16));
            //rectangle.FillColor = new Color(128, 128, 128);

            //rectangle.Position = new Vector2f(0, 0);
            View view = window.DefaultView;
            view.Center = new Vector2f(0, 0);
            window.SetView(view);

            ViewMonitor viewMonitor = new ViewMonitor(view, worldMonitor);

            window.KeyPressed += OnKeyPressed;
            window.KeyPressed += viewMonitor.OnKeyPressed;

            // Start the game loop
            while (window.IsOpen)
            {
                Time deltaTime = clock.Restart();

                // Process events
                window.DispatchEvents();

                worldMonitor.UpdateWorld(deltaTime);

                // Draw window
                window.Clear();

                viewMonitor.DrawIn(window, deltaTime);

                // Finally, display the rendered frame on screen
                window.Display();
            }
        }

        private static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            if(e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
        }
    }
}
