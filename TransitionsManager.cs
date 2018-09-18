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
    public class TransitionsManager : StoryboardObjectGenerator
    {
        public override void Generate()
        {
		      TransitionStore(48628, 50040, 8);
          
          
          GenerateFlare(212393, 213805);
          GenerateDiagSplit(213805, 214158);
        }

        private void TransitionStore(int startTime, int endTime, int storeNumber)
        {
            var beat = Beatmap.GetTimingPointAt(startTime).BeatDuration;
            var duration = endTime - startTime;
            var storeWidth = 854/storeNumber;
            var posX = -107 + storeWidth/2;
            var delay = 0d;

            for(int i = 0; i < storeNumber; i++)
            {
                var sprite = GetLayer("Transitions").CreateSprite("sb/pixel.png", OsbOrigin.Centre, new Vector2(posX, 240));
                sprite.ScaleVec(OsbEasing.OutExpo, startTime + delay, endTime, 0, 480, storeWidth, 480);
                sprite.Fade(endTime, endTime + 300, 1, 0);
                sprite.Color(startTime, Color4.Black);

                delay += beat/2;
                posX += storeWidth;
            }
        }
        
        void GenerateFlare(double startTime, double endTime)
        {
            var flare = GetLayer("Transistions").CreateSprite("sb/highlight.png");
            flare.Scale(OsbEasing.Out, startTime, endTime, 0.05, 9);
            flare.Fade(OsbEasing.Out, startTime, endTime - 200, 0, 1);
            flare.Fade(endTime, 0);
        }
        
        void GenerateDiagSplit(double startTime, double endTime)
        {
            //Side1 = Top to bottom right
            //Side2 = Bottom Right to Top
            var side1 = GetLayer("Transitions").CreateSprite("sb/pixel.png", OsbOrigin.BottomRight, new Vector2(747, 490));
            var side2 = GetLayer("Transitions").CreateSprite("sb/pixel.png", OsbOrigin.BottomLeft, new Vector2(-400, 410));
            
            side1.ScaleVec(OsbEasing.Out,startTime, endTime, new Vector2(1200, 500), new Vector2(0, 500));
            side2.ScaleVec(OsbEasing.Out,startTime, endTime, new Vector2(1200, -500), new Vector2(0, -500));
            
            side1.Rotate(startTime, Math.Atan2(9, 16));
            side2.Rotate(startTime, Math.Atan2(9, 16));
            
        }
        
        public double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
        
        
    }
}
