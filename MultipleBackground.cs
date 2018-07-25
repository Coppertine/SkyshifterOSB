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
    public class MultipleBackground : StoryboardObjectGenerator
    {
        private List<OsbSprite> backgroundList = new List<OsbSprite>();

        public override void Generate()
        {
            //Create the 3 background sprites
		    backgroundList.Add(GetLayer("Background").CreateSprite("sb/introbackground01.jpg", OsbOrigin.TopCentre));
		    backgroundList.Add(GetLayer("Background").CreateSprite("sb/introbackground02.jpg", OsbOrigin.TopCentre));
		    backgroundList.Add(GetLayer("Background").CreateSprite("sb/introbackground03.jpg", OsbOrigin.TopCentre));
            
            init();
            Fade(0, 4863, 140393, 140393);
            Move(OsbEasing.InOutSine, 27452, 50040, 0, 480);
        }

        private void init()
        {
            var posY = 0;
            foreach(var background in backgroundList)
            {
                background.Scale(0, 480.0/1080);
                background.MoveY(0, posY);
                posY -= 480;
            }
        }

        private void Fade(int startTime, int startFade, int endTime, int endFade)
        {
            foreach(var background in backgroundList)
            {
                background.Fade(startTime, startFade, 0, 1);
                background.Fade(endFade, endTime, 1, 0);
            }
        }

        private void Move(OsbEasing easing, int startTime, int endTime, int startY, int endY)
        {
            var offsetY = 0;
            foreach(var background in backgroundList)
            {
                background.MoveY(easing, startTime, endTime, startY + offsetY, endY + offsetY); 
                offsetY -= 480;
            }
        }
    }
}