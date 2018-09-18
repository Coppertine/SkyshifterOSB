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
            Fade(0, 4863, 27452, 27452, 0, 0.5f);
            Fade(27453, 27453, 50040, 50040, 0, 1f);
            Move(OsbEasing.InOutSine, 27452, 50040, 0, 480);

            Move(OsbEasing.OutExpo, 72628, 76863, 400, 480);
            Move(OsbEasing.InOutSine, 95216, 117099, 480, 960);
            
            //Move(OsbEasing.InExpo,117099,117805,960,400);
            
            Fade(50040, 50040, 72628, 72628, 0, 0); 
            Fade(72628, 74040, 117805, 117805, 0, 1f);

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

        private void Fade(int startTime, int startFading, int endTime, int endFading, float startFade, float endFade)
        {
            foreach(var background in backgroundList)
            {
                background.Fade(startTime, startFading, startFade, endFade);
                background.Fade(endFading, endTime, endFade, startFade);
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