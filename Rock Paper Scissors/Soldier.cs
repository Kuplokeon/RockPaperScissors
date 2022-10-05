using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rock_Paper_Scissors
{
    public enum SoldierClass
    {
        Rock,
        Paper,
        Scissors,
    }

    public class Soldier
    {
        public SoldierClass soldierClass;
        public Point position;
        public Point drawSize = new Point (35);
        public float infectionDist = 5;

        public Texture2D GetTexture()
        {
            return Game1.instance.classTextures[soldierClass];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                GetTexture(),
                new Rectangle(
                    position, 
                    drawSize),
                Color.White
                );
        }

        public void Update()
        {
            Move();

            TryInfectSelf();
        }

        #region Movement
        public void Move()
        {
            int globalSpeed = Game1.instance.globalSpeed;

            //move away from random enemy
            MoveAwayFromTarget(GetRandomSoldier(GetPredators()), globalSpeed);

            //move away from closest enemy
            MoveAwayFromTarget(GetClosestSoldier(GetPredators()), globalSpeed);


            //move towards random friend
            MoveAwayFromTarget(GetRandomSoldier(GetFriends()), globalSpeed);


            //move towards random target
            MoveTowardTarget(GetRandomSoldier(GetPrey()), globalSpeed);

            //move towards closest target
            MoveTowardTarget(GetClosestSoldier(GetPrey()), 2 * globalSpeed);


            MoveAboutCenter();

            //RandomMove(3);
        }

        public void OldMove()
        {
            //move away from random enemy
            MoveAwayFromTarget(GetRandomSoldier(GetPredators()));

            //move away from closest enemy
            if (soldierClass == SoldierClass.Paper)
            {
                MoveAwayFromTarget(GetClosestSoldier(GetPredators()), 4);
            }
            else
            {
                MoveAwayFromTarget(GetClosestSoldier(GetPredators()), 2);
            }

            //move towards random friend
            if (soldierClass == SoldierClass.Rock)
            {
                MoveTowardTarget(GetRandomSoldier(GetFriends()));
            }
            else
            {
                MoveAwayFromTarget(GetRandomSoldier(GetFriends()));
            }

            //move towards random target
            MoveTowardTarget(GetRandomSoldier(GetPrey()));

            //move towards closest target
            if (soldierClass == SoldierClass.Scissors)
            {
                MoveTowardTarget(GetClosestSoldier(GetPrey()), 5);
            }
            else
            {
                MoveTowardTarget(GetClosestSoldier(GetPrey()), 2);
            }

            MoveAboutCenter();

            //RandomMove(3);
        }

        public void RandomMove(int speed = 1)
        {
            position += new Point (Game1.instance.random.Next(-1, 1) * speed, Game1.instance.random.Next(-1, 2) * speed);
        }

        public void MoveAboutCenter()
        {
            //move away from center point
            float innerRing = 50, outerRing = (Game1.instance.screenSize.Y / 2) - 100;
            Point centerPoint = GetScreenCenterPoint();
            float dist = Vector2.Distance(position.ToVector2(), centerPoint.ToVector2());
            Console.WriteLine(dist);
            
            if (dist < innerRing)
            {
                Console.WriteLine("Move away");
                MoveAwayFromTarget(centerPoint, Game1.instance.globalSpeed);
            }
            else if (dist > outerRing)
            {
                Console.WriteLine("Move in");
                MoveTowardTarget(centerPoint, ((int)dist / (int)(outerRing / 2)) * Game1.instance.globalSpeed);
            }
        }

        public void MoveTowardTarget(Soldier target, int speed = 1)
        {
            MoveTowardTarget(target.position, speed);
        }

        public void MoveTowardTarget(Point target, int speed = 1)
        {
            Point movement = AwfulNormalization(target - position);
            movement.X *= speed;
            movement.Y *= speed;
            position += movement;
        }

        public void MoveAwayFromTarget(Soldier target, int speed = 1)
        {
            MoveAwayFromTarget(target.position, speed);
        }

        public void MoveAwayFromTarget(Point target, int speed = 1)
        {
            Point movement = AwfulNormalization(target - position);
            movement.X *= speed;
            movement.Y *= speed;
            position -= movement;
        }

        public Point AwfulNormalization(Point input)
        {
            Point output = new Point();
            
            if (input.X == 0)
            {
                output.X = 0;
            } else if (input.X > 0)
            {
                output.X = 1;
            } else
            {
                output.X = -1;
            }

            if (input.Y == 0)
            {
                output.Y = 0;
            }
            else if (input.Y > 0)
            {
                output.Y = 1;
            }
            else
            {
                output.Y = -1;
            }

            return output;
        }
        #endregion

        #region General Use
        public Point GetCenterPoint(List<Soldier> listToDrawFrom)
        {
            Point output = new Point();

            foreach (Soldier soldier in Game1.instance.soldiers)
            {
                output += soldier.position;
            }

            output.X /= listToDrawFrom.Count;
            output.Y /= listToDrawFrom.Count;

            return output;
        }

        public Point GetScreenCenterPoint()
        {
            return (Game1.instance.screenSize / 2f).ToPoint();
        }

        public List<Soldier> GetPredators()
        {
            return GetSoldierOfClass(GetPredatorClass());
        }

        public List<Soldier> GetPrey()
        {
            return GetSoldierOfClass(GetPreyClass());
        }

        public List<Soldier> GetFriends()
        {
            return GetSoldierOfClass(soldierClass);
        }

        public List<Soldier> GetSoldierOfClass(SoldierClass soldierClassToCheck)
        {
            List<Soldier> output = new List<Soldier>();
            foreach (Soldier i in Game1.instance.soldiers)
            {
                if (i.soldierClass == soldierClassToCheck)
                {
                    output.Add(i);
                }
            }
            return output;
        }

        public SoldierClass GetPredatorClass()
        {
            switch (soldierClass)
            {
                case SoldierClass.Rock:
                    return SoldierClass.Paper;
                case SoldierClass.Paper:
                    return SoldierClass.Scissors;
                case SoldierClass.Scissors:
                    return SoldierClass.Rock;
            }
            return SoldierClass.Paper;
        }

        public SoldierClass GetPreyClass()
        {
            switch (soldierClass)
            {
                case SoldierClass.Rock:
                    return SoldierClass.Scissors;
                case SoldierClass.Paper:
                    return SoldierClass.Rock;
                case SoldierClass.Scissors:
                    return SoldierClass.Paper;
            }
            return SoldierClass.Paper;
        }

        public Soldier GetClosestSoldier(List<Soldier> listToDrawFrom)
        {
            if (listToDrawFrom.Count == 0)
            {
                return this; //temp value
            }

            Soldier closestSoldier = null;
            float dist = 999999;
            foreach (Soldier soldier in listToDrawFrom)
            {
                float distChecking = Vector2.Distance(position.ToVector2(), soldier.position.ToVector2());
                if (distChecking < dist)
                {
                    closestSoldier = soldier;
                    dist = distChecking;
                }
            }
            return closestSoldier;
        }

        public Soldier GetRandomSoldier(List<Soldier> listToDrawFrom)
        {
            if (listToDrawFrom.Count == 0)
            {
                return this; //temp value
            }

            return listToDrawFrom[Game1.instance.random.Next(0, listToDrawFrom.Count)];
        }
        #endregion

        #region Infection
        public void TryInfectSelf()
        {
            Soldier closestSoldier = GetClosestSoldier(GetPredators());
            if (Vector2.Distance(closestSoldier.position.ToVector2(), position.ToVector2()) < infectionDist)
            {
                soldierClass = closestSoldier.soldierClass;
            }
        }
        #endregion
    }
}
