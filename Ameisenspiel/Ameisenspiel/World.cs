using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// implementation of World size, time that passes in "world ticks", keeping track of entities inside.
/// One "world tick" equals 1/120 of one second.
/// For example one ant moves one tile in 1 second
/// </summary>
namespace Ameisenspiel {
    internal class World {
        private Log log = new Log("World.cs");
        protected static int width=85;
        protected static int height=25;
        protected Random random;

        protected List<Entity> worldContent = new List<Entity>();
        public World() {
            this.random = new Random();
        }
        public static void SetWorldProperties(int width, int height) {
            World.width = width;
            World.height = height;
        }
        public static int GetWorldWidth() {
            return width;
        }
        public static int GetWorldHeight() {
            return height;
        }
        /*
        public struct Position {
            public int x;
            public int y;
            public Entity entity;
            //@todo: drawsymbol as well?
            public bool destroyNow;
            public Position(int x, int y, Entity entity) {
                this.x = x; this.y = y; 
                
                this.entity = entity; 
                
                destroyNow = false;
            }
        }
        */
        /*
        public World() {
            //Standardwerte ist Kommandozeilenfensterbreite
            this.width = 80;
            this.height = 25;
            this.random = new Random();
        }
        */

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
            return true; //@todo: hier ist es möglich Kollision zu implementieren
        }

        public List<Entity> GetContent() { 
            return this.worldContent;
        }

        public void DestroyEntity(Entity entity) {
            this.worldContent.Remove(entity);
        }
    }
}
