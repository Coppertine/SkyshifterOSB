using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class RectangleManger : StoryboardObjectGenerator
    {
        public override void Generate()
        {
		    GenerateRectangle(1000, 10000, new Vector2(320, 240), 200, 100, 10, 10);
            
        }

        private void GenerateRectangle(int startTime, int endTime, Vector2 position, int scaleX, int scaleY, int xElements, int yElements)
        {
            var startPosX = position.X - scaleX/2;
            var startPosY = position.Y - scaleY/2;

            var elementScaleX = scaleX/xElements;
            var elementScaleY = scaleY/yElements;

            var delay = 0d;

            for(float x = startPosX; x < (startPosX + scaleX) - elementScaleX; x += elementScaleX)
            {
                for(float y = startPosY; y < (startPosY + scaleY) - elementScaleY; y += elementScaleY)
                {
                    var rectNumberX = (x - startPosX) / elementScaleX;
                    var rectNumberY = (y - startPosY) / elementScaleY;
                    var sprite = GetLayer("Rectangle").CreateSprite("sb/pixel.png", OsbOrigin.TopLeft, new Vector2(x, y));

                    sprite.Fade(startTime + delay, startTime + delay + 300, 0, 0.5);
                    sprite.Fade(endTime, rectNumberY / yElements, 0, 0.5);
                    sprite.Fade(endTime, endTime, 0.5, 1);
                    sprite.ScaleVec(startTime, elementScaleX, elementScaleY);

                    //Log("y = " + (rectNumberY / yElements).ToString());
                    Log("x = " + (rectNumberX / xElements).ToString());

                    delay = (rectNumberX * rectNumberX + rectNumberY * rectNumberY) * 100;
                }
            }
        }
    }
}