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
    public class CreditsManager : StoryboardObjectGenerator
    {
        public List<OsbSprite> letterList = new List<OsbSprite>();
        public override void Generate()
        {
		    GenerateCredit("SAKURABURST", 7687, 16158, 100, 320, 220);
            
        }

        private void GenerateCredit(string text, int startTime, int endTime, int size, int posX, int posY)
        {
            var font = LoadFont("sb/credits/" + startTime, new FontDescription()
            {
                FontPath = "Roboto-Light.ttf",
                FontSize = size,
                Color = Color4.White,
                FontStyle = FontStyle.Regular,
            });

            var currentNumber = letterPicker(0, text.Length);
            var layer = GetLayer("Credits");

            var letterX = 0f;
            float letterY = posY;

            var lineWidth = 0f;
            var lineHeight = 0f;

            var scale = 0.5f;

            foreach(var letter in text)
            {
                var texture = font.GetTexture(letter.ToString());
                lineWidth += texture.BaseWidth * scale;
                lineHeight = Math.Max(lineHeight, texture.BaseHeight * scale);
                letterList.Add(layer.CreateSprite(texture.Path));
            }

            letterX = posX - lineWidth * scale;

            foreach(var letter in text)
            {
                var texture = font.GetTexture(letter.ToString());
                
                if(!texture.IsEmpty)
                {
                    var switchNumber = Random(10, 50);
                    var switchDuration = 50 * switchNumber;
                    var letterStart = startTime - switchDuration;

                    var position = new Vector2(letterX, letterY)
                        + texture.OffsetFor(OsbOrigin.Centre) * scale;

                    var sprite = GetLayer("Credits").CreateSprite(texture.Path, OsbOrigin.Centre, position);
                    
                    for(int i = letterStart; i < startTime + switchDuration; i += 50)
                    {
                        var newNumber = letterPicker(currentNumber, text.Length);
                        spawnCharacter(newNumber, i, i + 50, position, scale);
                        currentNumber = newNumber;
                    }

                    sprite.Fade(startTime + switchDuration, endTime, 1, 1);
                    sprite.Scale(startTime + switchDuration, scale);
                }
                letterX += texture.BaseWidth * scale;
            }
        }

        private int letterPicker(int currentNumber, int maxNumber)
        {
            int n = Random(0, maxNumber);
            if(n == currentNumber)
            {
                return letterPicker(currentNumber, maxNumber);
            }
            else 
            {
                return n;
            }
        }

        private void spawnCharacter(int spriteIndex, int startTime, int endTime, Vector2 position, float scale)
        {
            var sprite = GetLayer("").CreateSprite(letterList[spriteIndex].TexturePath, OsbOrigin.Centre, position);
            sprite.Scale(startTime, scale);
            sprite.Fade(startTime, endTime, 1, 1);
        }
    }
}
