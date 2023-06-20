using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AnimationTest2
{
    //this is an enum - it will make it easier to 
    //keep track of the Goblin states
    enum RatState
    {
        walking,
        attacking,
        idle
    }


    internal class RatFolk
    {
        //ONE TEXTURE this time - compare with Goblin.
        private Texture2D _ratFolkSpriteSheet;

        //variables for each of the rows in the spritesheet - this way, they can have different
        //configurations with no concerns for the algorithm
        private int _attackRow, _attackCols, _totalAttackFrames;
        private int _idleRow, _idleCols, _totalIdleFrames;
        private int _walkRow, _walkCols, _totalWalkFrames;

        //this will store the current spritesheet details that we're using
        private int _cols, _rows, _currentRow, _totalFrames, _currentFrame;

        //goblin position
        private int _x, _y;

        //hacked timer - to slow the animation down
        private int _counter;
        
        //boolean to track if we're attacking
        private bool _attacking;
        
        //enum to track current state (see above0
        private RatState _myState;

        //we'll use this to flip the sprite when needed
        private SpriteEffects _flipLeft;
        
        //constructor - the nature of this object means we're going to do a lot of work
        //to set it up post construction
        public RatFolk(int x, int y, Texture2D ratFolkSpriteSheet, int rows, int cols)
        {
            _currentFrame = 0;
            _x = x;
            _y = y;
            _counter = 0;
            _ratFolkSpriteSheet = ratFolkSpriteSheet;

            //these are for thw whole sheet
            _rows = rows;
            _cols = cols;
            
            //I'm hard-coding these values, but you COULD bring them in as arguments.
            //This is KEY for the single sprite sheet - what row are the different animations on?
            _attackRow = 2;
            _idleRow = 0;
            _walkRow = 1;

            //for the RatFolk, they are all 8, but they could easily be different numbers of columns
            _attackCols = 8;
            _idleCols = 8;
            _walkCols = 8;

            //since these are all single rows, this is trivial, but if they were multi rows, you would multiply.
            _totalAttackFrames = _attackCols;
            _totalIdleFrames = _idleCols;
            _totalWalkFrames = _walkCols;

        }


        //here's the magic- but it's actually not much
        public void Update()
        {
            _counter++; //update the counter 

            //space bar press and not attacking
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && !_attacking)
            {
                _attacking = true;  //we are now attacking
                changeAnimationState(RatState.attacking);  //change states (see below)
            }

            //if we're not attacking
            if (!_attacking)
            {
                //arrow keys - they all work the same-ish way
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    changeAnimationState(RatState.walking);
                    _x += 5;
                    _flipLeft = SpriteEffects.None;

                }
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    changeAnimationState(RatState.walking);
                    _x -= 5;
                    _flipLeft = SpriteEffects.FlipHorizontally;

                }
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    changeAnimationState(RatState.walking);
                    _y += 5;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    changeAnimationState(RatState.walking);
                    _y -= 5;
                }

                //if no keys are being pressed, we are idle
                if(Keyboard.GetState().GetPressedKeyCount() <= 0) 
                {
                    changeAnimationState(RatState.idle);
                }
            }
            
        }

        //this method simply changes a bunch of variables when we are moving from one 
        //state to another. Notice it has one argument - the enum (from the top)
        public void changeAnimationState(RatState newState)
        {
            //only do the work below IF we are changing states (don't do it if 
            //we try to change to the state we're already on)
            if (_myState != newState)
            {
                _myState = newState;  //set our new state
                _currentFrame = 0;    //we changed states so set the current frame to 0

                //if we are attacking, set these variables
                if (newState == RatState.attacking)
                {
                    _cols = _attackCols;
                    _currentRow = _attackRow;
                    _totalFrames = _totalAttackFrames;
                }
                //if we are walking, set these variables
                else if (newState == RatState.walking)
                {
                    _cols = _walkCols;
                    _currentRow = _walkRow;
                    _totalFrames = _totalWalkFrames;
                }
                //we must be idle
                else
                {
                    _cols = _idleCols;
                    _currentRow = _idleRow;
                    _totalFrames = _totalIdleFrames;
                }
            }
        }

        //ye olde draw method
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_counter % 5 == 0)  //slow down the animation
            {
                _currentFrame++;  //move the frame every 5 steps
                
                //the number of frames is limited. If we are at the end, start
                //back at 0.
                if (_currentFrame >= _totalFrames)
                {
                    _currentFrame = 0;
                    
                    //deal with attacking differently (we run the attack animation only 1x)
                    //If we are at the end of our frames and we were attacking...
                    if (_attacking)
                    {
                        _attacking = false; //no longer attacking
                        changeAnimationState(RatState.idle);  //default to idle
                    }
                }
                    
            }

            //this is the engine of the animation work - it relies on 
            //all the variables that we set in the changeAnimationState method.
            int width = _ratFolkSpriteSheet.Width / _cols;
            int height = _ratFolkSpriteSheet.Height / _rows;
            int row = _currentRow;
            int column = _currentFrame;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle(_x, _y, width, height);

            //special begin so that we can scale the sprites without losing any detail
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);
            //complex draw so we can do cool things
            spriteBatch.Draw(_ratFolkSpriteSheet, new Vector2(_x, _y), sourceRectangle, Color.White, 0, new Vector2(width/2, height/2), new Vector2(4f, 4f), _flipLeft, 0);
            spriteBatch.End();
        }


    }
}
