using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeneralUnityUtils.Tweening
{
    public abstract class TweenItem
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

        public float TweenTime;
        public Transform TargetTransform;

        protected float currTime;

        public TweenItem(float time, Transform target)
        {
            TweenTime = time;
            TargetTransform = target;
            currTime = 0.0f;
        }

        /// <summary>
        /// Move the tweened item.
        /// </summary>
        /// <param name="deltaTime">The amount of time to tween.</param>
        /// <returns>True if at destination, else false.</returns>
        abstract public bool UpdatePosition(float deltaTime);

        public float GetPercentage() { return currTime / TweenTime; }
    }

    public class StaticTween : TweenItem
    {
        public Vector2 Start;
        public Vector2 End;

        public StaticTween(Vector2 start, Vector2 end, float time, Transform target) : base(time, target)
        {
            Start = start;
            End = end;
        }

        public override bool UpdatePosition(float deltaTime)
        {
            currTime += Time.deltaTime;

            if(base.currTime >= TweenTime)
            {
                TargetTransform.position = End;
                return true;
            }
            else
            {
                TargetTransform.position = Vector2.Lerp(Start, End, currTime / TweenTime);
                return false;
            }
        }
    }

    public class RectTween : TweenItem
    {
        public Vector2 Start;
        public Vector2 End;
        public RectTransform Rect;

        public RectTween(Vector2 start, Vector2 end, float time, RectTransform target) : base(time, target)
        {
            Start = start;
            End = end;
            Rect = target;
        }

        public override bool UpdatePosition(float deltaTime)
        {
            currTime += Time.deltaTime;

            if (base.currTime >= TweenTime)
            {
                Rect.anchoredPosition = End;
                return true;
            }
            else
            {
                Rect.anchoredPosition = Vector2.Lerp(Start, End, currTime / TweenTime);
                return false;
            }
        }
    }

    public class DynamicTween : TweenItem
    {
        public Transform Start;
        public Transform End;

        public DynamicTween(Transform start, Transform end, float time, Transform target) : base(time, target)
        {
            Start = start;
            End = end;
        }

        public override bool UpdatePosition(float deltaTime)
        {
            currTime += Time.deltaTime;

            if (base.currTime >= TweenTime)
            {
                TargetTransform.position = End.position;
                return true;
            }
            else
            {
                TargetTransform.position = Vector2.Lerp(Start.position, End.position, currTime / TweenTime);
                return false;
            }
        }
    }
}