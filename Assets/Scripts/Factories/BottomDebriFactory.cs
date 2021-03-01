using UnityEngine;
using Assets.Scripts.Seasons;
using Assets.Scripts.Framework;
using Assets.Scripts.Controllers;
using System.Collections.Generic;
using Assets.Scripts.Views.Seasons;
using Assets.Scripts.AssetsManagers;
using Assets.Scripts.Framework.Utils;
using Assets.Scripts.Framework.AssetsManagers;

namespace Assets.Scripts.Factories
{
	public class BottomDebriFactory : RDOnDemandFactory<BottomDebriViewPoolObject, BottomDebriView>
	{
		public const int AT_LEAST = 30;

		private const float SCALE_MIN = 0.2f;
		private const float SCALE_MAX = 2f;

		private static readonly Color ColoMin = new Color(0.7f, 0.7f, 0.7f, 1f);
		private static readonly Color ColorMax = Statics.COLOR_WHITE;

		private static readonly string[] DebriArray = new string[]
		{
			"stone1",
			"stone2",
			"stone3",
			"stone4",
			"stone5",
			"stone6"
		};

		private static readonly Sprite[] SpriteArray = new Sprite[DebriArray.Length];

		public void RecycleBottomDebri(List<BottomDebriViewPoolObject> bottomDebri, Transform parent, int slices, Vector3 center, float waterDepthWidth)
		{
			Area2D area;
			float centerYOrigin = center.y;
			int debriIndex = 0;

			for (int i = 0; i < slices; i++)
			{
				center.y = centerYOrigin + i * ViewController.Area.Height;
				area = new Area2D(waterDepthWidth, ViewController.Area.Height, center);

				for (int j = 0; j < AT_LEAST; j++)
				{
					if (debriIndex >= bottomDebri.Count)
						bottomDebri.Add(GetAvailable());

					bottomDebri[debriIndex].Spawn(parent, area.GetRandomPosition());
					bottomDebri[debriIndex].View.SetSprite(GetRandomSprite());
					bottomDebri[debriIndex].View.transform.localScale = Statics.VECTOR3_ONE * Random.Range(SCALE_MIN, SCALE_MAX);
					bottomDebri[debriIndex].View.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 359f));
					bottomDebri[debriIndex].View.SetColor(Color.Lerp(ColoMin, ColorMax, Random.value));
					debriIndex++;
				}
			}

			if (debriIndex < bottomDebri.Count)
			{
				for (int i = debriIndex; i < bottomDebri.Count; i++)
					bottomDebri[i].Despawn();

				bottomDebri.RemoveRange(debriIndex, bottomDebri.Count - debriIndex);
			}
		}

		private static Sprite GetRandomSprite()
		{
			int debriSpriteIndex = Random.Range(0, DebriArray.Length);

			if (SpriteArray[debriSpriteIndex] == null)
				SpriteArray[debriSpriteIndex] = AssetLoader.ME.Loader<Sprite>("Sprites/BottomDebri/" + DebriArray[debriSpriteIndex]);

			return SpriteArray[debriSpriteIndex];
		}

		protected override void UnloadPrefab() => BottomDebriViewPoolObject.UnloadPrefab();
	}
}
