// Author: Zoodinger
// http://www.schemingdeveloper.com
// http://www.stopthegnomes.com

using UnityEngine;
using System.Collections;

namespace SpriteTile {
	public class SpriteTileDemo : MonoBehaviour {

		private int tilesX = 1;
		private int tilesY = 1;

		SpriteRenderer sr;

		// Use this for initialization
		void Start() {
			sr = GetComponent<SpriteRenderer>();
		}

		void UpdateTiles() {
			var size = sr.sprite.bounds.size;
			//We need to resize the sprite according to how many tiles we want.
			//If we don't do this, all tiles will be squished into the original bounds of the sprite.
			//That might be intended in some cases, so this step is optional.
			transform.localScale = new Vector3(size.x * tilesX, size.y * tilesY, 1);

			//Set the x and y tile factors. The values don't have to be integers.
			sr.material.SetFloat("RepeatX", tilesX);
			sr.material.SetFloat("RepeatY", tilesY);
		}

		void OnGUI() {
			GUILayout.BeginVertical();
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label("Horizontal");
					if (GUILayout.Button("<")) {
						tilesX = Mathf.Clamp(tilesX - 1, 1, 32);
						UpdateTiles();
					}
					GUILayout.Label(tilesX.ToString());
					if (GUILayout.Button(">")) {
						tilesX = Mathf.Clamp(tilesX + 1, 1, 32);
						UpdateTiles();
					}
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				{
					GUILayout.Label("Vertical");
					if (GUILayout.Button("<")) {
						tilesY = Mathf.Clamp(tilesY - 1, 1, 32);
						UpdateTiles();
					}
					GUILayout.Label(tilesY.ToString());
					if (GUILayout.Button(">")) {
						tilesY = Mathf.Clamp(tilesY + 1, 1, 32);
						UpdateTiles();
					}
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndVertical();
		}
	}
}