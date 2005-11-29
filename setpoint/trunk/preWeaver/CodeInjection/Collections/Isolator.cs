using System;
using System.Collections;

namespace Rail.Collections{
	public class Isolator: IEnumerable {
		internal class IsolatedEnumerator: IEnumerator {
			ArrayList items = new ArrayList();
			int currentItem;

			internal IsolatedEnumerator(IEnumerator enumerator) {
				while (enumerator.MoveNext() != false) {
					items.Add(enumerator.Current);
				}
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null) {
					disposable.Dispose();
				}
				currentItem = -1;
			}

			public void Reset() {
				currentItem = -1;
			}

			public bool MoveNext() {
				currentItem++;
				if (currentItem == items.Count)
					return false;

				return true;
			}

			public object Current {
				get {
					return items[currentItem];
				}
			}
		}

		public Isolator(IEnumerable enumerable) {
			this.enumerable = enumerable;
		}

		public IEnumerator GetEnumerator() {
			return new IsolatedEnumerator(enumerable.GetEnumerator());
		}

		IEnumerable enumerable;
	}
	
}