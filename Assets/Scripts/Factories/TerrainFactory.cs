using UnityEngine;
using Assets.Scripts.Seasons;
using System.Collections.Generic;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Factories
{
	public class TerrainFactory : RDOnDemandFactory<TerrainViewPoolObject, SpriteRenderer>
	{
		protected override void UnloadPrefab() => TerrainViewPoolObject.UnloadPrefab();

		public void RecycleTerrain(List<TerrainViewPoolObject> terrainPool, SpriteRenderer waterSurfaceSpriteRend, string typeStr)
		{
			TerrainViewPoolObject terrainBase = terrainPool.Count == 0 ?
				SpawnBase(waterSurfaceSpriteRend, typeStr) :
				SpawnBase(terrainPool[0], waterSurfaceSpriteRend, typeStr);

			float yInitPos = terrainBase.View.transform.localPosition.y;
			int totalNum = Mathf.FloorToInt(waterSurfaceSpriteRend.bounds.size.y / terrainBase.View.bounds.size.y);
			int doubleTotalNum = totalNum * 2;

			for (int i = 1; i < doubleTotalNum; i++)
			{
				if (i >= terrainPool.Count)
				{
					terrainBase = SpawnAndCopyNext(terrainBase, i == totalNum);
					terrainPool.Add(terrainBase);
				}
				else
					terrainBase = CopyNext(terrainBase, terrainPool[i], i == totalNum);

				Vector3 newLocalPos = terrainBase.View.transform.localPosition;
				newLocalPos.y += terrainBase.View.bounds.size.y;

				if (i == totalNum)
				{
					newLocalPos.y = yInitPos;
					newLocalPos.x *= -1;
				}

				terrainBase.View.transform.localPosition = newLocalPos;
			}

			if (doubleTotalNum < terrainPool.Count)
			{
				for (int i = doubleTotalNum; i < terrainPool.Count; i++)
					terrainPool[i].Despawn();

				terrainPool.RemoveRange(doubleTotalNum, terrainPool.Count - doubleTotalNum);
			}
		}

		public void BuildTerrain(SpriteRenderer waterSurfaceSpriteRend, string typeStr)
		{
			TerrainViewPoolObject terrainBase = SpawnBase(waterSurfaceSpriteRend, typeStr);

			int totalNum = Mathf.FloorToInt(waterSurfaceSpriteRend.bounds.size.y / terrainBase.View.bounds.size.y);
			int doubleTotalNum = totalNum * 2;
			
			for (int i = 1; i < doubleTotalNum; i++)
				SpawnAndCopyNext(terrainBase, i == totalNum);
		}

		private TerrainViewPoolObject SpawnBase(SpriteRenderer waterSurfaceSpriteRend, string typeStr)
		{
			TerrainViewPoolObject terrain = GetAvailable();
			return SpawnBase(terrain, waterSurfaceSpriteRend, typeStr);
		}

		private TerrainViewPoolObject SpawnBase(TerrainViewPoolObject terrain, SpriteRenderer waterSurfaceSpriteRend, string typeStr)
		{
			Sprite terrainSprite = AssetLoader.ME.Loader<Sprite>($"Sprites/Terrain/Terrain{typeStr}");
			terrain.SpawnBase(waterSurfaceSpriteRend, terrainSprite);
			return terrain;
		}

		private TerrainViewPoolObject SpawnAndCopyNext(TerrainViewPoolObject terrainBase, bool isFlipped)
		{
			TerrainViewPoolObject next = GetAvailable();
			return CopyNext(terrainBase, next, isFlipped);
		}

		private TerrainViewPoolObject CopyNext(TerrainViewPoolObject terrainBase, TerrainViewPoolObject next, bool isFlipped)
		{
			next.CloneAndSpawn(terrainBase, isFlipped);
			return next;
		}
	}
}
