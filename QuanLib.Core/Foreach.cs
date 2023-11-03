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
            if (enumerable1 is null)
                throw new ArgumentNullException(nameof(enumerable1));
            if (enumerable2 is null)
                throw new ArgumentNullException(nameof(enumerable2));
            if (body is null)
                throw new ArgumentNullException(nameof(body));

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
            if (enumerable1 is null)
                throw new ArgumentNullException(nameof(enumerable1));
            if (enumerable2 is null)
                throw new ArgumentNullException(nameof(enumerable2));
            if (body is null)
                throw new ArgumentNullException(nameof(body));

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
            if (enumerable1 is null)
                throw new ArgumentNullException(nameof(enumerable1));
            if (enumerable2 is null)
                throw new ArgumentNullException(nameof(enumerable2));
            if (enumerable3 is null)
                throw new ArgumentNullException(nameof(enumerable3));
            if (body is null)
                throw new ArgumentNullException(nameof(body));

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
            if (enumerable1 is null)
                throw new ArgumentNullException(nameof(enumerable1));
            if (enumerable2 is null)
                throw new ArgumentNullException(nameof(enumerable2));
            if (enumerable3 is null)
                throw new ArgumentNullException(nameof(enumerable3));
            if (body is null)
                throw new ArgumentNullException(nameof(body));

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
            if (enumerable1 is null)
                throw new ArgumentNullException(nameof(enumerable1));
            if (enumerable2 is null)
                throw new ArgumentNullException(nameof(enumerable2));
            if (enumerable3 is null)
                throw new ArgumentNullException(nameof(enumerable3));
            if (enumerable4 is null)
                throw new ArgumentNullException(nameof(enumerable4));
            if (body is null)
                throw new ArgumentNullException(nameof(body));

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
            if (enumerable1 is null)
                throw new ArgumentNullException(nameof(enumerable1));
            if (enumerable2 is null)
                throw new ArgumentNullException(nameof(enumerable2));
            if (enumerable3 is null)
                throw new ArgumentNullException(nameof(enumerable3));
            if (enumerable4 is null)
                throw new ArgumentNullException(nameof(enumerable4));
            if (body is null)
                throw new ArgumentNullException(nameof(body));

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
