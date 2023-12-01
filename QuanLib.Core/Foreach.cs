using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class Foreach
    {
        public static void Start<T1, T2>(
            IEnumerable<T1> enumerable1,
            IEnumerable<T2> enumerable2,
            Action<T1, T2> body)
        {
            ArgumentNullException.ThrowIfNull(enumerable1, nameof(enumerable1));
            ArgumentNullException.ThrowIfNull(enumerable2, nameof(enumerable2));
            ArgumentNullException.ThrowIfNull(body, nameof(body));

            using var enumerator1 = enumerable1.GetEnumerator();
            using var enumerator2 = enumerable2.GetEnumerator();

            while (enumerator1.MoveNext() && enumerator2.MoveNext())
            {
                body.Invoke(enumerator1.Current, enumerator2.Current);
            }
        }

        public static void Start<T1, T2>(
            IEnumerable<T1> enumerable1,
            IEnumerable<T2> enumerable2,
            Func<T1, T2, bool> body)
        {
            ArgumentNullException.ThrowIfNull(enumerable1, nameof(enumerable1));
            ArgumentNullException.ThrowIfNull(enumerable2, nameof(enumerable2));
            ArgumentNullException.ThrowIfNull(body, nameof(body));

            using var enumerator1 = enumerable1.GetEnumerator();
            using var enumerator2 = enumerable2.GetEnumerator();

            while (enumerator1.MoveNext() && enumerator2.MoveNext())
            {
                if (!body.Invoke(enumerator1.Current, enumerator2.Current))
                    break;
            }
        }

        public static void Start<T1, T2, T3>(
            IEnumerable<T1> enumerable1,
            IEnumerable<T2> enumerable2,
            IEnumerable<T3> enumerable3,
            Action<T1, T2, T3> body)
        {
            ArgumentNullException.ThrowIfNull(enumerable1, nameof(enumerable1));
            ArgumentNullException.ThrowIfNull(enumerable2, nameof(enumerable2));
            ArgumentNullException.ThrowIfNull(enumerable3, nameof(enumerable3));
            ArgumentNullException.ThrowIfNull(body, nameof(body));

            using var enumerator1 = enumerable1.GetEnumerator();
            using var enumerator2 = enumerable2.GetEnumerator();
            using var enumerator3 = enumerable3.GetEnumerator();

            while (enumerator1.MoveNext() && enumerator2.MoveNext() && enumerator3.MoveNext())
            {
                body.Invoke(enumerator1.Current, enumerator2.Current, enumerator3.Current);
            }
        }

        public static void Start<T1, T2, T3>(
            IEnumerable<T1> enumerable1,
            IEnumerable<T2> enumerable2,
            IEnumerable<T3> enumerable3,
            Func<T1, T2, T3, bool> body)
        {
            ArgumentNullException.ThrowIfNull(enumerable1, nameof(enumerable1));
            ArgumentNullException.ThrowIfNull(enumerable2, nameof(enumerable2));
            ArgumentNullException.ThrowIfNull(enumerable3, nameof(enumerable3));
            ArgumentNullException.ThrowIfNull(body, nameof(body));

            using var enumerator1 = enumerable1.GetEnumerator();
            using var enumerator2 = enumerable2.GetEnumerator();
            using var enumerator3 = enumerable3.GetEnumerator();

            while (enumerator1.MoveNext() && enumerator2.MoveNext() && enumerator3.MoveNext())
            {
                if (!body.Invoke(enumerator1.Current, enumerator2.Current, enumerator3.Current))
                    break;
            }
        }

        public static void Start<T1, T2, T3, T4>(
            IEnumerable<T1> enumerable1,
            IEnumerable<T2> enumerable2,
            IEnumerable<T3> enumerable3,
            IEnumerable<T4> enumerable4,
            Action<T1, T2, T3, T4> body)
        {
            ArgumentNullException.ThrowIfNull(enumerable1, nameof(enumerable1));
            ArgumentNullException.ThrowIfNull(enumerable2, nameof(enumerable2));
            ArgumentNullException.ThrowIfNull(enumerable3, nameof(enumerable3));
            ArgumentNullException.ThrowIfNull(enumerable4, nameof(enumerable4));
            ArgumentNullException.ThrowIfNull(body, nameof(body));

            using var enumerator1 = enumerable1.GetEnumerator();
            using var enumerator2 = enumerable2.GetEnumerator();
            using var enumerator3 = enumerable3.GetEnumerator();
            using var enumerator4 = enumerable4.GetEnumerator();

            while (enumerator1.MoveNext() && enumerator2.MoveNext() && enumerator3.MoveNext() && enumerator4.MoveNext())
            {
                body.Invoke(enumerator1.Current, enumerator2.Current, enumerator3.Current, enumerator4.Current);
            }
        }

        public static void Start<T1, T2, T3, T4>(
            IEnumerable<T1> enumerable1,
            IEnumerable<T2> enumerable2,
            IEnumerable<T3> enumerable3,
            IEnumerable<T4> enumerable4,
            Func<T1, T2, T3, T4, bool> body)
        {
            ArgumentNullException.ThrowIfNull(enumerable1, nameof(enumerable1));
            ArgumentNullException.ThrowIfNull(enumerable2, nameof(enumerable2));
            ArgumentNullException.ThrowIfNull(enumerable3, nameof(enumerable3));
            ArgumentNullException.ThrowIfNull(enumerable4, nameof(enumerable4));
            ArgumentNullException.ThrowIfNull(body, nameof(body));

            using var enumerator1 = enumerable1.GetEnumerator();
            using var enumerator2 = enumerable2.GetEnumerator();
            using var enumerator3 = enumerable3.GetEnumerator();
            using var enumerator4 = enumerable4.GetEnumerator();

            while (enumerator1.MoveNext() && enumerator2.MoveNext() && enumerator3.MoveNext() && enumerator4.MoveNext())
            {
                if (!body.Invoke(enumerator1.Current, enumerator2.Current, enumerator3.Current, enumerator4.Current))
                    break;
            }
        }
    }
}
