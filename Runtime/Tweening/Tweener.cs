using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeneralUnityUtils.Tweening
{
    public class Tweener : MonoBehaviour
    {
        #region SINGLETON
        public static Tweener Instance
        {
            get
            {
                if (_singleton == null) _singleton = GameObject.FindObjectOfType<Tweener>();
                return _singleton;
            }
        }
        private static Tweener _singleton;
        #endregion

        /// <summary>
        /// A callback used when the tween item makes it to its destination.
        /// </summary>
        /// <param name="target">The target of the completed tween operation.</param>
        public delegate void OnTweenComplete(GameObject target);

        /// <summary>
        /// A callback used whenever the tween position is updated. Allows for tweening multiple
        /// properties of an object at once other than position.
        /// </summary>
        /// <param name="target"></param>
        public delegate void OnTweenUpdate(GameObject target, float percentage);

        List<(TweenItem tween, OnTweenComplete completeCallback, OnTweenUpdate updateCallback, bool useScaleTime)> currentTweens = new List<(TweenItem tween, OnTweenComplete completeCallback, OnTweenUpdate updateCallback, bool useScaleTime)>();

        /// <summary>
        /// Tween an item between two positions over a set amount of time.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="transform"></param>
        /// <param name="time"></param>
        public void Tween(Vector2 start, Vector2 end, Transform transform, float time, OnTweenComplete callback, bool useScaledTime = true, OnTweenUpdate update = null)
        {
            currentTweens.Add((new StaticTween(start, end, time, transform), callback, update, useScaledTime));
        }

        /// <summary>
        /// Tween an item between two positions over a set amount of time.
        /// This tween is dynamic and follows the start and endpoints.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="transform"></param>
        /// <param name="time"></param>
        public void Tween(Transform start, Transform end, Transform transform, float time, OnTweenComplete callback, bool useScaledTime, OnTweenUpdate update = null)
        {
            currentTweens.Add((new DynamicTween(start, end, time, transform), callback, update, useScaledTime));
        }

        /// <summary>
        /// Tween a recttransform between two positions over a set amount of time.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="transform"></param>
        /// <param name="time"></param>
        public void Tween(Vector2 start, Vector2 end, RectTransform transform, float time, OnTweenComplete callback, bool useScaleTime, OnTweenUpdate update = null)
        {
            currentTweens.Add((new RectTween(start, end, time, transform), callback, update, useScaleTime));
        }

        void Awake()
        {
            /* First make sure we are the only tweener. If not, destroy this instance */
            if (_singleton != null) Destroy(gameObject);

            /* We are the singleton */
            DontDestroyOnLoad(gameObject);
        }

        public void Update()
        {
            /* Update each tween. handle tweens which have completed. */
            List<(TweenItem, OnTweenComplete, OnTweenUpdate, bool)> completed = new List<(TweenItem, OnTweenComplete, OnTweenUpdate, bool)>();
            foreach(var tween in currentTweens)
            {
                float delta = tween.useScaleTime ? Time.deltaTime : Time.unscaledDeltaTime;
                bool completedTween = tween.tween.UpdatePosition(delta);
                if (tween.updateCallback != null) tween.updateCallback(tween.tween.TargetTransform.gameObject, tween.tween.GetPercentage());
                if (completedTween)
                {
                    /* completed tween */
                    completed.Add(tween);
                    if(tween.completeCallback != null) tween.completeCallback(tween.tween.TargetTransform.gameObject);
                }
            }
            
            /* Remove any completed tweens */
            foreach(var tween in completed)
            {
                currentTweens.Remove(tween);
            }
        }

    }
}