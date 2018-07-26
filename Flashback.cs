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
    public class Flashback : StoryboardObjectGenerator
    {
        public override void Generate()
        {
		    	generateBackground();
					generateVig();
            
        }
				
				void generateBackground()
				{
					var background = GetLayer("Background").CreateSprite("sb/introbackground01.jpg");
					background.Scale(185569,480.0f/1080);
					background.Fade(185393,185569,0, 1);
					background.Fade(190511,0);
				}
				
				void generateVig()
				{
					var vig = GetLayer("Viginette").CreateSprite("sb/vig.png");
					vig.Scale(185569,480.0f/1080);
					vig.Fade(185393,185569,0,1);
					vig.Fade(190511,0);
				
				}
    }
}
