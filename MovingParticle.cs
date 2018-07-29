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
                    for(int i = 0; i < 50; i++)
                    {
                        var particleStartPosition = generateRadianPositions(100, 300, hitobject.Position);
                        var particleStart = GetLayer("ParticleMoving").CreateSprite("sb/dot.png", OsbOrigin.Centre, particleStartPosition);
                        particleStart.Move(OsbEasing.InExpo, 24628, hitobject.StartTime, particleStartPosition, hitobject.Position);
                        particleStart.Scale(hitobject.StartTime, Random(0.05, 0.1));
                        particleStart.Fade(Random(24628, 26040), 26040, 0, 1);
                        particleStart.Move(OsbEasing.OutExpo, hitobject.StartTime, hitobject.StartTime + 1000, hitobject.Position, particleStartPosition);
                        particleStart.Fade(hitobject.StartTime, hitobject.StartTime + 500, 1, 0);
                    }


                    var sprite = GetLayer("ParticleMoving").CreateSprite("sb/flashydot.png", OsbOrigin.Centre, hitobject.Position);
                    sprite.Scale(hitobject.StartTime, 30275, 0, 0.08);
                    sprite.Scale(48628, 50040, 0.08, 0);
                    var timestep = Beatmap.GetTimingPointAt((int)hitobject.StartTime).BeatDuration / 4;
                    var startTime = hitobject.StartTime;
                    while (true)
                    {
                        var endTime = startTime + timestep;
                        var complete = hitobject.EndTime - endTime < 5;
                        if (complete) endTime = hitobject.EndTime;

                        var startPosition = sprite.PositionAt(startTime);
                        var particlePosition = generateRadianPositions(0, 10, startPosition);

                        var particle = GetLayer("ParticleMoving").CreateSprite("sb/dot.png", OsbOrigin.Centre, particlePosition);
                        particle.Fade(startTime, startTime + Random(1000, 2000), 1, 0);
                        particle.Scale(startTime, Random(0.05, 0.2));
                        particle.Color(startTime, Color4.White);
                        sprite.Move(startTime, endTime, startPosition, hitobject.PositionAtTime(endTime));

                        if (complete) break;
                            startTime += timestep;
                    }
                }
            }
        }
        private Vector2 generateRadianPositions(float minRadius, float maxRadius, Vector2 centre)
        {
            var radius = Random(minRadius, maxRadius);
            var radian = Random(0, Math.PI*2);

            return  new Vector2
            (
                (int)(radius * Math.Cos(radian) + centre.X),
                (int)(radius * Math.Sin(radian) + centre.Y)
            );
        }
    } 
}
