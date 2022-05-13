﻿using System;
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
