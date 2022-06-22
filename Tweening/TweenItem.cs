using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GeneralUnityUtils.Easing.Easing;

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
        public EasingFunction Easing;

        protected float currTime;

        public TweenItem(float time, Transform target, EasingFunction easing)
        {
            TweenTime = time;
            TargetTransform = target;
            Easing = easing;
            currTime = 0.0f;
        }

        /// <summary>
        /// Move the tweened item.
        /// </summary>
        /// <param name="deltaTime">The amount of time to tween.</param>
        /// <param name="useLocal">Should this update use the local position or global position</param>
        /// <returns>True if at destination, else false.</returns>
        abstract public bool UpdatePosition(float deltaTime);

        public float GetPercentage() { return currTime / TweenTime; }
    }

    public class StaticTween : TweenItem
    {
        public Vector2 Start;
        public Vector2 End;
        public bool UseLocalPosition;

        public StaticTween(Vector2 start, Vector2 end, float time, Transform target, bool useLocalPosition, EasingFunction easing) : base(time, target, easing)
        {
            Start = start;
            End = end;
            UseLocalPosition = useLocalPosition;
        }

        public override bool UpdatePosition(float deltaTime)
        {
            currTime += deltaTime;

            if(base.currTime >= TweenTime)
            {
                if (UseLocalPosition)
                {
                    TargetTransform.localPosition = End;
                }
                else
                {
                    TargetTransform.position = End;
                }
                
                return true;
            }
            else
            {
                float amount = currTime / TweenTime;
                if (Easing != null)
                {
                    amount = Easing(amount);
                }

                if (UseLocalPosition)
                {
                    TargetTransform.localPosition = Vector2.Lerp(Start, End, amount);
                }
                else
                {
                    TargetTransform.position = Vector2.Lerp(Start, End, amount);
                }

                
                return false;
            }
        }
    }

    public class RectTween : TweenItem
    {
        public Vector2 Start;
        public Vector2 End;
        public RectTransform Rect;

        public RectTween(Vector2 start, Vector2 end, float time, RectTransform target, EasingFunction easing) : base(time, target, easing)
        {
            Start = start;
            End = end;
            Rect = target;
        }

        public override bool UpdatePosition(float deltaTime)
        {
            currTime += deltaTime;

            if (base.currTime >= TweenTime)
            {
                Rect.anchoredPosition = End;
                return true;
            }
            else
            {
                float amount = currTime / TweenTime;
                if (Easing != null)
                {
                    amount = Easing(amount);
                }

                Rect.anchoredPosition = Vector2.Lerp(Start, End, amount);
                return false;
            }
        }
    }

    public class DynamicTween : TweenItem
    {
        public Transform Start;
        public Transform End;

        public DynamicTween(Transform start, Transform end, float time, Transform target, EasingFunction easing) : base(time, target, easing)
        {
            Start = start;
            End = end;
        }

        public override bool UpdatePosition(float deltaTime)
        {
            currTime += deltaTime;

            if (base.currTime >= TweenTime)
            {
                TargetTransform.position = End.position;
                return true;
            }
            else
            {
                float amount = currTime / TweenTime;
                if (Easing != null)
                {
                    amount = Easing(amount);
                }

                TargetTransform.position = Vector2.Lerp(Start.position, End.position, amount);
                return false;
            }
        }
    }
}