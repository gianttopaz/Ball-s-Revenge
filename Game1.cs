using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace Timers.GameStates
{

    enum Scenes
    {
        START,
        GAME,
        GAMEOVER

    };
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        int seconds;
        int timer;
        int respawnTimer;
        float spawn = 0;

        Song song1;
        Song song2;
        SoundEffect hit;
        SoundEffect play;

        Random random = new Random();
        List<Target> targets;
        Crosshair crosshair;
        SpriteFont font;
        SpriteFont fontBig;
        private Texture2D titleScreen;
        private Texture2D gameoverScreen;

        private Scenes activeScene;

        KeyboardState kstate = Keyboard.GetState();
        KeyboardState prevKBState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = Globals.WIDTH;
            _graphics.PreferredBackBufferHeight = Globals.HEIGHT;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            activeScene = Scenes.START;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            timer = 20;

            crosshair = new Crosshair();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            targets = new();

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.spriteBatch = new SpriteBatch(GraphicsDevice);

            Globals.pixel = new Texture2D(GraphicsDevice, 1, 1);
            Globals.pixel.SetData<Color>(new Color[] { Color.Red });
            Globals.pixel2 = new Texture2D(GraphicsDevice, 1, 1);

            Globals.pixel2.SetData<Color>(new Color[] { Color.White });
            targets.Add(new Target());
            titleScreen = Content.Load<Texture2D>("TitleScreen");
            gameoverScreen = Content.Load<Texture2D>("GameoverScreen");
            font = Content.Load<SpriteFont>("Score");
            fontBig = Content.Load<SpriteFont>("FinalScore");

            song1 = Content.Load<Song>("_VanillaDome");
            song2 = Content.Load<Song>("_KoopaCastle");
            hit = Content.Load<SoundEffect>("smw_kick");
            play = Content.Load<SoundEffect>("shatter");
            MediaPlayer.Play(song1);

            //attempt to align scores
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {


            // TODO: Add your update logic here
            KeyboardState kstate = Keyboard.GetState();


            switch (activeScene)
            {

                case Scenes.START:

                    if (kstate.IsKeyDown(Keys.Space) && !prevKBState.IsKeyDown(Keys.Space))
                    {
                        play.Play();
                        activeScene = Scenes.GAME;
                        MediaPlayer.Play(song2);
                        seconds = 60;



                    }
                    prevKBState = kstate;


                    break;
                case Scenes.GAME:

                    seconds--;
                    if (seconds == 0)
                    {
                        timer--;
                        seconds = 60;
                    }
                    if (timer == 0)
                    {
                        timer = 20;
                        MediaPlayer.Volume = 50;

                        activeScene = Scenes.GAMEOVER;

                    }


                    spawn += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    List<Target> killList = new();
                    crosshair.Update(gameTime);

                    foreach (var target in targets)
                    {
                        target.Update(gameTime);


                        //make it so space must be pressed after touching rectangle
                        if (kstate.IsKeyDown(Keys.Space) && !prevKBState.IsKeyDown(Keys.Space) && target.rect.Intersects(crosshair.rect))
                        {
                            killList.Add(target);
                            Globals.score += 1;
                            hit.Play();

                        }
                        if (target.despawn >= 4)
                        {
                            killList.Add(target);
                        }


                    }
                    prevKBState = kstate;

                    foreach (var target in killList)
                    {
                        targets.Remove(target);

                    }
                    break;

                case Scenes.GAMEOVER:


                    seconds--;
                    if (seconds == 1)
                        seconds = 1;

                    if (kstate.IsKeyDown(Keys.Space) && !prevKBState.IsKeyDown(Keys.Space) && seconds < 15)
                    {
                        activeScene = Scenes.START;
                        MediaPlayer.Play(song1);
                        Globals.score = 0;



                    }
                    prevKBState = kstate;

                    break;


            }




            LoadEnemies();

            base.Update(gameTime);
        }

        //spawns new targets...?
        public void LoadEnemies()
        {
            int randY = random.Next(50, Globals.WIDTH - 80);

            if (spawn >= 1)
            {
                spawn = 0;
                if (targets.Count < 5)
                    targets.Add(new Target());
            }


            //for (int i = 0; i < targets.Count; i++)

        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);


            switch (activeScene)
            {
                case Scenes.START:

                    _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
                    _spriteBatch.Draw(titleScreen, new Rectangle(60, 25, 512, 392), Color.White);
                    _spriteBatch.DrawString(font, "PRESS SPACE BAR TO START", new Vector2((Globals.WIDTH / 2) - 190, 410), Color.White);
                    _spriteBatch.End();



                    break;
                case Scenes.GAME:

                    Globals.spriteBatch.Begin();
                    Globals.spriteBatch.DrawString(font,"TIME" + "\n" + timer.ToString(), new Vector2(20, 15), Color.White);
                    Globals.spriteBatch.DrawString(font,"SCORE" + "\n" + Globals.score.ToString(), new Vector2(Globals.WIDTH-175, 15), Color.White);

                    foreach (var target in targets)
                    {
                        target.Draw();
                    }
                    crosshair.Draw();

                    Globals.spriteBatch.End();
                    break;
                case Scenes.GAMEOVER:

                    _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
                    _spriteBatch.Draw(gameoverScreen, new Rectangle(60, -20, 512, 392), Color.White);
                    _spriteBatch.End();

                    Globals.spriteBatch.Begin();
                    Globals.spriteBatch.DrawString(font, "FINAL SCORE", new Vector2((Globals.WIDTH/2) - 90, 360), Color.White);
                    Globals.spriteBatch.DrawString(fontBig, Globals.score.ToString(), new Vector2((Globals.WIDTH/2) - 30, 390), Color.Yellow);
                    Globals.spriteBatch.DrawString(font, "TRY AGAIN?", new Vector2((Globals.WIDTH/2) - 85, 445), Color.White);
                    Globals.spriteBatch.End();


                    break;
            }



            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
