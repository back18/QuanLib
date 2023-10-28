using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.Core
{
    public static class Foreach
    {
        public static void Start<TValue1, Tvalue2>(
            IEnumerable<TValue1> enumerable1,
            IEnumerable<Tvalue2> enumerable2,
            Action<TValue1, Tvalue2> body)
        {
            if (enumerable1 is null)
                throw new ArgumentNullException(nameof(enumerable1));
            if (enumerable2 is null)
                throw new ArgumentNullException(nameof(enumerable2));
            if (body is null)
                throw new ArgumentNullException(nameof(body));

            using var enumerator1 = enumerable1.GetEnumerator();
            using var enumerator2 = enumerable2.GetEnumerator();
            enumerator1.Reset();
            enumerator2.Reset();

            while (enumerator1.MoveNext() && enumerator2.MoveNext())
            {
                body.Invoke(enumerator1.Current, enumerator2.Current);
            }
        }

        public static void Start<TValue1, TValue2, TValue3>(
            IEnumerable<TValue1> enumerable1,
            IEnumerable<TValue2> enumerable2,
            IEnumerable<TValue3> enumerable3,
            Action<TValue1, TValue2, TValue3> body)
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
            enumerator1.Reset();
            enumerator2.Reset();
            enumerator3.Reset();

            while (enumerator1.MoveNext() && enumerator2.MoveNext() && enumerator3.MoveNext())
            {
                body.Invoke(enumerator1.Current, enumerator2.Current, enumerator3.Current);
            }
        }

        public static void Start<TValue1, TValue2, TValue3, TValue4>(
            IEnumerable<TValue1> enumerable1,
            IEnumerable<TValue2> enumerable2,
            IEnumerable<TValue3> enumerable3,
            IEnumerable<TValue4> enumerable4,
            Action<TValue1, TValue2, TValue3, TValue4> body)
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
            enumerator1.Reset();
            enumerator2.Reset();
            enumerator3.Reset();
            enumerator4.Reset();

            while (enumerator1.MoveNext() && enumerator2.MoveNext() && enumerator3.MoveNext() && enumerator4.MoveNext())
            {
                body.Invoke(enumerator1.Current, enumerator2.Current, enumerator3.Current, enumerator4.Current);
            }
        }
    }
}
