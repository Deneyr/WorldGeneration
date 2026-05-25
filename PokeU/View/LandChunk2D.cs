using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using System.Linq;
using WorldGeneration.ObjectChunks;
using WorldGeneration.ObjectChunks.ObjectLands;

namespace PokeU.View
{
    public class LandChunk2D : AObject2D
    {
        private List<LandCase2D[,]> landObjects2DLayers;

        private int currentAltitude;

        public int CurrentAltitude
        {
            get
            {
                return this.currentAltitude;
            }
            set
            {
                int realNewAltitude = value;
                if (realNewAltitude < 0)
                {
                    realNewAltitude = 0;
                }
                else if(realNewAltitude >= this.landObjects2DLayers.Count)
                {
                    realNewAltitude = this.landObjects2DLayers.Count - 1;
                }

                if (realNewAltitude != this.currentAltitude)
                {
                    this.currentAltitude = realNewAltitude;

                    this.UpdateCurrentAltitude();
                }
            }
        }

        public LandChunk2D(LandWorld2D landWorld2D, IObjectChunk landChunk)
        {
            this.TextureRect = new Rectangle(0, 0, landChunk.NbCaseSide * MainGame.MODEL_TO_VIEW, landChunk.NbCaseSide * MainGame.MODEL_TO_VIEW);

            this.landObjects2DLayers = new List<LandCase2D[,]>();

            int altitudeMax = (landChunk.GetCaseAtLocal(0, 0) as IZObjectCase).NbAltitudeLevel;

            for (int z = 0; z < altitudeMax; z++)
            {
                LandCase2D[,] landObject2Ds = new LandCase2D[landChunk.NbCaseSide, landChunk.NbCaseSide];

                LandCase2DFactory caseObject2DFactory = LandWorld2D.MappingObjectModelView[typeof(LandCase)] as LandCase2DFactory;
                caseObject2DFactory.CurrentObjectChunk = landChunk;

                for (int i = 0; i < landChunk.NbCaseSide; i++)
                {
                    for (int j = 0; j < landChunk.NbCaseSide; j++)
                    {
                        IZObjectCase zObjectCase = landChunk.GetCaseAtLocal(j, i) as IZObjectCase;

                        ObjectCase objectCase = zObjectCase[z] as ObjectCase;
                        if (objectCase != null)
                        {
                            LandCase landCase = objectCase.Land;

                            if (landCase != null)
                            {
                                LandCase2D landCase2D = caseObject2DFactory.CreateObject2D(landWorld2D, landCase, new Point(zObjectCase.Position.X * MainGame.MODEL_TO_VIEW, zObjectCase.Position.Y * MainGame.MODEL_TO_VIEW)) as LandCase2D;
                                landObject2Ds[i, j] = landCase2D;

                                if (z < altitudeMax - 1 && zObjectCase[z + 1] != null)
                                {
                                    LandCase landCaseUp = (zObjectCase[z + 1] as ObjectCase).Land;
                                    landCase2D.UpdateOverLandCase(landCaseUp);
                                }
                                if (z > 0 && zObjectCase[z - 1] != null)
                                {
                                    LandCase landCaseDown = (zObjectCase[z - 1] as ObjectCase).Land;
                                    landCase2D.UpdateUnderLandCase(landCaseDown);
                                }

                                landCase2D.SetLandCaseRatio(z, LandWorld2D.LOADED_ALTITUDE_RANGE);
                            }
                            else
                            {
                                landObject2Ds[i, j] = null;
                            }
                        }
                        else
                        {
                            landObject2Ds[i, j] = null;
                        }
                    }
                }

                this.landObjects2DLayers.Add(landObject2Ds);
            }

            this.currentAltitude = -1;
            this.CurrentAltitude = landWorld2D.CurrentAltitude;

            this.Position = new Vector2(landChunk.Position.X * landChunk.NbCaseSide * MainGame.MODEL_TO_VIEW, landChunk.Position.Y * landChunk.NbCaseSide * MainGame.MODEL_TO_VIEW);
        }

        public void UpdateCurrentAltitude()
        {
            int z = 0;
            foreach (LandCase2D[,] landCases in this.landObjects2DLayers)
            {
                int nbCaseSide = landCases.GetLength(0);

                for (int i = 0; i < nbCaseSide; i++)
                {
                    for (int j = 0; j < nbCaseSide; j++)
                    {
                        if (landCases[i, j] != null)
                        {
                            landCases[i, j].SetLandCaseRatio(z - this.CurrentAltitude, LandWorld2D.LOADED_ALTITUDE_RANGE);
                        }
                    }
                }
                z++;
            }
        }

        public override void DrawIn(SpriteBatch spriteBatch, ref FloatRect boundsView)
        {
            LandCase2D[,] layer2D = this.landObjects2DLayers.FirstOrDefault();

            if (layer2D == null)
            {
                return;
            }

            FloatRect landChunkViewBound = this.ViewBound;

            int offsetLeft = (int)Math.Floor((Math.Max(boundsView.Left, landChunkViewBound.Left) - landChunkViewBound.Left) / MainGame.MODEL_TO_VIEW);
            int offsetTop = (int)Math.Floor((Math.Max(boundsView.Top, landChunkViewBound.Top) - landChunkViewBound.Top) / MainGame.MODEL_TO_VIEW);

            float landChunkRight = landChunkViewBound.Left + landChunkViewBound.Width;
            float landChunkBottom = landChunkViewBound.Top + landChunkViewBound.Height;

            int offsetRight = (int)Math.Floor((landChunkRight - Math.Min(boundsView.Left + boundsView.Width, landChunkRight)) / MainGame.MODEL_TO_VIEW);
            int offsetBottom = (int)Math.Floor((landChunkBottom - Math.Min(boundsView.Top + boundsView.Height, landChunkBottom)) / MainGame.MODEL_TO_VIEW);

            foreach (LandCase2D[,] landObject2DsArray in this.landObjects2DLayers)
            {
                for (int i = offsetTop; i < layer2D.GetLength(0) - offsetBottom; i++)
                {
                    for (int j = offsetLeft; j < layer2D.GetLength(1) - offsetRight; j++)
                    {
                        LandCase2D landObjectsList = landObject2DsArray[i, j];
                        if (landObjectsList != null)
                        {
                            landObjectsList.DrawIn(spriteBatch, ref boundsView);
                        }
                    }
                }
            }
        }

        public override void RenderIn(Vector2 renderPosition, SpriteBatch spriteBatch, ref FloatRect boundsView)
        {
            LandCase2D[,] layer2D = this.landObjects2DLayers.FirstOrDefault();

            if (layer2D == null)
            {
                return;
            }

            for (int i = 0; i < layer2D.GetLength(0); i++)
            {
                for (int j = 0; j < layer2D.GetLength(1); j++)
                {
                    renderPosition.X = j * MainGame.MODEL_TO_VIEW;
                    renderPosition.Y = i * MainGame.MODEL_TO_VIEW;

                    foreach (LandCase2D[,] landObject2DsArray in this.landObjects2DLayers)
                    {
                        LandCase2D landObjectsList = landObject2DsArray[i, j];
                        if (landObjectsList != null)
                        {
                            landObjectsList.RenderIn(renderPosition, spriteBatch, ref boundsView);
                        }
                    }
                }
            }
        }

        public override void Dispose()
        {
            foreach (LandCase2D[,] landObject2DsArray in this.landObjects2DLayers)
            {
                for (int i = 0; i < landObject2DsArray.GetLength(0); i++)
                {
                    for (int j = 0; j < landObject2DsArray.GetLength(1); j++)
                    {
                        LandCase2D landObjectsList = landObject2DsArray[i, j];

                        if (landObjectsList != null)
                        {
                            landObjectsList.Dispose();
                        }
                    }
                }
            }

            this.landObjects2DLayers.Clear();
        }
    }
}
