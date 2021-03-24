using UnityEngine;
using Assets.Scripts.Seasons;
using Assets.Scripts.Factories;
using System.Collections.Generic;
using Assets.Scripts.Controllers;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Views.Seasons.Paywalls;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Views.Seasons
{
	public class SeasonView : MonoBehaviour
	{
		private const float WATER_DEPTH_NORMALISED = 0.9f;
		private static SpriteRenderer waterDepthSpriteRend;

		private readonly List<BottomDebriViewPoolObject> BottomDebri = new List<BottomDebriViewPoolObject>();
		private readonly List<TerrainViewPoolObject> Terrain = new List<TerrainViewPoolObject>();

		public PaywallView PaywallView { get; private set; }
		public Transform FishTank { get; private set; }

		private SpriteRenderer waterSurfaceSpriteRend;
		float halfWaterSurfaceSizzeY;

		private void Awake()
		{
			if (waterDepthSpriteRend == null)
			{
				waterDepthSpriteRend = Instantiate(AssetLoader.ME.Load<SpriteRenderer>("Prefabs/Stream/BottomEffect"), ViewController.MainCamera.transform);
				waterDepthSpriteRend.drawMode = SpriteDrawMode.Sliced;
				waterDepthSpriteRend.transform.ResetTransforms();
				waterDepthSpriteRend.transform.localPosition = new Vector3(0, 0, 10);
				waterDepthSpriteRend.size = ViewController.Area.Size;
			}
			
			waterSurfaceSpriteRend = transform.Find("SpriteWaterSurface").GetComponent<SpriteRenderer>();
			waterSurfaceSpriteRend.drawMode = SpriteDrawMode.Sliced;
			waterSurfaceSpriteRend.transform.ResetScale();

			
			FishTank = new GameObject("FishTank").transform;
			FishTank.SetParent(transform);
			FishTank.ResetTransforms();

			PaywallView = MonoBehaviour.Instantiate(AssetLoader.ME.Load<GameObject>($"Prefabs/Paywalls/PaywallViewBase"), transform).AddComponent<PaywallView>();
		}

		public void Set(Vector2 viewSize, Vector3 tankCeenterLocalPos, string typeStr, Vector3 baseWorldPosition)
		{
			waterSurfaceSpriteRend.size = viewSize;
			halfWaterSurfaceSizzeY = waterSurfaceSpriteRend.bounds.size.y / 2f;

			Vector2 waterDepthSize = waterDepthSpriteRend.size;
			waterDepthSize.x = viewSize.x * WATER_DEPTH_NORMALISED;
			waterDepthSpriteRend.size = waterDepthSize;

			BuildTerrain(typeStr);

			PositionFromBase(baseWorldPosition);

			FishTank.localPosition = tankCeenterLocalPos;
		}

		public void PositionFromBase(Vector3 baseWorldPosition)
		{
			baseWorldPosition.y += halfWaterSurfaceSizzeY;
			transform.position = baseWorldPosition;
		}

		public void PositionFromTop(Vector3 topWorldPosition)
		{
			topWorldPosition.y -= halfWaterSurfaceSizzeY;
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
				y = -halfWaterSurfaceSizzeY + ViewController.Area.HalfHeight
			};

			FactoryManager.BottomDebri.RecycleBottomDebri(BottomDebri, waterSurfaceSpriteRend.transform, slices, center, waterDepthSpriteRend.size.x);
		}

		public Vector3 GetTopWorldPosition() => GetTopWorldPosition(transform.position);

		public Vector3 GetTopWorldPosition(Vector3 centerWorldPos)
		{
			Vector3 result = centerWorldPos;
			result.y += halfWaterSurfaceSizzeY;
			return result;
		}

		public Vector3 GetBottomWorldPosition() => GetBottomWorldPosition(transform.position);

		public Vector3 GetBottomWorldPosition(Vector3 centerWorldPos)
		{
			Vector3 result = centerWorldPos;
			result.y -= halfWaterSurfaceSizzeY;
			return result;
		}

		public Vector3 BoundSize() => waterSurfaceSpriteRend.bounds.size;
	}
}
