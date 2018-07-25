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
    public class RemoveBackground : StoryboardObjectGenerator
    {
        public override void Generate()
        {
		    var sprite = GetLayer("").CreateSprite("50283696_p0.jpg");
            sprite.Fade(0, 0); 
        }
    }
}
