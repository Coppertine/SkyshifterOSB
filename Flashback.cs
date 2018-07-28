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
		
				public StoryboardLayer layer;
        public override void Generate()
        {
				layer = GetLayer("");
		    	generateBackground();
					generateLids();
					generateVig();
            
        }
				
				void generateBackground()
				{
					var background = layer.CreateSprite("sb/introbackground01.jpg");
					background.Scale(185569,480.0f/1080);
					background.Fade(185393,185569,0, 1);
					background.Fade(190511,0);
					
					var backgroundBlur = layer.CreateSprite("sb/introBgBlur.jpg");
					backgroundBlur.Scale(185569,480.0f/1080);
					backgroundBlur.Fade(186275,186805,0,1);
					backgroundBlur.Fade(186805,187687,1,0.3);
					backgroundBlur.Fade(187687,189099,0.3,0.89);
					backgroundBlur.Fade(OsbEasing.InExpo,189099,190511,0.89,0);
					
					
					
				}
				
				void generateVig()
				{
					var vig = layer.CreateSprite("sb/vig.png");
					vig.Scale(185569,480.0f/1080);
					vig.Fade(185393,185569,0,1);
					vig.Fade(190511,0);
				
				}
				
				void generateLids()
				{
					var lidTop = layer.CreateSprite("sb/pixel.png",OsbOrigin.TopCentre,new Vector2(320,0));
					var lidBottom = layer.CreateSprite("sb/pixel.png",OsbOrigin.BottomCentre,new Vector2(320,480));
					var beat = Beatmap.GetTimingPointAt(1000).BeatDuration;
					
					lidTop.ScaleVec(OsbEasing.OutExpo,185569,186187,new Vector2(1000,240),new Vector2(1000,30));
					lidTop.Color(185393,Color4.Black);
					lidTop.Fade(185393,185569,0,1);
					lidTop.Fade(190511,0);
					lidTop.StartLoopGroup(186187,2);
						lidTop.ScaleVec(OsbEasing.Out,0,beat * 2,new Vector2(1000,30),new Vector2(1000,42));
						lidTop.ScaleVec(OsbEasing.In,beat * 2,beat*4,new Vector2(1000,42),new Vector2(1000,30));
					lidTop.EndGroup();
					
					lidTop.ScaleVec(189099,190511,new Vector2(1000,30),new Vector2(1000,240));
					
					lidBottom.ScaleVec(OsbEasing.OutExpo,185569,186187,new Vector2(1000,240),new Vector2(1000,30));
					lidBottom.Color(185393,Color4.Black);
					lidBottom.Fade(185393,185569,0,1);
					lidBottom.Fade(190511,0);
					lidBottom.StartLoopGroup(186187,2);
						lidBottom.ScaleVec(OsbEasing.Out,0,beat * 2,new Vector2(1000,30),new Vector2(1000,42));
						lidBottom.ScaleVec(OsbEasing.In,beat * 2,beat*4,new Vector2(1000,42),new Vector2(1000,30));
					lidBottom.EndGroup();
					
					
					lidBottom.ScaleVec(189099,190511,new Vector2(1000,30),new Vector2(1000,240));
				}
    }
}
