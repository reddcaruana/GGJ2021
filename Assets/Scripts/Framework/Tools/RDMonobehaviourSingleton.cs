using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Framework.Tools
{
	public class RDMonobehaviourSingleton<T> : MonoBehaviour
        where T : Component
	{
        private static bool quit;

        private static T me;
        public static T ME
        {
            get
            {
                if (quit) return null;
                if (me != null) return me;

                me = FindObjectOfType<T>();

                if (me != null) return me;

                var obj = new GameObject(typeof(T).ToString().Split('.').Last());
                me = obj.AddComponent<T>();
                return me;
            }
        }

        public bool IsPersistent { get; private set; }

        protected virtual void Awake()
        {
            if (me == null)
                me = this as T;

            else
            {
                Debug.LogWarning("SECOND INSTANCE OF SINGLETON... VERIFY! " + typeof(T));
                Destroy(gameObject);
            }
        }

        protected void OnDestroy()
        {
            me = null;
        }

        protected virtual void OnApplicationQuit()
        {
            quit = true;
        }

        public void MakePersistent()
        {
            if (gameObject == null)
                return;

            IsPersistent = true;
            DontDestroyOnLoad(gameObject);
        }
    }
}
