// Drop this script in the Editor directory in your project (creating the Editor directory if it's not there yet)
// Then re-import the assets in the directories covered by this script (right click and reimport)
//
// I would replace my path checking with the path checking from this gist:
//   https://gist.github.com/1842177
//
// The texture settings for icons might want to use some of his settings combined with mine as well


using UnityEngine;
using UnityEditor;
using System.Collections;


class ProjectAssetPostprocessor : AssetPostprocessor
{
	
	
	// texture asset preprocessor
	void OnPreprocessTexture()
	{
		// check if it's a platform image (icons, splash, etc.)
		if( assetPath.StartsWith( "Assets/-UI" ) )
		{
			PreprocessUISprites();
		}
			
		
	}

	
	
		
	// preprocess platform icons, etc.
	void PreprocessUISprites()
	{
		TextureImporter importer = (TextureImporter)assetImporter;

		importer.textureType = TextureImporterType.Sprite;
		
	}

	
	
}
