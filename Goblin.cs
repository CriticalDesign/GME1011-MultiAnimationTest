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
    enum GoblinState
    {
        walking,
        attacking,
        idle
    }


    internal class Goblin
    {
        //four textures - three for different potential movements, and one to store the 
        //current movement.
        private Texture2D _goblinAttackingSprite, _goblinIdleSprite, _goblinWalkingSprite, _goblinCurrentSprite;

        //variables for each of the spritesheets - this way, they can have different
        //configurations with no concerns for the algorithm
        private int _attackRows, _attackCols, _totalAttackFrames;
        private int _idleRows, _idleCols, _totalIdleFrames;
        private int _walkRows, _walkCols, _totalWalkFrames;

        //this will store the current spritesheet details that we're using
        private int _cols, _rows, _totalFrames, _currentFrame;

        //goblin position
        private int _x, _y;

        //hacked timer - to slow the animation down
        private int _counter;
        
        //boolean to track if we're attacking
        private bool _attacking;
        
        //enum to track current state (see above0
        private GoblinState _myState;

        //we'll use this to flip the sprite when needed
        private SpriteEffects _flipLeft;
        
        //constructor - the nature of this object means we're going to do a lot of work
        //to set it up post construction
        public Goblin(int x, int y)
        {
            _currentFrame = 0;
            _x = x;
            _y = y;
            _counter = 0;
        }

        //load the attack sprite sheet and set the appropriate variables
        public void loadAttackSprite(Texture2D goblinAttackSprite, int rows, int cols)
        {
            _totalAttackFrames = _attackRows * _attackCols;
            _goblinAttackingSprite = goblinAttackSprite;
            _attackRows = rows;
            _attackCols = cols;
            _totalAttackFrames = _attackRows * _attackCols;
        }

        //load the idle sprite sheet and set the appropriate variables
        public void loadIdleSprite(Texture2D goblinIdleSprite, int rows, int cols)
        {
            _totalIdleFrames = _idleRows * _idleCols;
            _goblinIdleSprite = goblinIdleSprite;
            _idleRows = rows;
            _idleCols = cols;
            _totalIdleFrames = _idleRows * _idleCols;
            changeAnimationState(GoblinState.idle);
        }

        //load the walk sprite sheet and set the appropriate variables
        public void loadWalkSprite(Texture2D goblinWalkSprite, int rows, int cols)
        {
            _totalWalkFrames = _walkRows * _walkCols;
            _goblinWalkingSprite = goblinWalkSprite;
            _walkRows = rows;
            _walkCols = cols;
            _totalWalkFrames = _walkRows * _walkCols;
        }

        //here's the magic- but it's actually not much
        public void Update()
        {
            _counter++; //update the counter 

            //space bar press and not attacking
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !_attacking)
            {
                _attacking = true;  //we are now attacking
                changeAnimationState(GoblinState.attacking);  //change states (see below)
            }

            //if we're not attacking
            if (!_attacking)
            {
                //arrow keys - they all work the same-ish way
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    changeAnimationState(GoblinState.walking);
                    _x += 5;
                    _flipLeft = SpriteEffects.None;

                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    changeAnimationState(GoblinState.walking);
                    _x -= 5;
                    _flipLeft = SpriteEffects.FlipHorizontally;

                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    changeAnimationState(GoblinState.walking);
                    _y += 5;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    changeAnimationState(GoblinState.walking);
                    _y -= 5;
                }

                //if no keys are being pressed, we are idle
                if(Keyboard.GetState().GetPressedKeyCount() <= 0) 
                {
                    changeAnimationState(GoblinState.idle);
                }
            }
            
        }

        //this method simply changes a bunch of variables when we are moving from one 
        //state to another. Notice it has one argument - the enum (from the top)
        public void changeAnimationState(GoblinState newState)
        {
            //only do the work below IF we are changing states (don't do it if 
            //we try to change to the state we're already on)
            if (_myState != newState)
            {
                _myState = newState;  //set our new state
                _currentFrame = 0;    //we changed states so set the current frame to 0

                //if we are attacking, set these variables
                if (newState == GoblinState.attacking)
                {
                    _goblinCurrentSprite = _goblinAttackingSprite;
                    _cols = _attackCols;
                    _rows = _attackRows;
                    _totalFrames = _totalAttackFrames;
                }
                //if we are walking, set these variables
                else if (newState == GoblinState.walking)
                {
                    _goblinCurrentSprite = _goblinWalkingSprite;
                    _cols = _walkCols;
                    _rows = _walkRows;
                    _totalFrames = _totalWalkFrames;
                }
                //we must be idle
                else
                {
                    _goblinCurrentSprite = _goblinIdleSprite;
                    _cols = _idleCols;
                    _rows = _idleRows;
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
                        changeAnimationState(GoblinState.idle);  //default to idle
                    }
                }
                    
            }

            //this is the engine of the animation work - it relies on 
            //all the variables that we set in the changeAnimationState method.
            int width = _goblinCurrentSprite.Width / _cols;
            int height = _goblinCurrentSprite.Height / _rows;
            int row = _currentFrame / _cols;
            int column = _currentFrame % _cols;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle(_x, _y, width, height);

            //special begin so that we can scale the sprites without losing any detail
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);
            //complex draw so we can do cool things
            spriteBatch.Draw(_goblinCurrentSprite, new Vector2(_x, _y), sourceRectangle, Color.White, 0, new Vector2(width/2, height/2), new Vector2(4f, 4f), _flipLeft, 0);
            spriteBatch.End();
        }


    }
}
