namespace setPoint.semantics{
	/// <summary>
	/// A collection of elements of type PointCut
	/// </summary>
	public class MatchPoint: System.Collections.CollectionBase {
		/// <summary>
		/// Initializes a new empty instance of the MatchPoint class.
		/// </summary>
		public MatchPoint() {
			// empty
		}

		/// <summary>
		/// Adds an instance of type PointCut to the end of this MatchPoint.
		/// </summary>
		/// <param name="value">
		/// The PointCut to be added to the end of this MatchPoint.
		/// </param>
		public virtual void Add(PointCut value) {
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic PointCut value is in this MatchPoint.
		/// </summary>
		/// <param name="value">
		/// The PointCut value to locate in this MatchPoint.
		/// </param>
		/// <returns>
		/// true if value is found in this MatchPoint;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(PointCut value) {
			return this.List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this MatchPoint
		/// </summary>
		/// <param name="value">
		/// The PointCut value to locate in the MatchPoint.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(PointCut value) {
			return this.List.IndexOf(value);
		}

		/// <summary>
		/// Inserts an element into the MatchPoint at the specified index
		/// </summary>
		/// <param name="index">
		/// The index at which the PointCut is to be inserted.
		/// </param>
		/// <param name="value">
		/// The PointCut to insert.
		/// </param>
		public virtual void Insert(int index, PointCut value) {
			this.List.Insert(index, value);
		}

		/// <summary>
		/// Gets or sets the PointCut at the given index in this MatchPoint.
		/// </summary>
		public virtual PointCut this[int index] {
			get {
				return (PointCut) this.List[index];
			}
			set {
				this.List[index] = value;
			}
		}

		/// <summary>
		/// Removes the first occurrence of a specific PointCut from this MatchPoint.
		/// </summary>
		/// <param name="value">
		/// The PointCut value to remove from this MatchPoint.
		/// </param>
		public virtual void Remove(PointCut value) {
			this.List.Remove(value);
		}

		/// <summary>
		/// Type-specific enumeration class, used by MatchPoint.GetEnumerator.
		/// </summary>
		public class Enumerator: System.Collections.IEnumerator {
			private System.Collections.IEnumerator wrapped;

			public Enumerator(MatchPoint collection) {
				this.wrapped = ((System.Collections.CollectionBase)collection).GetEnumerator();
			}

			public PointCut Current {
				get {
					return (PointCut) (this.wrapped.Current);
				}
			}

			object System.Collections.IEnumerator.Current {
				get {
					return (PointCut) (this.wrapped.Current);
				}
			}

			public bool MoveNext() {
				return this.wrapped.MoveNext();
			}

			public void Reset() {
				this.wrapped.Reset();
			}
		}

		/// <summary>
		/// Returns an enumerator that can iterate through the elements of this MatchPoint.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new virtual MatchPoint.Enumerator GetEnumerator() {
			return new MatchPoint.Enumerator(this);
		}
	}
}