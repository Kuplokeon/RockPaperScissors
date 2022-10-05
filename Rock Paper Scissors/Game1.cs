using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace Rock_Paper_Scissors
{
    public class Game1 : Game
    {
        public static Game1 instance { get; private set; }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public List<Soldier> soldiers = new List<Soldier> ();

        public Dictionary<SoldierClass, Texture2D> classTextures = new Dictionary<SoldierClass, Texture2D>();

        public Vector2 screenSize = new Vector2 ();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            bool fullscreen = false;

            if (fullscreen)
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                graphics.ApplyChanges();
                //graphics.ToggleFullScreen();
            }
            else
            {
                graphics.PreferredBackBufferWidth = 1600;
                graphics.PreferredBackBufferHeight = 900;
                graphics.ApplyChanges();
            }


            screenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            classTextures = new Dictionary<SoldierClass, Texture2D>
            {
                {SoldierClass.Rock, Content.Load<Texture2D>("images/rock") },
                {SoldierClass.Paper, Content.Load<Texture2D>("images/paper") },
                {SoldierClass.Scissors, Content.Load<Texture2D>("images/scissors") },
            };
        }

        public Random random = new Random();

        bool ranStart = false;
        public void Start()
        {
            instance = this;

            soldiers.Clear();

            for (int i = 0; i < 100; i++)
            {
                CreateNewSoldier();
            }
        }

        public Soldier CreateNewSoldier()
        {
            return CreateNewSoldier((SoldierClass)random.Next(0, 3));
        }

        public Soldier CreateNewSoldier(SoldierClass soldierClass)
        {
            Soldier newSoldier = new Soldier();

            newSoldier.soldierClass = soldierClass;
            //place in random position on screen
            newSoldier.position = new Point(
                random.Next(0, (int)screenSize.X),
                random.Next(0, (int)screenSize.Y));

            soldiers.Add(newSoldier);

            return newSoldier;
        }

        public int globalSpeed;

        public bool autoReset = false;

        protected override void Update(GameTime gameTime)
        {
            if (ranStart == false)
            {
                Start();
                ranStart = true;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Tab))
            {
                globalSpeed = 5;
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {
                    globalSpeed = 25;
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                    {
                        globalSpeed = 250;
                    }
                }
            } else
            {
                globalSpeed = 1;
            }

            #region controls
            if (Keyboard.GetState().IsKeyDown(Keys.F5))
            {
                Start();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                CreateNewSoldier();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                CreateNewSoldier(SoldierClass.Rock);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                CreateNewSoldier(SoldierClass.Paper);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                CreateNewSoldier(SoldierClass.Scissors);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F1))
            {
                autoReset = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F2))
            {
                autoReset = false;
            }

            if (autoReset)
            {
                bool timeToReset = false;
                if (GetSoldierOfClass(SoldierClass.Rock).Count == soldiers.Count)
                {
                    timeToReset = true;
                }  if (GetSoldierOfClass(SoldierClass.Paper).Count == soldiers.Count)
                {
                    timeToReset = true;
                }  if (GetSoldierOfClass(SoldierClass.Scissors).Count == soldiers.Count)
                {
                    timeToReset = true;
                }
                if (timeToReset)
                {
                    Start();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                soldiers.Clear();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.X) && soldiers.Count == 0)
            {
                CreateNewSoldier(SoldierClass.Rock);
                CreateNewSoldier(SoldierClass.Paper);
                CreateNewSoldier(SoldierClass.Scissors);
            }
            #endregion

            // TODO: Add your update logic here
            foreach (Soldier soldier in soldiers)
            {
                soldier.Update();
            }

            base.Update(gameTime);
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

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            foreach (Soldier soldier in soldiers)
            {
                soldier.Draw(spriteBatch);
            }

            GraphicsDevice.Clear(Color.Gray);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}