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
    public class Kiai : StoryboardObjectGenerator
    {
        StoryboardLayer layer;
        double beatduration;
        double offset;
        FontGenerator font;

        public override void Generate() {
            //init
            layer = GetLayer("");
            beatduration = Beatmap.GetTimingPointAt(0).BeatDuration;
            offset = Beatmap.GetTimingPointAt(0).Offset;

            //Kiai1
            GenerateBackground(140393, 185569);

            GenerateParticles(140393, 162981);
            GenerateParticles(162981, 185569, true);
            GenerateStrongHits(141452, 184158);

            GenerateStartTransition(117805);
            GenerateMidTransition(161569);
            GenerateEndTransition(184158, extend:true);

            //Kiai2
            GenerateBackground(281569, 326746);

            GenerateCar(284393);
            GenerateParticles(281569, 304158);
            GenerateParticles(304158, 326746, true);
            GenerateCursorFollow(281569, 302746);
            GenerateCursorFollow(304158, 323746);
            GenerateStrongHits(282628, 325334);

            GenerateStartTransition(258981);
            GenerateMidTransition(302746);
            GenerateEndTransition(325334);

            var vignette = layer.CreateSprite("sb/vig.png"); //TODO remove this sprite in favor of a global vignette manager 
            vignette.Scale(117805, 480.0f/1080);
            vignette.Fade(117805, .9f);
            vignette.Fade(184158 + 1411, 0f);

            vignette.Fade(281569, .9f);
            vignette.Fade(325334 + 1411, 0f);
        }

        public void GenerateBackground(double startTime, double endTime) { 
            var bgBlur = layer.CreateSprite("sb/bgKiaiBlur.jpg");
            bgBlur.Scale(startTime, 480.0f / 1093 + 0.07f);
            bgBlur.Fade(startTime, 1);
            bgBlur.Fade(endTime, 0);

            var bgOverlay = layer.CreateSprite("sb/bgKiai.jpg");
            bgOverlay.Fade(startTime, 0.2f);
            bgOverlay.Scale(startTime, (480.0f / 1093) + 0.07f);
            bgOverlay.Fade(endTime - beatduration * 4, 0f);

            var movement = 3f;
            var rotate = .01f;
            for (var time = startTime; time < endTime - beatduration * 5; time += beatduration) {
                
                if (time > 313864 && time < 314922) //This is a silent part in the second kiai. No movement wanted
                    continue;

                var position = new Vector2(320, 240) + new Vector2((float)Random(-movement, movement), (float)Random(-movement, movement)) * (IsStrongHit(time) ? 10f : 1f);
                var rotation = (float)Random(-rotate, rotate) * (IsStrongHit(time) ? 5f : 1f);

                bgOverlay.Move(OsbEasing.OutExpo, time, time + beatduration, bgOverlay.PositionAt(time), position);
                bgBlur.Move(OsbEasing.InCirc, time, time + beatduration, bgOverlay.PositionAt(time), position);
                
                bgOverlay.Rotate(OsbEasing.OutExpo, time, time + beatduration, bgOverlay.RotationAt(time), rotation);
                bgBlur.Rotate(OsbEasing.InCirc, time, time + beatduration, bgOverlay.RotationAt(time), rotation);
            }
            bgOverlay.Additive(bgOverlay.CommandsStartTime, bgOverlay.CommandsEndTime);
        }

        #region kiai effects

        public void GenerateCursorFollow(double startTime, double endTime) {
            var sprite = layer.CreateSprite("sb/spotLight.png");
            sprite.ScaleVec(startTime, 1f, 4f);

            OsuHitObject prev = null;
            foreach (var hObj in Beatmap.HitObjects) {
                
                if(hObj.StartTime < startTime || hObj.StartTime > endTime)
                    continue;

                if(prev != null) {
                    sprite.Move(OsbEasing.InSine, prev.EndTime, hObj.StartTime, prev.EndPosition, hObj.Position);
                    
                    if(hObj is OsuSlider)
                        sprite.Move(hObj.StartTime, hObj.EndTime, hObj.Position, hObj.EndPosition);

                    if(prev.ColorIndex != hObj.ColorIndex)
                        sprite.Color(hObj.StartTime, hObj.Color);
                }
                    
                prev = hObj;
            }

            for (var time = startTime; time < endTime; time += beatduration) {

                if (time > 313864 && time < 314922) //This is a silent part in the second kiai. No movement wanted
                    continue;

                var rotation = Random(0.3f, 0.8f) * Random(0, 2) == 0 ? 1 : -1;
                
                sprite.Fade(OsbEasing.InCubic, time, time + beatduration, .8f, 0f);
                sprite.Rotate(OsbEasing.OutElasticQuarter, time, time + beatduration, sprite.RotationAt(time), sprite.RotationAt(time) + rotation);
            }

            sprite.Additive(sprite.CommandsStartTime, sprite.CommandsEndTime);
        }

        public void GenerateCar(double time) {
            var sprite = layer.CreateSprite("sb/mostImportantSpriteEUW.png", OsbOrigin.Centre, new Vector2(720, 500));

            var currentTime = time;
            while (sprite.PositionAt(currentTime).Y > -20) {
                var longHit = IsStrongHit(currentTime);

                var yPos = sprite.PositionAt(currentTime).Y;
                var yRot = sprite.RotationAt(currentTime);

                sprite.MoveY(longHit ? OsbEasing.OutCubic : OsbEasing.OutExpo, currentTime, currentTime + beatduration, yPos, yPos - 40);
                sprite.Rotate(longHit ? OsbEasing.Out : OsbEasing.OutExpo, currentTime, currentTime + beatduration, yRot, yRot + Math.PI * 0.25f);

                currentTime += beatduration;
            }
        }

        public void GenerateParticles(double startTime, double endTime, bool fromTop = false) { 
            using (var pool = new OsbSpritePool(layer, "sb/pixel.png", OsbOrigin.Centre, (sprite, sTime, eTime) => {
                sprite.Fade(sTime, Random(0.2f, 0.9f));
                sprite.Scale(sTime, Random(20f, 80f));
                sprite.MoveX(sTime, Random(-107, 757));

                if(eTime > endTime - beatduration * 4) //Hide sprites if they cross the end time
                    sprite.Fade(endTime, 0f);

            })){
                for (var sTime = (double)startTime; sTime <= endTime - beatduration * 4; sTime += beatduration / 2f) {
                    var baseSpeed = Random(40, 120);
                    var eTime = sTime + Math.Ceiling(620f / baseSpeed) * beatduration;

                    if(eTime > 313864 && sTime < 314922) //Add bonus time for the movement stop section in the second kiai
                        eTime += 1058 * 2;

                    var sprite = pool.Get(sTime, eTime);

                    var moveSpeed = baseSpeed * (fromTop ? 1 : -1);
                    var currentTime = sTime + (sTime - offset) % beatduration;

                    sprite.MoveY(sTime, fromTop ? -60 : 540);
                    while(fromTop ? sprite.PositionAt(currentTime).Y < 540 : sprite.PositionAt(currentTime).Y > -60) {

                        if (currentTime > 313864 && currentTime < 314922) { //This is a silent part in the second kiai. No movement wanted
                            currentTime += beatduration;
                            continue;
                        }

                        if (fromTop && currentTime > endTime - beatduration * 4) //Kiai fading out, stop movement at once (sprite needs to stay for a bit longer)
                            break;

                        var longHit = IsStrongHit(currentTime);

                        var yPos = sprite.PositionAt(currentTime).Y;
                        var yRot = sprite.RotationAt(currentTime);

                        sprite.MoveY(longHit ? OsbEasing.OutCubic : OsbEasing.OutExpo, currentTime, currentTime + beatduration, yPos, yPos + moveSpeed * (longHit ? 1.1 : 1));
                        sprite.Rotate(longHit ? OsbEasing.Out : OsbEasing.OutExpo, currentTime, currentTime + beatduration, yRot, yRot + Math.PI * 0.25f);

                        currentTime += beatduration;
                    }

                    if(Random(0, 4) > 0) //HACK move the time back in order to increase the particle count without running into syncing issues
                        sTime -= beatduration / 2f; 
                }
            }
        }

        public void GenerateStrongHits(double startTime, double endTime)
        {
            //flash
            var sprite = layer.CreateSprite("sb/pixel.png");
            sprite.ScaleVec(startTime, 864, 480);
            sprite.StartLoopGroup(startTime, 1 + (int)((endTime - startTime) / (beatduration * 4f)));
            {
                sprite.Fade(OsbEasing.In, 0, beatduration * 4, .2f, 0f);
            }
            sprite.EndGroup();
            sprite.Additive(sprite.CommandsStartTime, sprite.CommandsEndTime); 

            //slider highlight
            foreach (var hitobject in Beatmap.HitObjects)
            {
                if (hitobject.StartTime < startTime - 5 || endTime - 5 <= hitobject.StartTime)
                    continue;

                if (!IsStrongHit(hitobject.StartTime))
                    continue;

                var fadeTime = beatduration / 4f * 3 + 200;

                var hSprite = layer.CreateSprite("sb/highlight.png", OsbOrigin.Centre, hitobject.Position);
                hSprite.Scale(hitobject.StartTime, 4);
                hSprite.Fade(OsbEasing.In, hitobject.StartTime, hitobject.StartTime + fadeTime, .5f, 0);
                hSprite.Additive(hitobject.StartTime, hitobject.StartTime + fadeTime);
                hSprite.Color(hitobject.StartTime, hitobject.Color); //TODO tell the mapper to colorhax it

                if (hitobject is OsuSlider) {
                    var timestep = beatduration / 4f;
                    var currentTime = hitobject.StartTime;
                    while (true)
                    {
                        var nextTime = currentTime + timestep;

                        var complete = hitobject.EndTime - nextTime < 5;
                        if (complete) nextTime = hitobject.EndTime;

                        var startPosition = hSprite.PositionAt(currentTime);
                        hSprite.Move(currentTime, nextTime, startPosition, hitobject.PositionAtTime(nextTime));

                        if (complete) break;
                        currentTime += timestep;
                    }
                }
            }
        }

        #endregion

        #region transitions

        public void GenerateStartTransition(double time) {
            //background
            var bgBlur = layer.CreateSprite("sb/bgKiaiBlur.jpg"); //TODO change path depending on the previous section
            bgBlur.Scale(time, 480.0f / 1093);
            bgBlur.Fade(time + 22058, 1);

            //edges
            var off = 84; //move the outer boundaries back to prevent caps caused by the rotation
            var positions = new Vector2[] { new Vector2(-107 - off, 0 - off), new Vector2(757 + off, 0 - off), new Vector2(757 + off, 480 + off), new Vector2(-107 - off, 480 + off) };
            for (int i = 0; i < 4; i++) {
                var width = i % 2 == 0 ? 600 : 600;
                var height = off + (i % 2 == 0 ? 140 : 864 / 2f);

                var sprite = layer.CreateSprite("sb/pixel.png", OsbOrigin.TopCentre);
                sprite.Move(OsbEasing.InOutSine, time, time + 22058, positions[i], Vector2.Lerp(positions[i], positions[i == 3 ? 0 : (i + 1)], 0.5f));
                sprite.Rotate(time, time + 22058, Math.PI / 2f * i - Math.PI / 4f, Math.PI / 2f * i);
                sprite.ScaleVec(OsbEasing.InCirc, time, time + 22058, width, off * 1.4f, width, height);
            }

            //transition square 140393 - 117805
            var square = layer.CreateSprite("sb/pixel.png", OsbOrigin.Centre);
            square.ScaleVec(OsbEasing.OutElasticQuarter, time + 22058, time + 22058 + 265, 864, 864, 200, 200);
            square.ScaleVec(OsbEasing.InExpo, time + 22058 + 265, time + 22588, 200, 200, 1000, 1000);
            square.Rotate(OsbEasing.OutElasticHalf, time + 22058, time + 22588, 0, Math.PI / 4f);

            //flash
            var flash = layer.CreateSprite("sb/pixel.png", OsbOrigin.Centre);
            flash.ScaleVec(time + 22588, 864, 480);
            flash.Fade(OsbEasing.In, time + 22588, time + 22588 + 353, 1f, 0f);
        }

        public void GenerateMidTransition(double time) {
            //cover
            var segments = 4;
            for (int i = 0; i < segments; i++) {
                var sprite = layer.CreateSprite("sb/pixel.png", i % 2 == 0 ? OsbOrigin.CentreLeft : OsbOrigin.CentreRight, new Vector2(i % 2 == 0 ? -107 : 757, 480f / segments * (i + 0.5f)));
                sprite.Color(time, Color4.Gray);
                sprite.ScaleVec(OsbEasing.InQuad, time, time + 1059, 0, 480f / segments, 864, 480f / segments);
            }

            //flash
            var flash = layer.CreateSprite("sb/pixel.png");
            flash.ScaleVec(time + 1059, 864, 480);
            flash.Fade(OsbEasing.In, time + 1412, time + 2118, 1f, 0f);
        }

        public void GenerateEndTransition(double time, bool extend=false) {
            var text = layer.CreateSprite("sb/lyrics/_003.png"); //TODO don't hardcode this path
            text.Fade(OsbEasing.OutExpo, time, time + 400, 0f, 1f);
            text.Scale(time + 1411, .5f);

            for (var i = 0; i < 2; i++) {
                var sprite = layer.CreateSprite("sb/pixel.png", i == 0 ? OsbOrigin.TopCentre : OsbOrigin.BottomCentre, new Vector2(320, i == 0 ? 0 : 480));
                sprite.Color(time, Color4.Black);
                sprite.ScaleVec(OsbEasing.OutExpo, time, time + 176, 864, 0, 864, 220);
                sprite.ScaleVec(OsbEasing.InExpo, time + 176, time + 1412, 864, 220, 864, 240);

                if(!extend)
                    continue;
                
                sprite.ScaleVec(OsbEasing.OutElasticQuarter, time + beatduration * 4, time + beatduration * 12, 864, 240, 864, 40);
                sprite.ScaleVec(OsbEasing.InExpo, time + beatduration * 12, time + beatduration * 18, 864, 40, 864, 240);
            }
        }

        #endregion

        public bool IsStrongHit(double time) { 
            var dist = (time - offset - beatduration * 3) % (beatduration * 4);
            return dist < 5 || dist > beatduration * 4 - 5;
        }
    }
}
