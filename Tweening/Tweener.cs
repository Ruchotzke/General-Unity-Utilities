using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneralUnityUtils.Easing;
using static GeneralUnityUtils.Easing.Easing;

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

        /// <summary>
        /// The container for all tweens currently being operated on.
        /// </summary>
        List<(TweenItem tween, OnTweenComplete completeCallback, OnTweenUpdate updateCallback, bool useScaleTime)> currentTweens = new();

        /// <summary>
        /// A list of tweens to be added to the list on the following frame. useful to avoid concurrent modification exceptions.
        /// </summary>
        List<(TweenItem tween, OnTweenComplete completeCallback, OnTweenUpdate updateCallback, bool useScaleTime)> toAdd = new();
        
        /// <summary>
        /// Tween an item between two positions over a set amount of time.
        /// Note: This action could potentially be delayed by a single frame.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="transform"></param>
        /// <param name="time"></param>
        public void Tween(Vector2 start, Vector2 end, Transform transform, float time, OnTweenComplete callback, bool useScaledTime = true, EasingFunction easing = null, OnTweenUpdate update = null, bool useLocalTransform = false)
        {
            toAdd.Add((new StaticTween(start, end, time, transform, useLocalTransform, easing), callback, update, useScaledTime));
        }

        /// <summary>
        /// Tween an item between two positions over a set amount of time.
        /// This tween is dynamic and follows the start and endpoints.
        /// Note: This action could potentially be delayed by a single frame.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="transform"></param>
        /// <param name="time"></param>
        public void Tween(Transform start, Transform end, Transform transform, float time, OnTweenComplete callback, bool useScaledTime = true, EasingFunction easing = null, OnTweenUpdate update = null)
        {
            toAdd.Add((new DynamicTween(start, end, time, transform, easing), callback, update, useScaledTime));
        }

        /// <summary>
        /// Tween a recttransform between two positions over a set amount of time.
        /// Note: This action could potentially be delayed by a single frame.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="transform"></param>
        /// <param name="time"></param>
        public void Tween(Vector2 start, Vector2 end, RectTransform transform, float time, OnTweenComplete callback, bool useScaleTime = true, EasingFunction easing = null, OnTweenUpdate update = null)
        {
            toAdd.Add((new RectTween(start, end, time, transform, easing), callback, update, useScaleTime));
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
            /* Start each frame by adding the next set of tweens to the list */
            if (toAdd.Count > 0)
            {
                foreach (var item in toAdd)
                {
                    currentTweens.Add(item);
                }
                toAdd.Clear();
            }
            
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