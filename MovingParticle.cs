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
    public class MovingParticle : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            

            foreach(var hitobject in GetBeatmap("sliders").HitObjects)
            {
                if(hitobject.StartTime == 27452)
                {
                    var sprite = GetLayer("ParticleMoving").CreateSprite("sb/flashydot.png", OsbOrigin.Centre, hitobject.Position);
                    sprite.Scale(hitobject.StartTime, 0.08);
                    var timestep = Beatmap.GetTimingPointAt((int)hitobject.StartTime).BeatDuration / 4;
                    var startTime = hitobject.StartTime;
                    while (true)
                    {
                        var endTime = startTime + timestep;

                        var complete = hitobject.EndTime - endTime < 5;
                        if (complete) endTime = hitobject.EndTime;

                        var startPosition = sprite.PositionAt(startTime);
                        sprite.Move(startTime, endTime, startPosition, hitobject.PositionAtTime(endTime));

                        if (complete) break;
                            startTime += timestep;
                    }
                }
            }
        }
    }
}
