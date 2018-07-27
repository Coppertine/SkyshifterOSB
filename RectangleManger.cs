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
		    GenerateSplittedRectangle(4863, 14746, new Vector2(320, 240), 300, 50, 30, 10, Color4.Black, 0.75, 20);
            
        }
        private void GenerateSplittedRectangle(int startTime, int endTime, Vector2 position, int scaleX, int scaleY, int xElements, int yElements, Color4 color, double fade, float speed)
        {

            var startPosX = position.X - scaleX/2;
            var startPosY = position.Y - scaleY/2;

            var elementScaleX = scaleX/xElements;
            var elementScaleY = scaleY/yElements;

            var delayMultiplier = speed;
            var maxDelay = Math.Abs(xElements /2f + yElements /2f) * delayMultiplier; //used to invert our function
            var offset = new Vector2(scaleX /2f, scaleY /2f);

            for (int x = 0; x < xElements; x++) {
                for (int y = 0; y < yElements; y++) {
                    var sTime = startTime + maxDelay - Math.Abs(x - xElements / 2f + y - yElements / 2f) * delayMultiplier; //get the startTime of the indiviual rect
                    
                    var sprite = GetLayer("Rectangle").CreateSprite("sb/pixel.png", OsbOrigin.TopLeft, new Vector2(x * elementScaleX, y * elementScaleY) + position - offset);
                    sprite.Fade(sTime, sTime + 300, 0, fade);
                    sprite.Fade(endTime, endTime + 1000, fade, 0);
                    sprite.ScaleVec(sTime, elementScaleX, elementScaleY);
                    sprite.Color(startTime, color);
                }
            }
        }
    }
}
