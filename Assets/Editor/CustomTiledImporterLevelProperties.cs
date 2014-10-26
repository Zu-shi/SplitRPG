using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

[Tiled2Unity.CustomTiledImporter]
public class CustomTiledImporterLevelProperties : Tiled2Unity.ICustomTiledImporter {

	public void HandleCustomProperties(GameObject gameObject, IDictionary<string, string> props){
		if(gameObject.transform.parent == null){
			LevelPropertiesScript tmp = gameObject.AddComponent<LevelPropertiesScript>();
			if(props.ContainsKey("canJump") && props["canJump"].ToLower() != "false")
				tmp.canJump = true;
			if(props.ContainsKey("canPushHeavy") && props["canPushHeavy"].ToLower() != "false")
				tmp.canPushHeavy = true;
			if(props.ContainsKey("fallInWater") && props["fallInWater"].ToLower() != "false")
				tmp.fallInWater = true;
		}
	}
	
	public void CustomizePrefab(GameObject prefab){}
}
