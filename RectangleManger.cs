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

            var delayMultiplier = 80f;
            var maxDelay = Math.Abs(xElements + yElements) * delayMultiplier; //used to invert our function

            for (int x = 0; x < xElements; x++) {
                for (int y = 0; y < yElements; y++) {
                    var sTime = maxDelay - Math.Abs(x - xElements / 2f + y - yElements / 2f) * delayMultiplier; //get the startTime of the indiviual rect
                    
                    var sprite = GetLayer("Rectangle").CreateSprite("sb/pixel.png", OsbOrigin.TopLeft, new Vector2(x * elementScaleX, y * elementScaleY) + position);
                    sprite.Fade(sTime, sTime + 300, 0, 0.5f);
                    sprite.Fade(endTime, 1);
                    sprite.ScaleVec(sTime, elementScaleX, elementScaleY);
                }
            }
        }
    }
}
