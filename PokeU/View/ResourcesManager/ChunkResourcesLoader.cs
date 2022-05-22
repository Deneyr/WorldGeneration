using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldGeneration.ObjectChunks;

namespace PokeU.View.ResourcesManager
{
    public class ChunkResourcesLoader
    {
        private static readonly int NB_MAX_CACHE_CHUNKS = 0;

        //private static readonly int NB_MAX_CACHE_ENTITIES = 0;

        private HashSet<IntRect> loadedChunks;

        private List<IntRect> chunksInCache;

        private Dictionary<string, HashSet<IntRect>> pathToChunksDictionary;

        private Dictionary<IntRect, HashSet<string>> chunksToPathsDictionary;

        // Entities
        //private HashSet<IEntity> loadedEntities;

        //private List<IEntity> entitiesInCache;

        //private Dictionary<string, HashSet<IEntity>> pathToEntitiesDictionary;

        //private Dictionary<IEntity, HashSet<string>> entitiesToPathsDictionary;



        public ChunkResourcesLoader()
        {
            this.pathToChunksDictionary = new Dictionary<string, HashSet<IntRect>>();

            this.chunksToPathsDictionary = new Dictionary<IntRect, HashSet<string>>();

            this.loadedChunks = new HashSet<IntRect>();

            this.chunksInCache = new List<IntRect>();

            // Entities
            //this.pathToEntitiesDictionary = new Dictionary<string, HashSet<IEntity>>();

            //this.entitiesToPathsDictionary = new Dictionary<IEntity, HashSet<string>>();

            //this.loadedEntities = new HashSet<IEntity>();

            //this.entitiesInCache = new List<IEntity>();
        }

        public void LoadChunkResources(IObjectChunk landChunk)
        {
            IntRect altitudeRect = new IntRect(landChunk.Position.X, landChunk.Position.Y, 0, 0);

            if (this.loadedChunks.Contains(altitudeRect))
            {
                throw new Exception("Try to load an already loaded chunk");
            }

            if (chunksInCache.Contains(altitudeRect))
            {
                chunksInCache.Remove(altitudeRect);
            }
            else
            {
                HashSet<string> resourcesPath = new HashSet<string>();

                HashSet<Type> landObjectTypes = landChunk.TypesInChunk;

                foreach (Type type in landObjectTypes)
                {
                    IEnumerable<string> resources = LandWorld2D.MappingObjectModelView[type].Resources.Keys;

                    foreach (string path in resources)
                    {
                        resourcesPath.Add(path);
                    }
                }

                HashSet<string> realResourcesToLoad = new HashSet<string>();
                foreach (string path in resourcesPath)
                {
                    if (this.pathToChunksDictionary.ContainsKey(path) == false)
                    {
                        this.pathToChunksDictionary.Add(path, new HashSet<IntRect>());

                        realResourcesToLoad.Add(path);
                    }

                    this.pathToChunksDictionary[path].Add(altitudeRect);
                }

                this.chunksToPathsDictionary.Add(altitudeRect, resourcesPath);

                if (realResourcesToLoad.Any())
                {
                    LandWorld2D.TextureManager.LoadTextures(realResourcesToLoad);
                }
            }
            loadedChunks.Add(altitudeRect);
        }

        public void UnloadChunkResources(IObjectChunk landChunk)
        {
            IntRect altitudeRect = new IntRect(landChunk.Position.X, landChunk.Position.Y, 0, 0);

            if (this.loadedChunks.Contains(altitudeRect) == false)
            {
                throw new Exception("Try to unload a not loaded chunk");
            }

            this.chunksInCache.Add(altitudeRect);

            if (this.chunksInCache.Count > NB_MAX_CACHE_CHUNKS)
            {
                IntRect altitudeToRemove = this.chunksInCache.First();
                this.chunksInCache.RemoveAt(0);

                HashSet<string> pathsAltitudeToRemove = this.chunksToPathsDictionary[altitudeToRemove];
                HashSet<string> pathsToRemove = new HashSet<string>();

                foreach (string path in pathsAltitudeToRemove)
                {
                    HashSet<IntRect> altitudes = this.pathToChunksDictionary[path];
                    altitudes.Remove(altitudeRect);

                    if (altitudes.Any() == false)
                    {
                        pathsToRemove.Add(path);
                        this.pathToChunksDictionary.Remove(path);
                    }
                }

                this.chunksToPathsDictionary.Remove(altitudeToRemove);

                if (pathsToRemove.Any())
                {
                    LandWorld2D.TextureManager.UnloadTextures(pathsToRemove);
                }
            }

            this.loadedChunks.Remove(altitudeRect);
        }

        // Entities
        //public void LoadEntitiesResources(IEntity entity)
        //{

        //    if (this.loadedEntities.Contains(entity))
        //    {
        //        throw new Exception("Try to load an already loaded entity");
        //    }

        //    if (entitiesInCache.Contains(entity))
        //    {
        //        entitiesInCache.Remove(entity);
        //    }
        //    else
        //    {
        //        HashSet<string> resourcesPath = new HashSet<string>();

        //        Type type = entity.GetType();

        //        IEnumerable<string> resources = LandWorld2D.MappingObjectModelView[type].Resources.Keys;

        //        foreach (string path in resources)
        //        {
        //            resourcesPath.Add(path);
        //        }

        //        HashSet<string> realResourcesToLoad = new HashSet<string>();
        //        foreach (string path in resourcesPath)
        //        {
        //            if (this.pathToEntitiesDictionary.ContainsKey(path) == false)
        //            {
        //                this.pathToEntitiesDictionary.Add(path, new HashSet<IEntity>());

        //                realResourcesToLoad.Add(path);
        //            }

        //            this.pathToEntitiesDictionary[path].Add(entity);
        //        }

        //        this.entitiesToPathsDictionary.Add(entity, resourcesPath);

        //        if (realResourcesToLoad.Any())
        //        {
        //            LandWorld2D.TextureManager.LoadTextures(realResourcesToLoad);
        //        }
        //    }
        //    loadedEntities.Add(entity);
        //}

        //public void UnloadEntitiesResources(IEntity entity)
        //{

        //    if (this.loadedEntities.Contains(entity) == false)
        //    {
        //        throw new Exception("Try to unload a not loaded entity");
        //    }

        //    this.entitiesInCache.Add(entity);

        //    if (this.entitiesInCache.Count > NB_MAX_CACHE_ENTITIES)
        //    {
        //        IEntity entityToRemove = this.entitiesInCache.First();
        //        this.entitiesInCache.RemoveAt(0);

        //        HashSet<string> pathsEntitiesToRemove = this.entitiesToPathsDictionary[entityToRemove];
        //        HashSet<string> pathsToRemove = new HashSet<string>();

        //        foreach (string path in pathsEntitiesToRemove)
        //        {
        //            HashSet<IEntity> entities = this.pathToEntitiesDictionary[path];
        //            entities.Remove(entity);

        //            if (entities.Any() == false)
        //            {
        //                pathsToRemove.Add(path);
        //                this.pathToEntitiesDictionary.Remove(path);
        //            }
        //        }

        //        this.entitiesToPathsDictionary.Remove(entity);

        //        if (pathsToRemove.Any())
        //        {
        //            LandWorld2D.TextureManager.UnloadTextures(pathsToRemove);
        //        }
        //    }

        //    this.loadedEntities.Remove(entity);
        //}
    }
}
