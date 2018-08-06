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
using System.Drawing;

namespace StorybrewScripts
{
    public class IntroductionHC : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            Rectangle(5569, 14746, 5, 250);
            
            Flash(27452, 1000, 0.5);
            Flash(95216, 5000, 0.5);
            
            Flash(117805, 1000, 0.5);

            NameOnBuilding();
        }

        void Rectangle(int startTime, int endTime, int rectNumber, int maxScale)
        {
            var scale = 0;
            var delay = 0;
            for(int i = 0; i < rectNumber - 1; i++)
            {
                scale += maxScale/rectNumber;
                var sprite = GetLayer("Squares").CreateSprite("sb/pixel.png", OsbOrigin.Centre);
                sprite.Scale(OsbEasing.OutBack, startTime + delay, startTime + 1000, 0, scale);
                sprite.Fade(startTime + delay, startTime + 1000, 0, 0.3);
                sprite.Fade(endTime, endTime + 1000, 0.3, 0);
                sprite.Rotate(OsbEasing.OutBack, startTime + delay, startTime + 1000, 0, -Math.PI/4);
                sprite.Rotate(OsbEasing.InOutSine, endTime + delay, endTime + 1500, Math.PI/4, 0);
                sprite.Color(startTime, Color4.Black);

                delay += 100;
            }
        }
        void Flash(int startTime, int duration, double intensity)
        {
            var sprite = GetLayer("Flash").CreateSprite("sb/pixel.png");
            sprite.ScaleVec(startTime, 854, 480);
            sprite.Fade(startTime, startTime + duration, intensity, 0);
            sprite.Additive(startTime, startTime + duration);
        }

        void NameOnBuilding()
        {
            var font = LoadFont("sb/credits/" + 27451, new FontDescription()
            {
                FontPath = "Roboto-Light.ttf",
                FontSize = 26,
                Color = Color4.White,
                FontStyle = FontStyle.Regular,
            });

            var scale = 0.5f;
            var layer = GetLayer("Credits");
            var letterX = 0f;
            var lineWidth = 0f;
            var lineHeight = 0f;
            var delay = 0;

            foreach(var letter in "SHIZUKU-")
            {
                var texture = font.GetTexture(letter.ToString());
                lineWidth += texture.BaseWidth * scale;
                lineHeight = Math.Max(lineHeight, texture.BaseHeight * scale);
            }

            letterX = 505 - lineWidth * scale;

            foreach(var letter in "SHIZUKU-")
            {
                var texture = font.GetTexture(letter.ToString());
                
                if(!texture.IsEmpty)
                {
                    var position = new Vector2(letterX, 115)
                        + texture.OffsetFor(OsbOrigin.Centre) * scale;
                    var shadowOffset = new Vector2(1, 1);
                    
                    var sprite = GetLayer("Credits").CreateSprite(texture.Path, OsbOrigin.Centre, position);
                    var shadow = GetLayer("CreditsShadow").CreateSprite(texture.Path, OsbOrigin.Centre, position + shadowOffset);
                    sprite.Fade(31687 + delay, 33099 + delay, 0, 1);
                    sprite.Fade(38746 + delay, 40158 + delay, 1, 0);
                    sprite.Scale(27452, 0.5);
                    sprite.MoveY(OsbEasing.InOutSine, 27452, 50040, position.Y, position.Y + 480);
                    sprite.MoveX(OsbEasing.OutSine, 31687 + delay, 31687 + delay + 300, position.X - 50, position.X);

                    shadow.Fade(31687 + delay, 33099 + delay, 0, 1);
                    shadow.Fade(38746 + delay, 40158 + delay, 1, 0);
                    shadow.Scale(27452, 0.5);
                    shadow.MoveY(OsbEasing.InOutSine, 27452, 50040, position.Y + shadowOffset.Y, position.Y + shadowOffset.Y + 480);
                    shadow.Color(27452, Color4.Black);
                    shadow.MoveX(OsbEasing.OutSine, 31687 + delay, 31687 + delay + 300, position.X + 50, position.X - shadowOffset.X);

                    delay += 100;
                }
                letterX += texture.BaseWidth * scale;
            }
        }
    }
}
