using System;
using System.Collections.Generic;
using System.Text;

namespace EFWCoreLib.CoreFrame.Common
{
    public static class CollectionHelper
    {
        #region Find
        /// <summary>
        /// Find 从集合中选取符合条件的元素
        /// </summary>       
        public static IList<TObject> Find<TObject>(IEnumerable<TObject> source, Predicate<TObject> predicate)
        {
            IList<TObject> list = new List<TObject>();
            CollectionHelper.ActionOnSpecification(source, delegate(TObject ele) { list.Add(ele); } , predicate);
            return list;
        }
        #endregion     

        #region FindFirstSpecification
        /// <summary>
        /// FindFirstSpecification 返回符合条件的第一个元素
        /// </summary>      
        public static TObject FindFirstSpecification<TObject>(IEnumerable<TObject> source, Predicate<TObject> predicate)
        {
            foreach (TObject element in source)
            {
                if (predicate(element))
                {
                    return element;
                }
            }

            return default(TObject);
        }
        #endregion

        #region ContainsSpecification
        /// <summary>
        /// ContainsSpecification 集合中是否包含满足predicate条件的元素。
        /// </summary>       
        public static bool ContainsSpecification<TObject>(IEnumerable<TObject> source, Predicate<TObject> predicate, out TObject specification)
        {            
            specification = default(TObject);
            foreach (TObject element in source)
            {
                if (predicate(element))
                {
                    specification = element;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// ContainsSpecification 集合中是否包含满足predicate条件的元素。
        /// </summary>       
        public static bool ContainsSpecification<TObject>(IEnumerable<TObject> source, Predicate<TObject> predicate)
        {
            TObject specification;
            return CollectionHelper.ContainsSpecification<TObject>(source, predicate, out specification);
        }
        #endregion        

        #region ActionOnSpecification
        /// <summary>
        /// ActionOnSpecification 对集合中满足predicate条件的元素执行action。如果没有条件，predicate传入null。
        /// </summary>       
        public static void ActionOnSpecification<TObject>(IEnumerable<TObject> collection, Action<TObject> action, Predicate<TObject> predicate)
        {
            if (collection == null)
            {
                return;
            }

            if (predicate == null)
            {
                foreach (TObject obj in collection)
                {
                    action(obj);
                }

                return;
            }

            foreach (TObject obj in collection)
            {
                if (predicate(obj))
                {
                    action(obj);
                }
            }
        }
        #endregion

        #region ActionOnEach
        /// <summary>
        /// ActionOnEach  对集合中的每个元素执行action。
        /// </summary>        
        public static void ActionOnEach<TObject>(IEnumerable<TObject> collection, Action<TObject> action)
        {
            CollectionHelper.ActionOnSpecification<TObject>(collection, action, null);
        }
        #endregion              

        #region GetPart
        public static T[] GetPart<T>(T[] ary, int startIndex, int count)
        {
            return CollectionHelper.GetPart<T>(ary, startIndex, count, false);
        }

        public static T[] GetPart<T>(T[] ary, int startIndex, int count, bool reverse)
        {
            if (startIndex >= ary.Length)
            {
                return null;
            }

            if (ary.Length < startIndex + count)
            {
                count = ary.Length - startIndex;
            }

            T[] result = new T[count];
            
            if (!reverse)
            {
                for (int i = 0; i < count; i++)
                {
                    result[i] = ary[startIndex + i];
                }                 
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    result[i] = ary[ary.Length - startIndex - 1 - i];
                }
            }

            return result;
        } 
        #endregion

        #region BinarySearch
        /// <summary>
        /// BinarySearch 从已排序的列表中，采用二分查找找到目标在列表中的位置。
        /// 如果刚好有个元素与目标相等，则返回true，且minIndex会被赋予该元素的位置；否则，返回false，且minIndex会被赋予比目标小且最接近目标的元素的位置
        /// </summary>       
        public static bool BinarySearch<T>(IList<T> sortedList, T target, out int minIndex) where T : IComparable
        {
            if (target.CompareTo(sortedList[0]) == 0)
            {
                minIndex = 0;
                return true;
            }

            if (target.CompareTo(sortedList[0]) < 0)
            {
                minIndex = -1;
                return false;
            }

            if (target.CompareTo(sortedList[sortedList.Count - 1]) == 0)
            {
                minIndex = sortedList.Count - 1;
                return true;
            }

            if (target.CompareTo(sortedList[sortedList.Count - 1]) > 0)
            {
                minIndex = sortedList.Count - 1;
                return false;
            }

            int targetPosIndex = -1;
            int left = 0;
            int right = sortedList.Count - 1;

            while (right - left > 1)
            {
                int middle = (left + right) / 2;

                if (target.CompareTo(sortedList[middle]) == 0)
                {
                    minIndex = middle;
                    return true;
                }

                if (target.CompareTo(sortedList[middle]) < 0)
                {
                    right = middle;
                }
                else
                {
                    left = middle;
                }
            }

            minIndex = left;
            return false;
        } 
        #endregion

        #region GetIntersection 、GetUnion
        /// <summary>
        /// GetIntersection 高效地求两个List元素的交集。
        /// </summary>        
        public static List<T> GetIntersection<T>(List<T> list1, List<T> list2) where T : IComparable
        {
            List<T> largList = list1.Count > list2.Count ? list1 : list2;
            List<T> smallList = largList == list1 ? list2 : list1;
            
            largList.Sort();

            int minIndex = 0;

            List<T> result = new List<T>();
            foreach (T tmp in smallList)
            {
                if (CollectionHelper.BinarySearch<T>(largList, tmp, out minIndex))
                {
                    result.Add(tmp);
                }
            }

            return result;
        }      

        /// <summary>
        /// GetUnion 高效地求两个List元素的并集。
        /// </summary> 
        public static List<T> GetUnion<T>(IList<T> list1, IList<T> list2)
        {
            SortedDictionary<T, int> result = new SortedDictionary<T, int>();
            foreach (T tmp in list1)
            {
                if (!result.ContainsKey(tmp))
                {
                    result.Add(tmp ,0);
                }
            }

            foreach (T tmp in list2)
            {
                if (!result.ContainsKey(tmp))
                {
                    result.Add(tmp, 0);
                }
            }

            return (List<T>)CollectionConverter.CopyAllToList<T>(result.Keys);
        } 
        #endregion
    }

   
}
