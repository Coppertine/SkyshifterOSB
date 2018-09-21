using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using StorybrewCommon.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

namespace StorybrewScripts
{
    public class CreditsManager : StoryboardObjectGenerator
    {
        private FontGenerator Font;
        public override void Generate()
        {
            
            Font = SetFont();
		    GenerateCredit("SAKURABURST", 7687, 15099, 0.5f, 320, 216);
		    GenerateCredit("SKYSHIFTER VIP", 8040, 15099, 25, 320, 265);
        
        
            GenerateCredit("BEATMAP", 16158, 23216, 0.3f, 320, 130);
        
            GenerateCredit("STORYBOARD",17569, 23216, 0.3f, 320, 280);
        
            GenerateCredit("SCUBDOMINO & SHIZUKU-", 18981, 23216, 0.3f, 320, 153);
            GenerateCredit("COPPERTINE - DARKY1 - PONO", 20393, 23216, 0.3f, 320, 303);

            var partLines = File.ReadAllLines(ProjectPath + "/parts.txt");

            foreach(var line in partLines)
            {
                var values = line.Split(';');
                GeneratePartName(values[0], int.Parse(values[1]), int.Parse(values[2]));
            }
        }
        private FontGenerator SetFont()
        {
            var font = LoadFont("sb/credits/f", new FontDescription()
            {
                FontPath = "Roboto-Light.ttf",
                FontSize = 60,
                Color = Color4.White,
                FontStyle = FontStyle.Regular,
            });

            return font;
        }
        private void GeneratePartName(string name, int startTime, int endTime)
        {
            GenerateCredit(name, startTime, endTime, 0.3f, -10, 140);
        }
        private void GenerateCredit(string text, int startTime, int endTime, float scale, int posX, int posY)
        {
            List<OsbSprite> letterList = new List<OsbSprite>();
            var currentNumber = letterPicker(0, text.Length);
            var layer = GetLayer("Credits");
            var letterX = 0f;
            float letterY = posY;
            var lineWidth = 0f;
            var lineHeight = 0f;
        
            foreach(var letter in text)
            {
                var texture = Font.GetTexture(letter.ToString());
                lineWidth += texture.BaseWidth * scale;
                lineHeight = Math.Max(lineHeight, texture.BaseHeight * scale);

                if(!texture.IsEmpty)
                    letterList.Add(layer.CreateSprite(texture.Path));
            }

            letterX = posX - lineWidth/2;

            foreach(var letter in text)
            {
                var texture = Font.GetTexture(letter.ToString());
                var fade = 0f;
                
                if(!texture.IsEmpty)
                {
                    var switchNumber = Random(10, 30);
                    double switchDuration = 50 * switchNumber;
                    var letterStart = startTime - switchDuration;

                    var position = new Vector2(letterX, letterY)
                        + texture.OffsetFor(OsbOrigin.Centre) * scale;

                    var sprite = GetLayer("Credits").CreateSprite(texture.Path, OsbOrigin.Centre, position);
                    
                    for(double i = letterStart; i < startTime + switchDuration; i += 50)
                    {
                        var newNumber = letterPicker(currentNumber, letterList.Count);
                        spawnCharacter(newNumber, i, i + 50, position, scale, fade, letterList);

                        currentNumber = newNumber;
                        fade += 1.0f/(switchNumber*2);
                    }
                    sprite.Fade(startTime + switchDuration, endTime, 1, 1);
                    sprite.Fade(endTime, endTime + 1000, 1, 0);
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

        private void spawnCharacter(int spriteIndex, double startTime, double endTime, Vector2 position, float scale, float Fade, List<OsbSprite> letterList)
        {
            var sprite = GetLayer("").CreateSprite(letterList[spriteIndex].TexturePath, OsbOrigin.Centre, position);
            sprite.Scale(startTime, scale);
            sprite.Fade(startTime, endTime, Fade, Fade);
        }
    }
}
