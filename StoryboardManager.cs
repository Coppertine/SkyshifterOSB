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
    public class StoryboardManager : StoryboardObjectGenerator
    {
        [Configurable] public string gitSpritesPath;
        [Configurable] public bool Refresh;
        public override void Generate()
        {
            ImportAssets();
						ExportAssets();
						Log("Done! (" + System.DateTime.Now.TimeOfDay + ")");
        }
        void ImportAssets()
        {
            //set the direction Path to copy assets
            string dirPath = MapsetPath + "/sb/";
            //Check if the direction path exist, if not create a new one and restart the function
            if(System.IO.Directory.Exists(dirPath))
            {
                string[] files = System.IO.Directory.GetFiles(gitSpritesPath);
                //List each elements of the gitrepo sprite folder then get their file name and copy them in the destination folder
                foreach(var file in files)
                {
                    var fileName = System.IO.Path.GetFileName(file);
                    var destFile = System.IO.Path.Combine(dirPath, fileName);
                    //Check if the file is already existing in the mapset sprite folder, if yes it copy the file!
                    if(!System.IO.File.Exists(destFile))
                    {
                        System.IO.File.Copy(file, destFile, true);
                        Log("Added " + fileName + " To " + dirPath);
                    } 
                }
            }
            else
            {
                System.IO.Directory.CreateDirectory(dirPath);
                Log("Created sprite directory in " + dirPath);
                ImportAssets();
                return;
            }
            //After every instructions are done :)
            
        }
				
				void ExportAssets()
				{
					string dirPath = MapsetPath + "/sb/";
					
					if(System.IO.Directory.Exists(dirPath))
          {
						//get list of files from sb folder
						string[] files = System.IO.Directory.GetFiles(dirPath);
						 foreach(var file in files)
                {
                    var fileName = System.IO.Path.GetFileName(file);
                    var destFile = System.IO.Path.Combine(gitSpritesPath, fileName);
                    //Check if the file is already existing in the mapset sprite folder, if yes it copy the file!
                    if(!System.IO.File.Exists(destFile))
                    {
                        System.IO.File.Copy(file, destFile, true);
                        Log("Added " + fileName + " To " + gitSpritesPath);
                    } 
                }
            }
            else
            {
                System.IO.Directory.CreateDirectory(gitSpritesPath);
                Log("Created sprite directory in " + gitSpritesPath);
                ExportAssets();
                return;
            }
						
						
					}
				
				}
 }