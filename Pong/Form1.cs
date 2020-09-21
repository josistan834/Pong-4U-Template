/*
 * Description:     A epic pong game
 * Author:          Josiah Stanley
 * Date:            September 21 2020
 */

#region libraries

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;

#endregion

namespace Pong
{
    public partial class Form1 : Form
    {
        #region global values

        //graphics objects for drawing
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        Font drawFont = new Font("Courier New", 20);

        // Sounds for game
        SoundPlayer scoreSound = new SoundPlayer(Properties.Resources.score);
        SoundPlayer collisionSound = new SoundPlayer(Properties.Resources.collision);

        //determines whether a key is being pressed or not
        Boolean aKeyDown, zKeyDown, jKeyDown, mKeyDown;

        // check to see if a new game can be started
        Boolean newGameOk = true;

        //ball directions, speed, and rectangle
        Boolean ballMoveRight = true;
        Boolean ballMoveDown = true;
        int BALL_SPEED = 4;
        Rectangle ball;

        //paddle speeds and rectangles
        int PADDLE_SPEED = 4;
        Rectangle p1, p2;

        //player and game scores
        int player1Score = 0;
        int player2Score = 0;

        //mode switch
        bool bonusMode;

        //Extra
        int counter = 0;
        Random randGen = new Random();
        int R, G, B;
        #endregion

        public Form1()
        {
            //Starting and loading the objects in the form
            InitializeComponent();
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.A:
                    aKeyDown = true;
                    break;
                case Keys.Z:
                    zKeyDown = true;
                    break;
                case Keys.J:
                    jKeyDown = true;
                    break;
                case Keys.M:
                    mKeyDown = true;
                    break;
                case Keys.D2:
                    if (newGameOk)
                    {
                        bonusMode = false;
                        SetParameters();
                    }
                    break;
                case Keys.D1:
                    if (newGameOk)
                    {
                        bonusMode = true;
                        SetParameters();
                    }
                    break;
                case Keys.N:
                    if (newGameOk)
                    {
                        Close();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //check to see if a key has been released and set its KeyDown value to false if it has
            switch (e.KeyCode)
            {
                case Keys.A:
                    aKeyDown = false;
                    break;
                case Keys.Z:
                    zKeyDown = false;
                    break;
                case Keys.J:
                    jKeyDown = false;
                    break;
                case Keys.M:
                    mKeyDown = false;
                    break;
            }
        }


        /// <summary>
        /// sets the ball and paddle positions for game start
        /// </summary>
        private void SetParameters()
        {
            if (newGameOk)
            {
                player1Score = player2Score = 0;
                newGameOk = false;
                startLabel.Visible = false;
                BALL_SPEED = 4;
                PADDLE_SPEED = 4;
                gameUpdateLoop.Start();
            }

            //set starting position for paddles on new game and point scored 
            const int PADDLE_EDGE = 20;  // buffer distance between screen edge and paddle            

            p1.Width = p2.Width = 10;    //height for both paddles set the same
            p1.Height = p2.Height = 40;  //width for both paddles set the same

            //p1 starting position
            p1.X = PADDLE_EDGE;
            p1.Y = this.Height / 2 - p1.Height / 2;

            //p2 starting position
            if (!bonusMode)
            {
                p2.X = this.Width - PADDLE_EDGE - p2.Width;
                p2.Y = this.Height / 2 - p2.Height / 2;
            }
            else
            {
                p2.Y = 0;
                p2.X = this.Width - 10;
                p2.Height = this.Height * 2;
            }


            // TODO set Width and Height of ball
            ball.Width = 15;
            ball.Height = 15;
            // TODO set starting X position for ball to middle of screen, (use this.Width and ball.Width)
            ball.X = this.Width / 2;
            // TODO set starting Y position for ball to middle of screen, (use this.Height and ball.Height)
            ball.Y = this.Height / 2;

        }

        /// <summary>
        /// This method is the game engine loop that updates the position of all elements
        /// and checks for collisions.
        /// </summary>
        private void gameUpdateLoop_Tick(object sender, EventArgs e)
        {
            #region update ball position
            if (bonusMode)
            {
                if (counter < 1000)
                {
                    counter++;
                }

                else
                {
                    BALL_SPEED = BALL_SPEED + (counter / 1000);
                    counter = 0;
                }

                if (player1Score == 5)
                {
                    PADDLE_SPEED *= 2;
                    whiteBrush.Color = Color.Aqua;
                }

                else if (player1Score == 10)
                {
                    PADDLE_SPEED *= 2;
                    whiteBrush.Color = Color.Orange;
                }

                else if (player1Score == 15)
                {
                    PADDLE_SPEED *= 2;
                    whiteBrush.Color = Color.LimeGreen;
                }

                else if (player1Score == 20)
                {
                    PADDLE_SPEED *= 2;
                    whiteBrush.Color = Color.Purple;
                }
    
                if (player1Score >= 25)
                {
                    if (counter % 20 == 0)
                    {
                        R = randGen.Next(1, 255);
                        G = randGen.Next(1, 255);
                        B = randGen.Next(1, 255);

                        whiteBrush.Color = Color.FromArgb(R, G, B);
                    }
                    
                }


            }

            // TODO create code to move ball either left or right based on ballMoveRight and using BALL_SPEED
            if (ballMoveRight == true)
            {
                ball.X = ball.X + BALL_SPEED;
            }
            else
            {
                ball.X = ball.X - BALL_SPEED;
            }

            // TODO create code move ball either down or up based on ballMoveDown and using BALL_SPEED
            if (ballMoveDown)
            {
                ball.Y = ball.Y - BALL_SPEED;
            }
            else
            {
                ball.Y = ball.Y + BALL_SPEED;
            }


            #endregion

            #region update paddle positions

            if (aKeyDown == true && p1.Y > 0)
            {
                // TODO create code to move player 1 paddle up using p1.Y and PADDLE_SPEED
                p1.Y = p1.Y - PADDLE_SPEED;
            }

            if (zKeyDown == true && p1.Y < (this.Height - p1.Height))
            {
                // TODO create an if statement and code to move player 1 paddle down using p1.Y and PADDLE_SPEED
                p1.Y = p1.Y + PADDLE_SPEED;
            }
            if (!bonusMode)
            {


                if (jKeyDown == true && p2.Y > 0)
                {
                    // TODO create an if statement and code to move player 2 paddle up using p2.Y and PADDLE_SPEED
                    p2.Y = p2.Y - PADDLE_SPEED;
                }

                if (mKeyDown == true && p2.Y < (this.Height - p1.Height))
                {
                    // TODO create an if statement and code to move player 2 paddle down using p2.Y and PADDLE_SPEED
                    p2.Y = p2.Y + PADDLE_SPEED;
                }
            }


            #endregion

            #region ball collision with top and bottom lines

            if (ball.Y < 0) // if ball hits top line
            {
                // TODO use ballMoveDown boolean to change direction
                ballMoveDown = false;
                // TODO play a collision sound
                collisionSound.Play();
            }
            // TODO In an else if statement use ball.Y, this.Height, and ball.Width to check for collision with bottom line
            if (ball.Y > (this.Height - ball.Height))
            {
                // If true use ballMoveDown down boolean to change direction
                ballMoveDown = true;
                collisionSound.Play();
            }


            #endregion

            #region ball collision with paddles
            if (!bonusMode)
            {
                if (ball.IntersectsWith(p1) || ball.IntersectsWith(p2))
                {
                    // --- play a "paddle hit" sound and
                    collisionSound.Play();
                    // --- use ballMoveRight boolean to change direction
                    ballMoveRight = !ballMoveRight;
                }
            }
            // TODO create if statment that checks player collides with ball and if it does
            if (bonusMode)
            {
                if (ball.IntersectsWith(p2))
                {
                    collisionSound.Play();
                    ballMoveRight = !ballMoveRight;
                }
                else if (ball.IntersectsWith(p1))
                {
                    collisionSound.Play();
                    ballMoveRight = !ballMoveRight;
                    player1Score++;
                }
            }



            /*  ENRICHMENT
             *  Instead of using two if statments as noted above see if you can create one
             *  if statement with multiple conditions to play a sound and change direction
             */

            #endregion

            #region ball collision with side walls (point scored)

            if (ball.X < 0)  // ball hits left wall logic
            {
                // TODO
                // --- play score sound
                scoreSound.Play();
                // --- update player 2 score
                player2Score++;

                // TODO use if statement to check to see if player 2 has won the game. If true run GameOver method.
                if (player2Score == 5 && !bonusMode)
                {
                    GameOver("Player 2 Wins!");
                }
                else if (player2Score == 1 && bonusMode)
                {
                    GameOver($"Your score was {player1Score}");
                }
                //Else change direction of ball and call SetParameters method.
                else
                {
                    ballMoveRight = !ballMoveRight;
                    SetParameters();
                }

            }

            // TODO same as above but this time check for collision with the right wall

            if (ball.X > (this.Width - ball.Width))  // ball hits left wall logic
            {
                // TODO
                // --- play score sound
                scoreSound.Play();
                // --- update player 2 score
                player1Score++;

                // TODO use if statement to check to see if player 2 has won the game. If true run GameOver method.
                if (player1Score == 5 && !bonusMode)
                {
                    GameOver("Player 1 Wins!");
                }
                //Else change direction of ball and call SetParameters method.
                else
                {
                    ballMoveRight = !ballMoveRight;
                    SetParameters();
                }

            }
            #endregion

            //refresh the screen, which causes the Form1_Paint method to run
            this.Refresh();
        }

        /// <summary>
        /// Displays a message for the winner when the game is over and allows the user to either select
        /// to play again or end the program
        /// </summary>
        /// <param name="winner">The player name to be shown as the winner</param>
        private void GameOver(string winner)
        {
            newGameOk = true;

            // TODO create game over logic
            startLabel.Show();
            // --- stop the gameUpdateLoop
            gameUpdateLoop.Stop();
            // --- show a message on the startLabel to indicate a winner, (need to Refresh).
            startLabel.Text = winner;
            // --- pause for two seconds 
            Thread.Sleep(2000);
            // --- use the startLabel to ask the user if they want to play again
            startLabel.Text = "Would you like to play again? (1/2/N)";

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // TODO draw paddles using FillRectangle
            e.Graphics.FillRectangle(whiteBrush, p1);
            e.Graphics.FillRectangle(whiteBrush, p2);
            // TODO draw ball using FillRectangle
            e.Graphics.FillRectangle(whiteBrush, ball);
            // TODO draw scores to the screen using DrawString
            e.Graphics.DrawString($"P1: {player1Score}", drawFont, whiteBrush, 10, 10);
            if (!bonusMode)
            {
                e.Graphics.DrawString($"P2: {player2Score}", drawFont, whiteBrush, (this.Width - 100), 10);
            }
        }

    }
}
