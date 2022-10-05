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

            for (int i = 0; i < 100; i++)
            {
                Soldier newSoldier = new Soldier();

                newSoldier.soldierClass = (SoldierClass)random.Next(0, 3);
                //place in random position on screen
                newSoldier.position = new Point(
                    random.Next(0, (int)screenSize.X),
                    random.Next(0, (int)screenSize.Y));

                soldiers.Add(newSoldier);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (ranStart == false)
            {
                Start();
                ranStart = true;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            foreach (Soldier soldier in soldiers)
            {
                soldier.Update();
            }

            base.Update(gameTime);
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