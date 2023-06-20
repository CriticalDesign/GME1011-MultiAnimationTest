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
    internal class Explosion
    {
        private Texture2D _explosionSprite;
        private int _rows, _cols;
        private int _currentframe, _totalframes;
        private int _x, _y;
        private int _counter;

        
        public Explosion(Texture2D explosionSprite, int rows, int cols)
        {
            _explosionSprite = explosionSprite;
            _rows = rows;
            _cols = cols;
            _totalframes = _rows * _cols;
            _currentframe = 0;
            _x = 150;
            _y = 50;
            _counter = 0;
        }

        public void Update()
        {
            _counter++;
            if (_counter % 5 == 0)
            {
                _currentframe++;
                if (_currentframe == _totalframes)
                    _currentframe = 0;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int width = _explosionSprite.Width / _cols;
            int height = _explosionSprite.Height / _rows;
            int row = _currentframe / _cols;
            int column = _currentframe % _cols;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle(_x, _y, width, height);

            spriteBatch.Begin();
            spriteBatch.Draw(_explosionSprite, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }


    }
}
