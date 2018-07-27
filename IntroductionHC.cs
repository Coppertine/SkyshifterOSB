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
    public class IntroductionHC : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            Rectangle(5569, 14746, 5, 250);

            
        }

        void Rectangle(int startTime, int endTime, int rectNumber, int maxScale)
        {
            var scale = 0;
            var delay = 0;
            for(int i = 0; i < rectNumber - 1; i++)
            {
                scale += maxScale/rectNumber;
                var sprite = GetLayer("Squares").CreateSprite("sb/pixel.png", OsbOrigin.Centre);
                sprite.Scale(OsbEasing.OutBack, startTime + delay, startTime + 1000, 0, scale);
                sprite.Fade(startTime + delay, startTime + 1000, 0, 0.3);
                sprite.Fade(endTime, endTime + 1000, 0.3, 0);
                sprite.Rotate(OsbEasing.OutBack, startTime + delay, startTime + 1000, 0, -Math.PI/4);
                sprite.Rotate(OsbEasing.InOutSine, endTime + delay, endTime + 1500, Math.PI/4, 0);
                sprite.Color(startTime, Color4.Black);

                delay += 100;
            }
        }
    }
}
