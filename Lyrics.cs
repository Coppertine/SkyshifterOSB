using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System;
using System.Drawing;
using System.IO;

namespace StorybrewScripts
{
    public class Lyrics : StoryboardObjectGenerator
    {
        public string SubtitlesPath = "lyrics.ass";
        public string FontName = "Raleway-Light.ttf";
        public string SpritesPath = "sb/lyrics";
        public int FontSize = 40;
        public float FontScale = 0.5f;
        public Color4 FontColor = Color4.White;
        public FontStyle FontStyle = FontStyle.Regular;
        public int GlowRadius = 0;
        public Color4 GlowColor = new Color4(255, 255, 255, 100);
        public bool AdditiveGlow = true;
        public int OutlineThickness = 0;
        public Color4 OutlineColor = new Color4(50, 50, 50, 200);
        public int ShadowThickness = 2;
        public Color4 ShadowColor = new Color4(0, 0, 0, 255);
        public Vector2 Padding = Vector2.Zero;
        public float SubtitleY = 400;
        public bool TrimTransparency = true;
        public OsbOrigin Origin = OsbOrigin.Centre;

        public override void Generate()
        {
            var font = LoadFont(SpritesPath, new FontDescription()
            {
                FontPath = FontName,
                FontSize = FontSize,
                Color = FontColor,
                Padding = Padding,
                FontStyle = FontStyle,
                TrimTransparency = TrimTransparency,
            },
            new FontGlow()
            {
                Radius = AdditiveGlow ? 0 : GlowRadius,
                Color = GlowColor,
            },
            new FontOutline()
            {
                Thickness = OutlineThickness,
                Color = OutlineColor,
            },
            new FontShadow()
            {
                Thickness = ShadowThickness,
                Color = ShadowColor,
            });

            var layer = GetLayer("");
            foreach (var line in LoadSubtitles(SubtitlesPath).Lines)
            {
                var texture = font.GetTexture(line.Text);
                var position = new Vector2(320 - texture.BaseWidth * FontScale * 0.5f, SubtitleY)
                    + texture.OffsetFor(Origin) * FontScale;

                var sprite = layer.CreateSprite(texture.Path, Origin, position);
                sprite.Scale(line.StartTime, FontScale);
                sprite.Fade(line.StartTime - 200, line.StartTime, 0, 1);
                sprite.Fade(line.EndTime, line.EndTime + 400, 1, 0);
            }
        }
    }
}
