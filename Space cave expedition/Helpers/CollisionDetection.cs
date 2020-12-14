﻿using System;
using System.Collections.Generic;
using System.Text;

using Space_cave_expedition.Models;
using Space_cave_expedition.Enums;

namespace Space_cave_expedition.Helpers
{
    public class CollisionDetection
    {
        /// <summary>
        /// Returns, whether the entity can move in a specific direction.
        /// Use only for entities of size 1x1 that move by one field at a time!
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="movementDirection"></param>
        /// <returns></returns>
        public static bool DetectCollision(char[,] MapLayout, ControllableEntity entity, PlayerMoveDirection movementDirection)
        {
            switch (movementDirection)
            {
                case PlayerMoveDirection.Up:
                    return MapLayout[entity.XPosition, entity.YPosition - 1] == ' ';
                case PlayerMoveDirection.Down:
                    return MapLayout[entity.XPosition, entity.YPosition + 1] == ' ';
                case PlayerMoveDirection.Left:
                    return MapLayout[entity.XPosition - 1, entity.YPosition] == ' ';
                case PlayerMoveDirection.Right:
                    return MapLayout[entity.XPosition + 1, entity.YPosition] == ' ';
                default:
                    throw new ArgumentException("Error, unexpected movement direction.");
            }
        }
    }
}
