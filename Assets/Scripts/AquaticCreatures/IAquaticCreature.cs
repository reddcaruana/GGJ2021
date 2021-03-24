using UnityEngine;
using Assets.Scripts.AquaticCreatures.Fish;
using Assets.Scripts.Generic;

namespace Assets.Scripts.AquaticCreatures
{
	public interface IAquaticCreature
	{
		FishData Data { get; }
		float Weight { get; }
		float Size { get; }
		Energy Energy { get; }
		bool IsReadyToSet { get; } 
		bool CanCatch { get; }

		void Set(FishData data);
		Vector3 GetLocalSpawnPosition();
		Vector3 GetViewWorldPosition();
		IAquaticCreature OnCast(Vector3 floatPosition);
		void Appear();
		void Escape();
		void ApproachFloat(Vector2 floatLocalPosition);
		void Fight();
		void ReelIn(float speed);
		FishLogData Caught();
	}
}
