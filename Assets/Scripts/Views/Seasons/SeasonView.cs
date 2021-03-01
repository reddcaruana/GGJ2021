using UnityEngine;
using Assets.Scripts.Seasons;
using Assets.Scripts.Factories;
using System.Collections.Generic;
using Assets.Scripts.Controllers;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Views.Seasons.Paywalls;
using Assets.Scripts.AssetsManagers;

namespace Assets.Scripts.Views.Seasons
{
	public class SeasonView : MonoBehaviour
	{
		private const float WATER_DEPTH_NORMALISED = 0.9f;

		private readonly List<BottomDebriViewPoolObject> BottomDebri = new List<BottomDebriViewPoolObject>();
		private readonly List<TerrainViewPoolObject> Terrain = new List<TerrainViewPoolObject>();

		public PaywallView PaywallView { get; private set; }
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

			PaywallView = MonoBehaviour.Instantiate(AssetLoader.ME.Loader<GameObject>($"Prefabs/Paywalls/PaywallViewBase"), transform).AddComponent<PaywallView>();
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
			FactoryManager.Terrain.RecycleTerrain(Terrain, waterSurfaceSpriteRend, typeStr);
			DistributeUnderwaterAssets();
		}

		private void DistributeUnderwaterAssets()
		{
			int slices = Mathf.FloorToInt(waterSurfaceSpriteRend.size.y / ViewController.Area.Height);

			Vector3 center = new Vector3()
			{
				x = 0,
				y = (-waterSurfaceSpriteRend.bounds.size.y / 2f) + ViewController.Area.HalfHeight
			};

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
