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
        public OsuHitObject lastHitobject = new OsuHitObject();
        public override void Generate()
        {
            MovingParticleGenerate();
            HitObjectParticles(50040, 72628);
        }

        private void MovingParticleGenerate()
        {
            foreach(var hitobject in GetBeatmap("sliders").HitObjects)
            {
                if(hitobject.StartTime == 27452)
                {
                    for(int i = 0; i < 50; i++)
                    {
                        var particleStartPosition = generateRadianPositions(100, 300, hitobject.Position);
                        var particleStart = GetLayer("Particle").CreateSprite("sb/dot.png", OsbOrigin.Centre, particleStartPosition);
                        particleStart.Move(OsbEasing.InExpo, 24628, hitobject.StartTime, particleStartPosition, hitobject.Position);
                        particleStart.Scale(hitobject.StartTime, Random(0.05, 0.1));
                        particleStart.Fade(Random(24628, 26040), 26040, 0, 1);
                        particleStart.Move(OsbEasing.OutExpo, hitobject.StartTime, hitobject.StartTime + 1000, hitobject.Position, particleStartPosition);
                        particleStart.Fade(hitobject.StartTime, hitobject.StartTime + 500, 1, 0);
                    }


                    var sprite = GetLayer("Particle").CreateSprite("sb/flashydot.png", OsbOrigin.Centre, hitobject.Position);
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

                        var particle = GetLayer("Particle").CreateSprite("sb/dot.png", OsbOrigin.Centre, particlePosition);
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

        private void HitObjectParticles(int startTime, int endTime)
        {
            foreach(var hitobject in Beatmap.HitObjects)
            {
                if(hitobject.StartTime >= startTime && hitobject.StartTime <= endTime)
                {
                    for(int i = 0; i < 10; i++)
                    {
                        double radianObject = Math.Atan2(hitobject.Position.Y - lastHitobject.Position.Y, hitobject.Position.X - lastHitobject.Position.X);
 
                        var distance = Math.Sqrt(Math.Pow(hitobject.Position.X - lastHitobject.Position.X, 2) + Math.Pow(hitobject.Position.Y - lastHitobject.Position.Y, 2));
                        if(distance < 20) continue;
                            
                        var radius = distance/Random(2.0, 3.0);
                        var randPI = Random(0, Math.PI/4);
                        var finalAngle = Random(radianObject - randPI, radianObject + randPI);
 
                        var x = radius * Math.Cos(finalAngle) + hitobject.Position.X;
                        var y = radius * Math.Sin(finalAngle) + hitobject.Position.Y;
                        
 
                        var sprite = GetLayer("").CreateSprite("sb/pixel.png");
                        sprite.ScaleVec(OsbEasing.OutExpo, hitobject.StartTime, hitobject.StartTime + 1000, 2, distance/10, 0, distance/10);
                        sprite.Fade(hitobject.StartTime, hitobject.StartTime + 1000, 1, 1);
                        sprite.Move(OsbEasing.OutExpo, hitobject.StartTime, hitobject.StartTime + 1000, hitobject.Position, new Vector2((float)x, (float)y));
                        sprite.Rotate(hitobject.StartTime, finalAngle - Math.PI/2);
                        sprite.Color(hitobject.StartTime, hitobject.StartTime + 1000, Color4.White, Color4.Yellow);  
               
                    }
                lastHitobject = hitobject;
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
