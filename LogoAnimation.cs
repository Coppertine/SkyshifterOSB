using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class LogoAnimation : StoryboardObjectGenerator
    {
        public static int StepSize = 20;
        public static double StartTime = 185569;
        public static double MidTime = 186981;
        public static double EndTime = 190511;
        public int segmentDelay = (int)((MidTime - StartTime) / StepSize);

        public override void Generate() {
            var startPosition = PositionAt(0f);
            
            GenerateImage();
            
            var logoBack = GetLayer("").CreateSprite("sb/logoBack.png", OsbOrigin.Centre, new Vector2(320, 230));
            logoBack.Scale(StartTime, .5f);
            logoBack.Fade(EndTime, .7f);

            //Cover generation
            for (int i = 0; i < StepSize; i++) {
                var sprite = GetLayer("").CreateSprite("sb/pixel.png", OsbOrigin.CentreRight, PositionAt(i / (float)StepSize));
                sprite.Color(StartTime, Color4.Black);

                var prev = PositionAt(i / (float)StepSize);
                var next = PositionAt((i + 1) / (float)StepSize);

                //sprite.ScaleVec(StartTime + (i - 1) * segmentDelay, StartTime + i * segmentDelay, (prev - next).Length + 10, 10, 0, 10);
                sprite.ScaleVec(StartTime + i * segmentDelay, (prev - next).Length + 10, 15);
                sprite.Rotate(StartTime, RotationAt(i / (float)StepSize) + Math.PI) ;//Math.Atan2(next.Y - prev.Y, next.X - prev.X));

                if (sprite.CommandsEndTime > 186805) { 
                    sprite.Fade(sprite.CommandsStartTime, 1f);
                    sprite.Fade(186805, 0f);
                }
            }

            var positions = new float[] { 0.3f, .5f, .6f, .65f };
            ConnectPoints(0f, positions[0]);
            ConnectPoints(0f, positions[1]);
            ConnectPoints(0f, positions[3]);

            ConnectPoints(positions[0], positions[2]);

            ConnectPoints(positions[0], 1f);
            ConnectPoints(positions[2], 1f);

            var gradient = GetLayer("").CreateSprite("sb/logoGradient.png", OsbOrigin.Centre, startPosition);
            gradient.ScaleVec(StartTime, .6, 20);
            gradient.Color(StartTime, Color4.Black);
            gradient.Fade(186805, MidTime, 1f, 0f);
            MoveAlong(gradient, 1f, true);

            for (int i = 3; i >= 0; i--) {
                var dot = GetLayer("").CreateSprite("sb/logoDot.png", OsbOrigin.Centre, startPosition);
                dot.Scale(EndTime, 2 - i / 2f);

                MoveAlong(dot, positions[i]);
            }

            var text = GetLayer("").CreateSprite("sb/logoText.png", OsbOrigin.Centre, new Vector2(310, 245));
            text.Scale(OsbEasing.InOutSine, StartTime, EndTime, .49f, .5f);
            text.Fade(OsbEasing.OutExpo, StartTime, EndTime, 0f, 1f);
        }

        public void ConnectPoints(float start, float end) { 
            var prev = PositionAt(start);
            var next = PositionAt(end);

            var layer = GetLayer("");
            var sprite = layer.CreateSprite("sb/pixel.png", OsbOrigin.CentreLeft, prev);

            RotateAlong(sprite, prev, start, end);
            sprite.Fade(EndTime, .5f);
        }

        public void MoveAlong(OsbSprite sprite, float end, bool rotate=false) {
            var last = StartTime;
            for (int i = 1; i < StepSize; i++) {
                var time = StartTime + i * segmentDelay;
                var position = PositionAt(Math.Min(end, i / (float)StepSize));

                sprite.Move(last, time, sprite.PositionAt(last), position);
                if(rotate)
                    sprite.Rotate(last, time, sprite.RotationAt(last), RotationAt(Math.Min(end, i / (float)StepSize)));
                    
                last = time;
            }
        }

        public void RotateAlong(OsbSprite sprite, Vector2 startPosition, float start, float end) {
            var last = StartTime;
            for (int i = 1; i < StepSize; i++) {
                var time = StartTime + i * segmentDelay;
                
                if(start >= i / (float)StepSize) {
                    last = time;
                    continue;
                }

                var position = PositionAt(Math.Min(end, i / (float)StepSize));

                sprite.Rotate(last, time, sprite.RotationAt(last), Math.Atan2(position.Y - startPosition.Y, position.X - startPosition.X));
                sprite.ScaleVec(last, time, sprite.ScaleAt(last), (startPosition - position).Length + 2, 1);

                last = time;
            }
        }

        public Vector2 PositionAt(float percentage, float scale = 100f)
        {
            var t = Math.PI * 2 - 3.8 * (1 - percentage);

            return scale * new Vector2(
                3 * (float)Math.Cos(t),
                (float)Math.Sin(t) * (float)Math.Sin(t / 2f)
            ) + new Vector2(320, 240);
        }

        public float RotationAt(float percentage)
        {
            var prev = PositionAt(percentage - 0.01f);
            var next = PositionAt(percentage + 0.01f);

            return (float)Math.Atan2(next.Y - prev.Y, next.X - prev.X);
        }

        //generate this stupid shape. since mr. mapper lost it
        public void GenerateImage()
        {
            var steps = 200;
            var positions = new Vector2[steps];

            for (int i = 0; i < steps; i++)
                positions[i] = PositionAt(i / (float)steps, scale:200);

            var min = Min(positions);
            var max = Max(positions);
            var dim = max - min;

            var localPositions = new List<Vector2>(positions.Length);
            Array.ForEach(positions, p => localPositions.Add(new Vector2(p.X - min.X, p.Y - min.Y)));
            
            var bitmap = new System.Drawing.Bitmap((int)dim.X + 1, (int)dim.Y + 1);
            var image = (Image)bitmap;
            var graphics = Graphics.FromImage(image);
            var pen = new Pen(Color.White, 2);

            for (var i = 1; i < steps; i++)
                graphics.DrawLine(pen, localPositions[i - 1].X, localPositions[i - 1].Y, localPositions[i].X, localPositions[i].Y);

            bitmap.Save(System.IO.Path.Combine(ProjectPath, "logoRaw.png"));
            
            //Cleanup
            bitmap.Dispose();
            image.Dispose();
            graphics.Dispose();
            pen.Dispose();
        }

        Vector2 Min(Vector2[] points)
        {
            var minX = points[0].X;
            var minY = points[0].Y;
            for (int i = 1; i < points.Length; i++)
            {
                minX = Math.Min(minX, points[i].X);
                minY = Math.Min(minY, points[i].Y);
            }

            return new Vector2(minX, minY);
        }

        Vector2 Max(Vector2[] points)
        {
            var maxX = points[0].X;
            var maxY = points[0].Y;
            for (int i = 1; i < points.Length; i++)
            {
                maxX = Math.Max(maxX, points[i].X);
                maxY = Math.Max(maxY, points[i].Y);
            }

            return new Vector2(maxX, maxY);
        }
    }
}
