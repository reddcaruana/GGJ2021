using UnityEngine;
using Assets.Scripts.Seasons;
using Assets.Scripts.Factories;
using Assets.Scripts.Framework;
using System.Collections.Generic;
using Assets.Scripts.Controllers;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.Utils;

namespace Assets.Scripts.Views.Seasons
{
	public class SeasonView : MonoBehaviour
	{
		private const float WATER_DEPTH_NORMALISED = 0.9f;

		private readonly List<BottomDebriViewPoolObject> BottomDebri = new List<BottomDebriViewPoolObject>();
		public Transform FishTank { get; private set; }
		private SpriteRenderer waterSurfaceSpriteRend;
		private SpriteRenderer waterDepthSpriteRend;

		private void Awake()
		{
			waterSurfaceSpriteRend = transform.Find("SpriteWaterSurface").GetComponent<SpriteRenderer>();
			waterDepthSpriteRend = waterSurfaceSpriteRend.transform.Find("SpriteBottom").GetComponent<SpriteRenderer>();
			FishTank = new GameObject("FishTank").transform;
			FishTank.SetParent(transform);
			FishTank.ResetTransforms();

		}

		public void Set(Vector2 viewSize, Vector3 tankCeenterLocalPos, string typeStr, Vector3 baseWorldPosition)
		{

			waterSurfaceSpriteRend.drawMode = SpriteDrawMode.Sliced;
			waterSurfaceSpriteRend.size = viewSize;

			Vector2 waterDepthSize = viewSize;
			waterDepthSize.x = viewSize.x * WATER_DEPTH_NORMALISED;
			waterDepthSpriteRend.drawMode = SpriteDrawMode.Sliced;
			waterDepthSpriteRend.size = waterDepthSize;

			BuildTerrain(typeStr);

			PositionFromBase(baseWorldPosition);

			FishTank.localPosition = tankCeenterLocalPos;
		}

		public void PositionFromBase(Vector3 baseWorldPosition)
		{
			baseWorldPosition.y += (waterSurfaceSpriteRend.bounds.size.y / 2f);
			transform.position = baseWorldPosition;
		}

		public void PositionFromTop(Vector3 topWorldPosition)
		{
			topWorldPosition.y -= (waterSurfaceSpriteRend.bounds.size.y / 2f);
			transform.position = topWorldPosition;
		}

		private void BuildTerrain(string typeStr)
		{
			SpriteRenderer terrainSidePrefab = CreateTerrainSidePrefab(typeStr);

			int totalNum = Mathf.FloorToInt(waterSurfaceSpriteRend.bounds.size.y / terrainSidePrefab.bounds.size.y);
			int doubleTotalNum = totalNum * 2;

			float initY = terrainSidePrefab.transform.localPosition.y;
			Vector3 pos;
			for (int i = 1; i < doubleTotalNum; i++)
			{
				Create();
				if (i == totalNum)
					Flip();

			}

			void Create()
			{
				terrainSidePrefab = Instantiate(terrainSidePrefab);

				terrainSidePrefab.transform.SetParent(waterSurfaceSpriteRend.transform);
				pos = terrainSidePrefab.transform.localPosition;
				pos.y += terrainSidePrefab.bounds.size.y;
				terrainSidePrefab.transform.localPosition = pos;

			}

			void Flip()
			{
				pos.y = initY;
				pos.x = -pos.x;
				Vector3 scale = terrainSidePrefab.transform.localScale;
				scale.x *= -1;
				terrainSidePrefab.transform.localPosition = pos;
				terrainSidePrefab.transform.localScale = scale;
			}

			DistributeUnderwaterAssets();
		}

		private SpriteRenderer CreateTerrainSidePrefab(string typeStr)
		{
			Sprite terrainSprite = AssetLoader.ME.Loader<Sprite>($"Sprites/Terrain/Terrain{typeStr}");
			SpriteRenderer terrain = new GameObject("Terrain0").AddComponent<SpriteRenderer>();
			terrain.sprite = terrainSprite;
			terrain.transform.SetParent(waterSurfaceSpriteRend.transform);
			terrain.transform.ResetTransforms();

			float x = (ViewController.Area.Width - waterSurfaceSpriteRend.bounds.size.x) / 2f;
			float s = x / terrain.bounds.size.x;
			Vector3 vs = Statics.VECTOR3_ONE * s;
			terrain.transform.localScale = vs;

			float strechedY = waterSurfaceSpriteRend.size.y / Mathf.FloorToInt(waterSurfaceSpriteRend.size.y / terrain.bounds.size.y);
			vs.y = (strechedY / terrain.bounds.size.y) * s;
			terrain.transform.localScale = vs;

			terrain.transform.localPosition = new Vector3()
			{
				x = ViewController.Area.HalfWidth - (terrain.bounds.size.x / 2f),
				y = (-waterSurfaceSpriteRend.bounds.size.y / 2f) + (terrain.bounds.size.y / 2f)
			};

			return terrain;
		}

		private void DistributeUnderwaterAssets()
		{
			int slices = Mathf.FloorToInt(waterSurfaceSpriteRend.size.y / ViewController.Area.Height);

			Vector3 center = GetBottomWorldPosition();
			center.y += ViewController.Area.HalfHeight;

			FactoryManager.BottomDebri.RecycleBottomDebri(BottomDebri, waterDepthSpriteRend.transform, slices, center, waterDepthSpriteRend.size.x);
		}

		public Vector3 GetTopWorldPosition() => GetTopWorldPosition(transform.position);

		public Vector3 GetTopWorldPosition(Vector3 centerWorldPos)
		{
			Vector3 result = centerWorldPos;
			result.y += (waterSurfaceSpriteRend.bounds.size.y / 2f);
			return result;
		}

		public Vector3 GetBottomWorldPosition() => GetBottomWorldPosition(transform.position);

		public Vector3 GetBottomWorldPosition(Vector3 centerWorldPos)
		{
			Vector3 result = centerWorldPos;
			result.y -= (waterSurfaceSpriteRend.size.y / 2f);
			return result;
		}

		public Vector3 BoundSize() => waterSurfaceSpriteRend.bounds.size;
	}
}
