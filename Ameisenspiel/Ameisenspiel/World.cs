using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//////////////////////////////////////////////////////////////////////////////
/// <summary>
/// implementation of World size and keeping track of entities inside.
/// </summary>
namespace Ameisenspiel {
    internal class World {

        //Width and Height are used across the Entity-Objects and must have the same value across all generated objects.
        //I am not using a Configuration-object, because this makes changes even harder later on.
        protected static int width;
        protected static int height;

        private Log log;
        protected Random random;

        protected List<Entity> worldContent = new List<Entity>();
        protected List<Entity> worldFood = new List<Entity>();
        protected List<Entity> worldHives = new List<Entity>();
        protected List<Entity> worldAnts = new List<Entity>();
        protected List<Entity> worldEggs = new List<Entity>();
        protected List<Entity> queueForAdditions = new List<Entity>();

        int hatchedAntsCounter = 0;

        //Standard World size declared here for reference
        public World() : this (85, 25) {
        }

        public World(int width, int height) {
            World.width = width;
            World.height = height;
            this.random = new Random();
            log = new Log("World.cs");
            log.Add("Konstructor(): width=" + World.width + " height=" + World.height);
        }
        public static void SetWorldProperties(int width, int height) {
            World.width = width;
            World.height = height;
        }
        public static int GetWorldWidth() {
            return World.width;
        }
        public static int GetWorldHeight() {
            return World.height;
        }
        public bool AddEntity(Entity entity) {
            bool coordinateError = false;
            if (entity.GetX() > World.width || entity.GetX() < 0) {
                log.AddError("X coordinate is outside of world width. X=" + entity.GetX());
                coordinateError = true;
            }
            if (entity.GetY() > World.height || entity.GetY() < 0) {
                log.AddError("Y coordinate is outside of world height. Y=" + entity.GetY());
                coordinateError = true;
            }

            if (coordinateError) {
                return false;
            }
            this.worldContent.Add(entity);
            if(entity.GetType().Name == typeof(Food).Name) {
                worldFood.Add(entity);
            }

            if(entity.GetType().Name == typeof(Hive).Name) {
                worldHives.Add(entity);
            }

            //regular Ants only
            if ( entity.GetType().Name == typeof(Ant).Name
                || entity.GetType().IsSubclassOf(typeof(Ant))
                && entity.GetType().Name != typeof(DebugAnt).Name
                && entity.GetType().Name != typeof(QueenAnt).Name
                && entity.GetType().Name != typeof(AntEgg).Name
                ) {
                if (entity.GetType().Name != typeof(AntEgg).Name) {
                    worldAnts.Add(entity);
                } else {
                    worldEggs.Add(entity);
                }
                Hive hive = (Hive)worldHives.First();
                Ant ant = (Ant)entity;
                hive.AddOwnedAnt(ant);

                log.Add("added Ant id(" + entity.GetEntityId() + ")");
            }
            return true; //@todo: hier ist es möglich Kollision zu implementieren
        }

        public List<Entity> GetContent() { 
            return this.worldContent;
        }

        public List<Entity> GetFood() {
            return worldFood;
        }

        /// <summary>
        /// Supposed to contain all Ants except Queen and Debug ones
        /// Update AddEntity if additional Ant-Types get made!
        /// </summary>
        /// <returns></returns>
        public List<Entity> GetAnts() {
            return worldAnts;
        }

        public List<Entity> GetWorldHives() {
            return worldHives;
        }

        public List<Entity> GetWorldEggs() {
            return worldEggs;
        }

        public void DestroyEntity(Entity entity) {
            entity.DestroyChainLinks();
            this.worldContent.Remove(entity);
            if(entity.GetType().Name == typeof(Ant).Name
                || entity.GetType().IsSubclassOf(typeof(Ant))) {
                worldAnts.Remove(entity);
            }
            if (entity.GetType().Name == typeof(AntEgg).Name) {
                worldEggs.Remove(entity);
                worldEggs.TrimExcess();
            }
            worldContent.TrimExcess();
            worldAnts.TrimExcess();
            worldFood.Remove(entity);
            worldFood.TrimExcess();
        }

        public void AddEntityToQueue(Entity entity) {
            queueForAdditions.Add(entity);
        }

        public void HandleQueue() {
            foreach (Entity entity in queueForAdditions) {
                AddEntity(entity);
                if(entity.GetType().Name == typeof(Ant).Name
                || entity.GetType().IsSubclassOf(typeof(Ant))
                && entity.GetType().Name != typeof(AntEgg).Name ) {
                    hatchedAntsCounter++;
                }
            }
            queueForAdditions.Clear();
        }

        public int GetHatchedAntsCounter() {
            return hatchedAntsCounter;
        }
    }
}
