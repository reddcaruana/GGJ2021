using UnityEngine;
using Assets.Scripts.Framework.AssetsManagers;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Controllers;
using Assets.Scripts.Framework;
using System;

namespace Assets.Scripts.Seasons
{
	public class TerrainViewPoolObject : PoolObject<SpriteRenderer>
	{
		private static SpriteRenderer prefab;
		public static SpriteRenderer Prefab
		{
			get
			{
				if (prefab == null)
					prefab = new GameObject("Terrain").AddComponent<SpriteRenderer>();
				return prefab;
			}
		}

		public TerrainViewPoolObject() : base(MonoBehaviour.Instantiate(Prefab, null))
		{
		}

		public static void UnloadPrefab() => prefab = null;

		public void Spawn(Transform parent, Sprite sprite, Vector3 scale, bool isFlippedScale, Vector3 position, bool isLocalPosition = true)
		{
			Spawn(parent, position, isLocalPosition);
			View.sprite = sprite;

			if (isFlippedScale)
				scale.x *= -1;

			View.transform.localScale = scale;
		}

		public void SpawnBase(SpriteRenderer waterSurfaceSpriteRend, Sprite terrainSprite)
		{
			Spawn(waterSurfaceSpriteRend.transform);
			View.sprite = terrainSprite;
			View.transform.ResetTransforms(isWorld: false);

			float x = (ViewController.Area.Width - waterSurfaceSpriteRend.bounds.size.x) / 2f;
			float s = x / View.bounds.size.x;
			Vector3 vs = Statics.VECTOR3_ONE * s;
			View.transform.localScale = vs;

			float strechedY = waterSurfaceSpriteRend.size.y / Mathf.FloorToInt(waterSurfaceSpriteRend.size.y / View.bounds.size.y);
			vs.y = (strechedY / View.bounds.size.y) * s;
			View.transform.localScale = vs;

			View.transform.localPosition = new Vector3()
			{
				x = ViewController.Area.HalfWidth - (View.bounds.size.x / 2f),
				y = (-waterSurfaceSpriteRend.bounds.size.y / 2f) + (View.bounds.size.y / 2f)
			};
		}

		public void CloneAndSpawn(TerrainViewPoolObject terrainBase, bool isFlipped)
		{
			Spawn(terrainBase.View.transform.parent, terrainBase.View.sprite, terrainBase.View.transform.localScale, isFlipped, terrainBase.View.transform.localPosition);
		}
	}
}
