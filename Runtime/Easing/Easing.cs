
using UnityEngine;

namespace GeneralUnityUtils.Easing
{
    /// <summary>
    /// A general utiity for easing functions.
    /// Shamelessly pulled from https://easings.net/en#
    /// </summary>
    public class Easing
    {
        /// <summary>
        /// A delegate capable of easing a parameter t between 0 and 1.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public delegate float EasingFunction(float t);

        public static float easeInSine(float t)
        {
            return 1 - Mathf.Cos((t * Mathf.PI) / 2);
        }

        public static float easeOutSine(float t)
        {
            return Mathf.Sin((t * Mathf.PI) / 2);
        }

        public static float easeInOutSine(float t)
        {
            return -(Mathf.Cos(Mathf.PI * t) - 1) / 2;
        }

        public static float easeInCubic(float x)
        {
            return x * x * x;
        }

        public static float easeOutCubic(float x)
        {
            return 1 - Mathf.Pow(1 - x, 3);
        }

        public static float easeInOutCubic(float x)
        {
            return x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
        }

        public static float easeInExpo(float x)
        {
            return x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10);
        }

        public static float easeOutExpo(float x)
        {
            return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
        }

        public static float easeInOutExpo(float x)
        {
            return x == 0
              ? 0
              : x == 1
              ? 1
              : x < 0.5 ? Mathf.Pow(2, 20 * x - 10) / 2
              : (2 - Mathf.Pow(2, -20 * x + 10)) / 2;
        }

        const float c1 = 1.70158f;
        const float c2 = (float)(c1 * 1.525);
        const float c3 = c1 + 1;
        const float c4 = (2 * Mathf.PI) / 3;
        const float c5 = (float)((2 * Mathf.PI) / 4.5);
        const float n1 = 7.5625f;
        const float d1 = 2.75f;

        public static float easeInBack(float x)
        {
            

            return c3 * x * x * x - c1 * x * x;
        }

        public static float easeOutBack(float x)
        {
            return 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2);
        }

        public static float easeInOutBack(float x)
        {
            return x < 0.5
              ? (Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
              : (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
        }

        public static float easeInElastic(float x)
        {
            return x == 0
              ? 0
              : x == 1
              ? 1
              : -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10 - 10.75f) * c4);
        }

        public static float easeOutElastic(float x)
        {
            return x == 0
              ? 0
              : x == 1
              ? 1
              : Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * c4) + 1;
        }

        public static float easeInOutElastic(float x)
        {
            return x == 0
              ? 0
              : x == 1
              ? 1
              : x < 0.5
              ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2
              : (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2 + 1;
        }

        public static float easeInBounce(float x)
        {
            return 1 - easeOutBounce(1 - x);
        }

        public static float easeOutBounce(float x)
        {
            if (x < 1 / d1)
            {
                return n1 * x * x;
            }
            else if (x < 2 / d1)
            {
                return n1 * (x -= 1.5f / d1) * x + 0.75f;
            }
            else if (x < 2.5 / d1)
            {
                return n1 * (x -= 2.25f / d1) * x + 0.9375f;
            }
            else
            {
                return n1 * (x -= 2.625f / d1) * x + 0.984375f;
            }
        }

        public static float easeInOutBounce(float x)
        {
            return x < 0.5
              ? (1 - easeOutBounce(1 - 2 * x)) / 2
              : (1 + easeOutBounce(2 * x - 1)) / 2;
        }
    }
}
