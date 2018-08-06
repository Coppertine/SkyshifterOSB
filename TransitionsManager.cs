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
    }
}
