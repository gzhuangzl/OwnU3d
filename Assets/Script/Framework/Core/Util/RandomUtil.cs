using System;
using UnityEngine;

namespace Framework
{
    public class RandomUtil
    {
        /// <summary>
        /// 求得一组权重数据值中，最触发的权重下标
        /// </summary>
        /// <param name="probs">一组权重数据，每个数据都有不同的权重</param>
        /// <returns>被触发的权重值的下标</returns>
        public int Choose(float[] probs)
        {
            float totalProbs = 0;
            for(int i = 0; i < probs.Length; i++)
            {
                totalProbs += probs[i];
            }

            float randomPoint = UnityEngine.Random.value * totalProbs;
            for(int i = 0; i < probs.Length; i++)
            {
                if(randomPoint <= probs[i])
                {
                    return i;
                }
                else
                {
                    randomPoint -= probs[i];
                }
            }
            return probs.Length - 1;
        }
        /// <summary>
        /// 从一个Set数组中，随机取出特定数目的元素且保证不能重复，并返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceSet">set数组</param>
        /// <param name="requiredNum">需要的数目</param>
        /// <returns></returns>
        public T[] ChooseSet<T>(T[] sourceSet,int requiredNum)
        {
            T[] target;
            if (sourceSet.Length <= requiredNum)
            {
                target = new T[sourceSet.Length];
                Array.Copy(sourceSet, target, sourceSet.Length);
            }
            else
            {
                target = new T[requiredNum];
                for (int leftNum = sourceSet.Length; leftNum > 0; leftNum--)
                {
                    float prob = requiredNum / (float)leftNum;
                    if(UnityEngine.Random.value <= prob)
                    {
                        target[--requiredNum] = sourceSet[leftNum - 1];

                        if(requiredNum == 0)
                        {
                            break;
                        }
                    }
                }
            }
            return target;
        }
    }
}
