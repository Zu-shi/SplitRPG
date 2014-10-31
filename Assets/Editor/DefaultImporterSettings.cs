using UnityEditor;
using UnityEngine;
using System.Collections;

public class DefaultImporterSettings : AssetPostprocessor {
	
	void OnPreprocessTexture() {
		//TextureImporter importer = assetImporter as TextureImporter;
		//importer.spritePixelsToUnits = 64f;
	}
}